using Mango.Services.web.Models;
using Mango.web.Models;
using Mango.web.Models.Utility;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ITokenProvier tokenProvier;
        private readonly IAuthService authService;
        public AuthController(IAuthService _authService, ITokenProvier _tokenProvier)
        {
            authService = _authService;
            tokenProvier = _tokenProvier;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto? response = await authService.LoginAsync(obj);
            if (response != null && response.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(response.Result));

                await SignInUser(loginResponseDto);
                tokenProvier.SetToken(loginResponseDto.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomerError", response.Message);
                return View(obj);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var roleList = new List<SelectListItem>
            {
              new SelectListItem
              {
                  Text = SD.RouleAdmin,
                  Value = SD.RouleAdmin
              },
              new SelectListItem
              {
                  Text = SD.RouleCustomer,
                  Value = SD.RouleCustomer
              },
            };
            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto obj)
        {
            ResponseDto? result = await authService.RegisterAsync(obj);
            ResponseDto? assignRole;

            if (result.IsSuccess && result != null)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = SD.RouleCustomer;
                }
                assignRole = await authService.AssignRoleAsync(obj);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "User registered successfully! Please login.";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result.Message;
            }

            var roleList = new List<SelectListItem>
            {
              new SelectListItem
              {
                  Text = SD.RouleAdmin,
                  Value = SD.RouleAdmin
              },
              new SelectListItem
              {
                  Text = SD.RouleCustomer,
                  Value = SD.RouleCustomer
              },
            };
            ViewBag.RoleList = roleList;
            return View(obj);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            tokenProvier.ClearToken();

            return RedirectToAction("Index","Home");
        }



        private async Task SignInUser(LoginResponseDto model)
        {
            var handlder = new JwtSecurityTokenHandler();
            var jwt = handlder.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
