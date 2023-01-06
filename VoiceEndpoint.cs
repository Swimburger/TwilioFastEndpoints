using Twilio.AspNet.Common;
using Twilio.TwiML;

namespace TwilioFastEndpoints;

public class VoiceEndpoint : Endpoint<VoiceRequest>
{
    public override void Configure()
    {
        Post("/voice");
        Description(b => b.Accepts<VoiceRequest>("application/x-www-form-urlencoded"));
        PreProcessors(new ValidateTwilioRequestProcessor<VoiceRequest>());
    }
    
    public override async Task HandleAsync(VoiceRequest request, CancellationToken ct)
    {
        var voiceResponse = new VoiceResponse();
        voiceResponse.Say($"Ahoy {AddSpacesBetweenCharacters(request.From)}!");
        await this.SendTwiML(voiceResponse, ct);
    }

    // to spell out individual numbers in <Say>, add space between each number
    public string AddSpacesBetweenCharacters(string s) 
        => s.Aggregate("", (c, i) => c + i + ' ');
}