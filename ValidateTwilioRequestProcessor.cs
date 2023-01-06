using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Twilio.AspNet.Core;

namespace TwilioFastEndpoints;

public class ValidateTwilioRequestProcessor<TRequest> : IPreProcessor<TRequest>
{
    public Task PreProcessAsync(
        TRequest request, 
        HttpContext httpContext, 
        List<ValidationFailure> failures, 
        CancellationToken ct
    )
    {
        var httpRequest = httpContext.Request;
        var options = httpContext.Resolve<IOptions<TwilioRequestValidationOptions>>().Value;
        if(string.IsNullOrEmpty(options.AuthToken)) 
            throw new Exception("Twilio Auth Token not configured.");
        
        var baseUrlOverride = options.BaseUrlOverride?.TrimEnd('/');

        string? urlOverride = null;
        if (options.BaseUrlOverride != null)
        {
            urlOverride = $"{baseUrlOverride}{httpRequest.Path}{httpRequest.QueryString}";
        }
        
        if (!RequestValidationHelper.IsValidRequest(httpContext, options.AuthToken, urlOverride, options.AllowLocal ?? true))
        {
            return httpContext.Response.SendForbiddenAsync(ct);
        }

        return Task.CompletedTask;
    }
}