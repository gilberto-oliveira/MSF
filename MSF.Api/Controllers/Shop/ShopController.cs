using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Shop;

namespace MSF.Api.Controllers.Shop
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
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
