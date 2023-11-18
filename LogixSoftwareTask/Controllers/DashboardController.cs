using LogixSoftwareTask.BusinessLogicLayer.Interfaces;
using LogixSoftwareTask.Storage.Entities;
using LogixSoftwareTask.Storage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


//rest, ActionResult


namespace LogixSoftwareTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IClassesService _classesService;


        public DashboardController(IUserService userService, IClassesService classesService)
        {
            _userService = userService;
            _classesService = classesService;
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error retrieving all users: {ex.Message}" });
            }
        }


        [HttpPut("EditUser/{userId}")]
        public async Task<IActionResult> EditUser(int userId, [FromBody] UserModel updatedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _userService.UpdateUserAsync(userId, updatedUser);

                if (result)
                {
                    return Ok("User updated successfully.");
                }
                else
                {
                    return NotFound("User not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpDelete("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(userId);

                if (result)
                {
                    return Ok(new { Message = "User deleted successfully" });
                }

                return NotFound(new { Message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error deleting user: {ex.Message}" });
            }
        }



        [HttpGet("GetUserClasses/{userId}")]
        public async Task<IActionResult> GetUserClasses(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);

                if (user != null)
                {
                    var classes = user.Classes.ToList();

                    if (classes != null)
                    {
                        return Ok(classes);
                    }
                    return NotFound(new { Message = "No classes available" });
                }

                return NotFound(new { Message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error retrieving user classes: {ex.Message}" });
            }
        }



        [HttpPost("AssignClassesToUser/{userId}")]
        public async Task<IActionResult> AssignClassesToUser(int userId, [FromBody] AssignClassesModel model)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);

                if (user == null)
                {
                    return NotFound(new { Message = "User not found" });
                }

                var classes = await _classesService.GetClassesByIdsAsync(model.ClassIds);

                if (classes.Count() != model.ClassIds.Count)
                {
                    return BadRequest(new { Message = "One or more classes not found" });
                }

                user.Classes = classes.ToList();

                await _userService.UpdateUserAsync(userId, user); 

                return Ok(new { Message = "Classes assigned successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error assigning classes: {ex.Message}" });
            }
        }
    }
}
