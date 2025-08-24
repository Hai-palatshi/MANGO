using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Service.Iservice
{
    public interface IJWTTokenGenerator
    {
        public string GenerateToken(ApplicationUser applicationUser,IEnumerable<string> roles);

        //string GenerateRefreshToken();

        //bool ValidateToken(string token);

        //string GetUserIdFromToken(string token);

        //string GetUserNameFromToken(string token);

        //string GetEmailFromToken(string token);

        //string GetRoleFromToken(string token);
    }
}
