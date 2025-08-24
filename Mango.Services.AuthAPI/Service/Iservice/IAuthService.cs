using Mango.Services.AuthAPI.Models.DTO;

namespace Mango.Services.AuthAPI.Service.Iservice
{
    public interface IAuthService
    {
        public Task<string> Register(RegisterationRequestDto registerationRequestDto);
        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        public Task<bool> AssignRole(string email,string roleName);    
    }
}
