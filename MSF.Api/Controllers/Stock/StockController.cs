using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Stock;
using System.Threading.Tasks;

namespace MSF.Api.Controllers.Stock
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
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

    }
}
