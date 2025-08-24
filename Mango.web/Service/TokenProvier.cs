using Mango.web.Service.IService;
using Mango.web.Models.Utility;
using Newtonsoft.Json.Linq;

namespace Mango.web.Service
{
    public class TokenProvier : ITokenProvier
    {
        private readonly IHttpContextAccessor contextAccessor;
        public TokenProvier(IHttpContextAccessor _ContextAccessor)
        {
            contextAccessor = _ContextAccessor;
        }

        public void ClearToken()
        {
            contextAccessor.HttpContext?.Response.Cookies.Delete(SD.TookenCookie);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = contextAccessor.HttpContext?.Request.Cookies.TryGetValue(SD.TookenCookie, out token);
            return ((hasToken == true) ? token : null);
        }

        public void SetToken(string token)
        {
            contextAccessor.HttpContext?.Response.Cookies.Append(SD.TookenCookie, token);
        }
    }
}
