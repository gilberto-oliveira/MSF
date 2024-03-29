﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.WorkCenterControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSF.Api.Controllers.WorkCenterControl
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkCenterControlController : ControllerBase
    {
        private readonly IWorkCenterControlService _workCenterControlService;

        public WorkCenterControlController(IWorkCenterControlService workCenterControlService)
        {
            _workCenterControlService = workCenterControlService;
        }

        [HttpGet("LazyOpenedByWorkCenter")]
        [Authorize(Roles = "Admin, Vendedor")]
        public async Task<IActionResult> LazyOpenedByWorkCenter(int workCenterId)
        {
            var workCenterControl = await _workCenterControlService.LazyOpenedByWorkCenterAsync(workCenterId);
            return Ok(workCenterControl);
        }

        [HttpPut("FinishSaleProcess")]
        [Authorize(Roles = "Admin, Vendedor")]
        public async Task<IActionResult> FinishSaleProcess([FromBody] int workCenterId)
        {
            await _workCenterControlService.FinishSaleProcessAsync(workCenterId);
            return Ok();
        }
    }
}
