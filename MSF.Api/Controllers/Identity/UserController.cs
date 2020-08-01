using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Domain.ViewModels;
using MSF.Identity.Models;
using MSF.Service.Identity;

namespace MSF.Api.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var token = await _userService.Authenticate(user);
            if (token != null) return Ok(token);
            else return Unauthorized(token);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            await _userService.AddAsync(user);
            return Created($"/api/user/{user.Id}", new User { FirstName = user.FirstName });
        }

        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordViewModel user)
        {
            await _userService.ChangePassword(user);
            return Ok();
        }
        
        [HttpPut("ResetPassword")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword([FromBody] int userId)
        {
            var userExists = await _userService.ExistsUser(userId);

            if (!userExists)
                return NotFound();

            await _userService.ResetPassword(userId);

            return Ok();
        }

        [HttpGet("Lazy")]
        [Authorize]
        public async Task<IActionResult> LazyUsers(string filter, int take, int skip)
        {
            var users = await _userService.LazyUserViewModelAsync(filter, take, skip);
            return Ok(users);
        }

        [HttpGet("LazyById")]
        [Authorize]
        public async Task<IActionResult> LazyById(int userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            return Ok(user);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] UserViewModel user)
        {
            var userExists = await _userService.ExistsUser(user.Id);

            if (!userExists)
                return NotFound();

            await _userService.EditUser(user);

            return Created($"/api/user/{user.Id}", new User { FirstName = user.FirstName });
        }
    }
}
