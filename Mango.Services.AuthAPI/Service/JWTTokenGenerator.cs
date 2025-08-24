using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.Iservice;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Mango.Services.AuthAPI.Service
{
    public class JWTTokenGenerator : IJWTTokenGenerator
    {
        private readonly JwtOptions jwtOptions; 
        public JWTTokenGenerator(IOptions<JwtOptions> _jwtOptions)
        {
            jwtOptions = _jwtOptions.Value;   
        }

        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email ,applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id),
                new Claim(JwtRegisteredClaimNames.Name,applicationUser.UserName)
            };
            claims.AddRange(roles.Select(roles => new Claim(ClaimTypes.Role, roles)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token valid for 7 days
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
