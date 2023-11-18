using LogixSoftwareTask.BusinessLogicLayer.Interfaces;
using LogixSoftwareTask.Storage.Entities;
using LogixSoftwareTask.Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogixSoftwareTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService, IClassesService classesService)
        {
            _userService = userService;
        }


        [HttpGet("my-profile")]
        public async Task<IActionResult> MyProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized(new { Message = "Unable to retrieve user information" });
                }

                var user = await _userService.GetUserByIdAsync(int.Parse(userId));

                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error retrieving user profile: {ex.Message}" });
            }
        }


        [HttpGet("my-classes")]
        public async Task<IActionResult> MyClasses()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized(new { Message = "Unable to retrieve user information" });
                }

                var user = await _userService.GetUserByIdAsync(int.Parse(userId));

                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }

                return Ok(user.Classes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error retrieving user classes: {ex.Message}" });
            }
        }

        [HttpPut("edit-my-classes")]
        public async Task<IActionResult> EditMyClasses([FromBody] List<ClassModel> updatedClasses)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized(new { Message = "Unable to retrieve user information" });
                }

                 await _userService.EditClassAsync(updatedClasses, userId);

                return Ok("Success");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error updating user classes: {ex.Message}" });
            }
        }
    }
}
