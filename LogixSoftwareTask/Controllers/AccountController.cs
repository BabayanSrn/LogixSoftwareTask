using LogixSoftwareTask.BusinessLogicLayer.Interfaces;
using LogixSoftwareTask.Storage.Entities;
using LogixSoftwareTask.Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogixSoftwareTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                model.Address = _userService.TransformAddress(model.Address);

                var result = await _userService.RegisterAsync(model);

                if (result != null)
                {
                    return Ok(new { Token = result });
                }

                return BadRequest(new { Message = "Registration failed" });
            }

            return BadRequest(new { Message = "Invalid registration data", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginAsync(model);

                if (result != null)
                {
                    return Ok(new { Token = result });
                }

                return Unauthorized(new { Message = "Invalid login attempt" });
            }

            return BadRequest(new { Message = "Invalid login data", Errors = ModelState.Values.SelectMany(v => v.Errors) });
        }
    }
}
