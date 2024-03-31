using MKopa.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.UseConfiguredHttpClient()
    .UseSqlLite()
    .UseMasstransitExtension()
    .UseSerilogExtension()
    .UseServicesExtension();


var app = builder.Build();

app.MapGet("/", () => "MKopa Sms Service Started!");

app.TriggerDatabaseUpdate();

app.Run();

