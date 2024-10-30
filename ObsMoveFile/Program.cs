using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ObsMoveFile;

var builder = Host.CreateApplicationBuilder(args);

// Binds the AppSettings class to the appsettings.json, for easily reading config data and any changes
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.Run();
