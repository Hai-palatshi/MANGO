using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace Mango.Services.ShoppingCartAPI.Utility
{
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor accessor;
        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor _accessor)
        {
            accessor = _accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await accessor.HttpContext.GetTokenAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);

        }
    }
}
