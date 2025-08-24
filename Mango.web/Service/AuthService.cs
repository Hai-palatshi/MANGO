using Mango.Services.web.Models;
using Mango.web.Models;
using Mango.web.Models.Utility;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Win32;
using static Mango.web.Models.Utility.SD;

namespace Mango.web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService baseService;
        public AuthService(IBaseService baseService)
        {
            this.baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegisterationRequestDto registrationRequestDto)
        {
            var send = await baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = AuthAPIBase + "/api/auth/AssignRole",
                Data = registrationRequestDto
            },withBearer:false);
            
            return send;
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var send = await baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = AuthAPIBase + "/api/auth/login",
                Data = loginRequestDto
            }, withBearer: false);
            return send;
        }

        public async Task<ResponseDto?> RegisterAsync(RegisterationRequestDto loginRequestDto)
        {
            var send = await baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = AuthAPIBase + "/api/auth/register",
                Data = loginRequestDto
            }, withBearer: false);
            return send;
        }
    }
}
