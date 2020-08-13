using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Domain.ViewModels;
using MSF.Service.WorkCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Api.Controllers.WorkCenter
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class WorkCenterController: ControllerBase
    {
        private readonly IWorkCenterService _workCenterService;

        public WorkCenterController(IWorkCenterService workCenterService)
        {
            _workCenterService = workCenterService;
        }

        [HttpGet("Lazy")]
        public async Task<IActionResult> LazyWorkCenters(string filter, int take, int skip)
        {
            var workCenters = await _workCenterService.LazyWorkCentersViewModelAsync(filter, take, skip);
            return Ok(workCenters);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Domain.Models.WorkCenter workCenter)
        {
            await _workCenterService.AddAsync(workCenter);
            return Created($"/api/workcenter/{workCenter.Id}", new Domain.Models.WorkCenter { Id = workCenter.Id });
        }

        [HttpPost("Start")]
        public async Task<IActionResult> Start([FromBody] int id)
        {
            await _workCenterService.StartAsync(id);
            return Created($"/api/workcenter/{id}", new Domain.Models.WorkCenter { Id = id });
        }

        [HttpPost("Close")]
        public async Task<IActionResult> Close([FromBody] int id)
        {
            await _workCenterService.CloseAsync(id);
            return Created($"/api/workcenter/{id}", new Domain.Models.WorkCenter { Id = id });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Domain.Models.WorkCenter workCenter)
        {
            await _workCenterService.UpdateAsync(workCenter);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var workCenter = await _workCenterService.FindAsync(id);

            if (workCenter is null)
                return NotFound();

            await _workCenterService.DeleteAsync(workCenter);

            return Ok();
        }

        [HttpGet("FindByShop")]
        public async Task<IActionResult> FindByShop(int shopId)
        {
            var workCenters = await _workCenterService.FindByShopAsync(shopId);
            return Ok(workCenters);
        }
    }
}
