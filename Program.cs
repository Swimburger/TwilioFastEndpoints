global using FastEndpoints;
using Twilio.AspNet.Core;

var builder = WebApplication.CreateBuilder();

builder.Services.AddFastEndpoints();
builder.Services.AddTwilioRequestValidation();

var app = builder.Build();

app.UseFastEndpoints(c => {
    // everything is anonymous for this sample
    c.Endpoints.Configurator = epd => epd.AllowAnonymous();
});

app.Run();