using Twilio.AspNet.Common;
using Twilio.TwiML;

namespace TwilioSmsFastEndpoints;

public class VoiceEndpoint : Endpoint<SmsRequest>
{
    public override void Configure()
    {
        Post("/voice");
        Description(b => b.Accepts<SmsRequest>("application/x-www-form-urlencoded"));
        PreProcessors(new ValidateTwilioRequestProcessor<SmsRequest>());
    }
    
    public override async Task HandleAsync(SmsRequest request, CancellationToken ct)
    {
        var messagingResponse = new VoiceResponse();
        messagingResponse.Say($"Ahoy {AddSpacesBetweenCharacters(request.From)}!");
        await this.SendTwiML(messagingResponse, ct);
    }

    // to spell out individual numbers in <Say>, add space between each number
    public string AddSpacesBetweenCharacters(string s) 
        => s.Aggregate("", (c, i) => c + i + ' ');
}