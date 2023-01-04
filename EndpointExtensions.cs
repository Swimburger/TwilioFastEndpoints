using System.Xml.Linq;
using Twilio.TwiML;

namespace TwilioSmsFastEndpoints;

public static class EndpointExtensions
{
    public static async Task SendTwiML<TRequest, TResponse>(
        this Endpoint<TRequest, TResponse> endpoint,
        TwiML twiml,
        CancellationToken ct
    ) where TRequest : notnull, new()
    {
        var httpResponse = endpoint.HttpContext.Response;
        httpResponse.StatusCode = StatusCodes.Status200OK;
        httpResponse.Headers.ContentType = "application/xml";
        var xdocument = twiml.ToXDocument();
        await xdocument.SaveAsync(
            httpResponse.Body,
            SaveOptions.DisableFormatting,
            ct
        );
        endpoint.ResponseStarted = true;
    }
}