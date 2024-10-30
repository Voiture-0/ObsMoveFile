# OBS Move File
A simple program that moves an OBS recording once the file has finished saving.

## Setup
Uses the OBS WebSocket Server to listen to when a recording has stopped.
Open the appsettings.json to configure options. Remember to set your password either in the appsettings.json file for the "WebSocketPassword" value, or create an Environment Variable named `WebSocketPassword` setting it to your password instead. Also, ensure the port is correct.

## Publishing

### Prerequisites
You need to download and install the .NET 8, or later, SDK https://dotnet.microsoft.com/en-us/download/dotnet/8.0

### Publish Command
Run the following command while in the root repo directory.
```powershell
dotnet publish -c Release
```

In the publish directory, the 2 required files are the .exe and appsettings.json files.

Neither the .NET SDK nor Runtime are required to run the published executable, as it is published as a self-contained executable.