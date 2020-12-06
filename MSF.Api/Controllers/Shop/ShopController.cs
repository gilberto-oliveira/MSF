using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Shop;

namespace MSF.Api.Controllers.Shop
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet("Lazy")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LazyShops(string filter, int take, int skip)
        {
            var shops = await _shopService.LazyShopsViewModelAsync(filter, take, skip);
            return Ok(shops);
        }

        [HttpGet("FindByUserRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindByUserRole(int userId, int roleId)
        {
            var shops = await _shopService.FindShopsByUserRoleAsync(userId, roleId);
            return Ok(shops);
        }

        [HttpGet("FindByCurrentUser")]
        [Authorize(Roles = "Admin, Vendedor")]
        public async Task<IActionResult> FindByCurrentUser()
        {
            var shops = await _shopService.FindShopsByCurrentUserAsync();
            return Ok(shops);
        }

        [HttpGet("Find")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindShops(string filter)
        {
            var shops = await _shopService.FindShopsViewModelAsync(filter);
            return Ok(shops);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Shop shop)
        {
            await _shopService.AddAsync(shop);
            return Created($"/api/shop/{shop.Id}", new Domain.Models.Shop { Id = shop.Id });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Shop shop)
        {
            await _shopService.UpdateAsync(shop);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var shop = await _shopService.FindAsync(id);

            if (shop is null)
                return NotFound();

            await _shopService.DeleteAsync(shop);

            return Ok();
        }
    }
}
