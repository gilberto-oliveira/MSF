using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Identity.Models;
using MSF.Service.Identity;
using System.Threading.Tasks;

namespace MSF.Api.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("Lazy")]
        public async Task<IActionResult> LazyRoles(string filter, int take, int skip)
        {
            var roles = await _roleService.LazyRoles(take, skip, filter);
            return Ok(roles);
        }

        [HttpGet("LazyByUser")]
        public async Task<IActionResult> LazyRolesByUser(string filter, int take, int skip, int userId)
        {
            var roles = await _roleService.LazyRolesByUser(userId, take, skip, filter);
            return Ok(roles);
        }

        [HttpGet("GetWithoutUser")]
        public async Task<IActionResult> GetRolesWithoutUser(int userId)
        {
            var roles = await _roleService.GetRolesWithoutUser(userId);
            return Ok(roles);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateUserRole(UserRole userRole)
        {
            await _roleService.CreateUserRole(userRole.UserId, userRole.RoleId);
            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUserRole(int userId, int roleId)
        {
            await _roleService.DeleteUserRole(userId, roleId);
            return Ok();
        }

        [HttpPost("CreateUserRoleShop")]
        public async Task<IActionResult> CreateUserRoleShop(UserRoleShop userRoleShop)
        {
            await _roleService.CreateUserRoleShop(userRoleShop.UserId, userRoleShop.RoleId, userRoleShop.ShopId);
            return Ok();
        }

        [HttpDelete("DeleteUserRoleShop")]
        public async Task<IActionResult> DeleteUserRoleShop(int userId, int roleId, int shopId)
        {
            await _roleService.DeleteUserRoleShop(userId, roleId, shopId);
            return Ok();
        }
    }
}
