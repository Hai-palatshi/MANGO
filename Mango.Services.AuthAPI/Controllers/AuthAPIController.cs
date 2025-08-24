using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.Iservice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService authService;
        protected ResponseDto response;
        private readonly IMessageBus messageBus;
        private readonly IConfiguration configuration;
        public AuthAPIController(IConfiguration _configuration, IMessageBus _messageBus, IAuthService _authService)
        {
            authService = _authService;
            response = new ResponseDto();
            messageBus = _messageBus;
            configuration = _configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            var errorMessage = await authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                response.IsSuccess = false;
                response.Message = errorMessage;
                return BadRequest(response);
            }

            var value = configuration.GetValue<string>("TopicAndQueueName:RegisterUserQueue");
            await messageBus.PublishMessage(model.Email, configuration.GetValue<string>("TopicAndQueueName:RegisterUserQueue"));
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await authService.Login(model);

            if (loginResponse.User == null)
            {
                response.IsSuccess = false;
                response.Message = "Username or password is incorrect.";
                return BadRequest(response);
            }
            response.Result = loginResponse;
            response.IsSuccess = true;
            return Ok(response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDto model)
        {
            var loginResponse = await authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!loginResponse)
            {
                response.IsSuccess = false;
                response.Message = "Role assignment failed.";
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
