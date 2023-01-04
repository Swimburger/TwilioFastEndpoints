using Twilio.AspNet.Common;
using Twilio.TwiML;

namespace TwilioSmsFastEndpoints;

public class MessageEndpoint : Endpoint<SmsRequest>
{
    public override void Configure()
    {
        Post("/message");
        Description(b => b.Accepts<SmsRequest>("application/x-www-form-urlencoded"));
        PreProcessors(new ValidateTwilioRequestProcessor<SmsRequest>());
    }

    public override async Task HandleAsync(SmsRequest request, CancellationToken ct)
    {
        var messagingResponse = new MessagingResponse();
        messagingResponse.Message($"Ahoy {request.From}!");
        await this.SendTwiML(messagingResponse, ct);
    }
}