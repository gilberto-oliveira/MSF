using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Operation;

namespace MSF.Api.Controllers.Operation
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Vendedor")]
    public class OperationController : ControllerBase
    {
        private readonly IOperationService _operationService;

        public OperationController(IOperationService operationService)
        {
            _operationService = operationService;
        }

        [HttpGet("Lazy")]
        public async Task<IActionResult> Lazy(int workCenterId, string type, string filter, int take, int skip)
        {
            var operations = await _operationService.LazyViewModelAsync(workCenterId, type, filter, take, skip);
            return Ok(operations);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Operation operation)
        {
            await _operationService.AddAsync(operation);
            return Created($"/api/operation/{operation.Id}", new Domain.Models.Operation { Id = operation.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Operation operation)
        {
            await _operationService.UpdateAsync(operation);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var operation = await _operationService.FindAsync(id);

            if (operation is null)
                return NotFound();

            await _operationService.DeleteAsync(operation);

            return Ok();
        }

        [HttpGet("Find")]
        public async Task<IActionResult> Find(int id)
        {
            var operation = await _operationService.FindAsync(id);
            return Ok(operation);
        }

        [HttpGet("FindTotalPriceByWorkCenterControlAndType")]
        public async Task<IActionResult> FindTotalPriceByWorkCenterControlAndType(int workCenterControlId, string type)
        {
            var totalPrice = await _operationService.FindTotalPriceByWorkCenterControlAndTypeAsync(workCenterControlId, type);
            return Ok(totalPrice);
        }

        [HttpGet("GetSalesByUser")]
        public async Task<IActionResult> GetSalesByUser()
        {
            var salesByUser = await _operationService.GetSalesByUser();
            return Ok(salesByUser);
        }

        [HttpGet("GetSalesByCategory")]
        public async Task<IActionResult> GetSalesByCategory()
        {
            var salesByCategory = await _operationService.GetSalesByCategory();
            return Ok(salesByCategory);
        }
    }
}
