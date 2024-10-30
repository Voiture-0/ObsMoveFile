# OBS Move File
A simple program that moves an OBS recording once the file has finished saving.

Uses the OBS WebSocket Server to listen to when a recording has stopped.
Open the appsettings.json to configure options. Remember to set your password either in the appsettings.json file for the "WebSocketPassword" value, or create an Environment Variable named `WebSocketPassword` setting it to your password instead. Also, ensure the port is correct.