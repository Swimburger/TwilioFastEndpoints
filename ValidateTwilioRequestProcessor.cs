using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Twilio.AspNet.Core;

namespace TwilioSmsFastEndpoints;

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
        var options = httpContext.RequestServices.GetRequiredService<IOptions<TwilioRequestValidationOptions>>().Value;
        var baseUrlOverride = options.BaseUrlOverride?.TrimEnd('/');

        string? urlOverride = null;
        if (options.BaseUrlOverride != null)
        {
            urlOverride = $"{baseUrlOverride}{httpRequest.Path}{httpRequest.QueryString}";
        }
        
        if (!RequestValidationHelper.IsValidRequest(httpContext, options.AuthToken, urlOverride, options.AllowLocal ?? true))
        {
            return httpContext.Response.SendForbiddenAsync();
        }

        return Task.CompletedTask;
    }
}