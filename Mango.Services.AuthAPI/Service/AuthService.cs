using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.Iservice;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext db;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IJWTTokenGenerator jwtTokenGenerator;
        public AuthService(AppDbContext _db, RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager, IJWTTokenGenerator _JwtTokenGenerator)
        {
            db = _db;
            roleManager = _roleManager;
            userManager = _userManager;
            jwtTokenGenerator = _JwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                var existing = await roleManager.RoleExistsAsync(roleName);
                if (!existing)
                {
                   await roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await userManager.AddToRoleAsync(user, roleName);
                return true;    
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await db.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());
            bool isValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);


            if (user == null || !isValid)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = "",
                };
            }
            var roles = await userManager.GetRolesAsync(user);  
            var token = jwtTokenGenerator.GenerateToken(user,roles);

            UserDto userDto = new UserDto()
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };


            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };
            return loginResponseDto;

        }

        public async Task<string> Register(RegisterationRequestDto registerationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerationRequestDto.Email,
                Email = registerationRequestDto.Email,
                Name = registerationRequestDto.Name,
                PhoneNumber = registerationRequestDto.PhoneNumber,
                NormalizedEmail = registerationRequestDto.Email.ToUpper()
            };

            try
            {
                var result = await userManager.CreateAsync(user, registerationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToRetun = db.ApplicationUsers.First(x => x.UserName == registerationRequestDto.Email);

                    UserDto userDto = new UserDto()
                    {
                        ID = userToRetun.Id,
                        Email = userToRetun.Email ?? "",
                        Name = userToRetun.Name,
                        PhoneNumber = userToRetun.PhoneNumber ?? ""
                    };
                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "error";
        }
    }
}
