using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Stock;
using System.Threading.Tasks;

namespace MSF.Api.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("Lazy")]
        public async Task<IActionResult> LazyProviders(string filter, int take, int skip)
        {
            var providers = await _stockService.LazyStocksViewModelAsync(filter, take, skip);
            return Ok(providers);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Stock stock)
        {
            await _stockService.AddAsync(stock);
            return Created($"/api/stock/{stock.Id}", new Domain.Models.Stock { Id = stock.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Stock stock)
        {
            await _stockService.UpdateAsync(stock);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var stock = await _stockService.FindAsync(id);

            if (stock is null)
                return NotFound();

            await _stockService.DeleteAsync(stock);

            return Ok();
        }

        [HttpGet("FindProviderByFilterAndProduct")]
        public async Task<IActionResult> FindProviderByFilterAndProduct(string filter, int productId)
        {
            var providers = await _stockService.FindProviderByFilterAndProduct(filter, productId);
            return Ok(providers);
        }

        [HttpGet("FindProductByFilter")]
        public async Task<IActionResult> FindProductByFilter(string filter)
        {
            var products = await _stockService.FindProductByFilter(filter);
            return Ok(products);
        }

        [HttpGet("FindTotalPriceByProductAndProvider")]
        public async Task<IActionResult> FindTotalPriceByProductAndProvider(int productId, int providerId)
        {
            var totalPrice = await _stockService.FindTotalPriceByProductAndProvider(productId, providerId);
            return Ok(totalPrice);
        }
    }
}
