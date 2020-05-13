using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Threading.Tasks;

namespace Uno.CustomProviders
{
    public class CustomRouteDataCultureProvider : IRequestCultureProvider
    {
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var culture = httpContext.Request.Path.Value.Split('/')[1]?.ToString();
            
            if (culture == null)
                return null;

            var providerResultCulture = new ProviderCultureResult(culture);

            return Task.FromResult(providerResultCulture);
        }
    }
}