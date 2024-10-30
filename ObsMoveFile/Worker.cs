using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Communication;

namespace ObsMoveFile;

/// <summary>
/// Background worker that manages the OBS WebSocket connection and listens for recording events.
/// </summary>
public class Worker(IOptionsMonitor<AppSettings> options) : IHostedService
{
    private readonly OBSWebsocket _obs = new();
    private bool _connectingOrConnected = false; // Extra variable tracking if we are already attempting to connect

    /// <summary>
    /// Starts the OBS WebSocket connection and subscribes to recording events.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (_obs.IsConnected || _connectingOrConnected) throw new Exception("OBS websocket is already connected???");

            // Register Events
            _obs.Connected += (sender, e) => Console.WriteLine("Connected to OBS websocket!");
            _obs.Disconnected += async (sender, e) => await OnDisconnectedAsync(e);
            _obs.ExitStarted += (sender, e) => Console.WriteLine("OBS has exited!");
            _obs.RecordStateChanged += (sender, e) => RecordStateChangedHandler.Handle(e, options.CurrentValue);
            
            ConnectToObsWebSocketServer();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception while using OBS WebSocket!");
            Console.WriteLine(ex.Message);
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Connects to the OBS WebSocket Server.
    /// </summary>
    private void ConnectToObsWebSocketServer()
    {
        if (_connectingOrConnected)
        {
            Console.WriteLine("ERROR: Invalid state. Attempting to connect to OBS WebSocket Server while _connectingOrConnected = true. Possibly attempting to create extra connection?");
            return;
        }
        
        // Get configuration for OBS WebSocket Server
        var appSettings = options.CurrentValue;
        var port = appSettings.WebSocketPort;
        var password = appSettings.WebSocketPassword;
        if (password == "YOU NEED TO CHANGE THIS IDIOT!")
        {
            Console.WriteLine($"""
                USER ERROR: You need to set your OBS WebSocket Server password in the appsettings.json file.
                  Find it in OBS > Tools > WebSocket Server Settings > Server Password.
                  Edit the file {Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json")} 
                  and set the "WebSocketPassword" value to your password.
                    (you can also set an Environment Variable instead with that same name, setting the value to your password)

                Press ENTER to continue... once your save your appsettings.json file.
                """);
            Console.ReadLine();
            ConnectToObsWebSocketServer();
            return;
        }

        // Establish connection to OBS WebSocket using configured port and password
        _connectingOrConnected = true;
        _obs.ConnectAsync($"ws://localhost:{port}", password);
    }

    /// <summary>
    /// Disconnects from the OBS WebSocket upon service stop.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _obs.Disconnect();
        Console.WriteLine("Worker stopped; Disconnected from OBS WebSocket Server.");
        return Task.CompletedTask;
    }


    /// <summary>
    /// Handles when we are disconnected from the OBS WebSocket Server for any reason.
    /// </summary>
    public async Task OnDisconnectedAsync(ObsDisconnectionInfo e)
    {
        Console.WriteLine($"Disconnected from OBS WebSocket Server! Reason: {e.DisconnectReason}");
        _connectingOrConnected = false;

        var appSettings = options.CurrentValue;
        if (appSettings.RetryConnection)
        {
            Console.WriteLine("Retrying in 5 seconds...");
            await Task.Delay(TimeSpan.FromSeconds(appSettings.RetryIntervalSeconds));

            ConnectToObsWebSocketServer();
        }
    }
}
