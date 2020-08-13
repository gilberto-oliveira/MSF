using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Shop;

namespace MSF.Api.Controllers.Shop
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ShopController : ControllerBase
    {
        private readonly IShopService _shopService;

        public ShopController(IShopService shopService)
        {
            _shopService = shopService;
        }

        [HttpGet("Lazy")]
        public async Task<IActionResult> LazyShops(string filter, int take, int skip)
        {
            var shops = await _shopService.LazyShopsViewModelAsync(filter, take, skip);
            return Ok(shops);
        }

        [HttpGet("FindByUserRole")]
        public async Task<IActionResult> FindByUserRole(int userId, int roleId)
        {
            var shops = await _shopService.FindShopsByUserRoleAsync(userId, roleId);
            return Ok(shops);
        }

        [HttpGet("FindByCurrentUser")]
        public async Task<IActionResult> FindByCurrentUser()
        {
            var shops = await _shopService.FindShopsByCurrentUserAsync();
            return Ok(shops);
        }

        [HttpGet("Find")]
        public async Task<IActionResult> FindShops(string filter)
        {
            var shops = await _shopService.FindShopsViewModelAsync(filter);
            return Ok(shops);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Shop shop)
        {
            await _shopService.AddAsync(shop);
            return Created($"/api/shop/{shop.Id}", new Domain.Models.Shop { Id = shop.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Shop shop)
        {
            await _shopService.UpdateAsync(shop);
            return Ok();
        }

        [HttpDelete("{id}")]
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
