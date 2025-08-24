using Mango.Services.web.Models;
using Mango.web.Models;

namespace Mango.web.Service.IService
{
    public interface IAuthService
    {
        public Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        public Task<ResponseDto?> RegisterAsync(RegisterationRequestDto loginRequestDto);
        public Task<ResponseDto?> AssignRoleAsync(RegisterationRequestDto registrationRequestDto);
    }
}
