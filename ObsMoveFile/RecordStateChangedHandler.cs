using OBSWebsocketDotNet.Types.Events;
using OBSWebsocketDotNet.Types;

namespace ObsMoveFile;

public static class RecordStateChangedHandler
{
    public static void Handle(RecordStateChangedEventArgs e, AppSettings appSettings)
    {
        switch (e.OutputState.State)
        {
            case OutputState.OBS_WEBSOCKET_OUTPUT_STARTED:
                Console.WriteLine($"New file recording to {e.OutputState.OutputPath}");
                break;
            case OutputState.OBS_WEBSOCKET_OUTPUT_STOPPED:
                RecordingMover.MoveFile(appSettings.OutputPaths, e.OutputState.OutputPath);
                break;
            default:
                break;
        }
    }
}
