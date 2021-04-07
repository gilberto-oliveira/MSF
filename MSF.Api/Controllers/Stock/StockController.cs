using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Stock;
using System.Threading.Tasks;

namespace MSF.Api.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet("Lazy")]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> LazyProviders(string filter, int take, int skip)
        {
            var providers = await _stockService.LazyStocksViewModelAsync(filter, take, skip);
            return Ok(providers);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Stock stock)
        {
            await _stockService.AddAsync(stock);
            return Created($"/api/stock/{stock.Id}", new Domain.Models.Stock { Id = stock.Id });
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Stock stock)
        {
            await _stockService.UpdateAsync(stock);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> Delete(int id)
        {
            var stock = await _stockService.FindAsync(id);

            if (stock is null)
                return NotFound();

            await _stockService.DeleteAsync(stock);

            return Ok();
        }

        [HttpGet("FindProviderByFilterAndProduct")]
        [Authorize(Roles = "Admin, Vendedor, Estoque")]
        public async Task<IActionResult> FindProviderByFilterAndProduct(string filter, int productId)
        {
            var providers = await _stockService.FindProviderByFilterAndProduct(filter, productId);
            return Ok(providers);
        }

        [HttpGet("FindProductByFilter")]
        [Authorize(Roles = "Admin, Vendedor, Estoque")]
        public async Task<IActionResult> FindProductByFilter(string filter)
        {
            var products = await _stockService.FindProductByFilter(filter);
            return Ok(products);
        }

        [HttpGet("FindTotalPriceByProductAndProvider")]
        [Authorize(Roles = "Admin, Vendedor, Estoque")]
        public async Task<IActionResult> FindTotalPriceByProductAndProvider(int productId, int providerId)
        {
            var totalPrice = await _stockService.FindTotalPriceByProductAndProvider(productId, providerId);
            return Ok(totalPrice);
        }
    }
}
