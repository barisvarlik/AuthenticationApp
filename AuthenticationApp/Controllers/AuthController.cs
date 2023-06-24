using AuthenticationApp.Core.DTOs;
using AuthenticationApp.Core.Entities;
using AuthenticationApp.Core.Services;
using AuthenticationApp.Service.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult CreateActionResult<D>(ResponseDto<D> response)
        {// If you have a base controller that implements this function, then make this class inherit from that and use it from there.
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode,
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserCredentialsDto userCredentials)
        {
            return CreateActionResult(await _userService.Login(userCredentials));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDto registerInfo)
        {
            return CreateActionResult(await _userService.Register(registerInfo));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(UserCredentialsDto userCredentials)
        {
            string token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", string.Empty);

            return CreateActionResult(_userService.Logout(token));
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto userCredentials)
        {
            return CreateActionResult(await _userService.ChangePassword(userCredentials));
        }

        [HttpDelete("deleteAccount")]
        public async Task<IActionResult> DeleteAccount(User user)
        {
            string token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", string.Empty);

            return CreateActionResult(await _userService.DeleteAccount(user, token));
        }

    }
}
