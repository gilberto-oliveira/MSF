using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSF.Service.State;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MSF.Api.Controllers.State
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        // GET: api/State
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var states = await _stateService.AllAsync();
            return Ok(states);
        }

        // GET: api/State/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var state = await _stateService.FindAsync(id);

            if (state is null)
                return NotFound();

            return Ok(state);
        }

        // GET: api/State/Lazy?filter=
        [HttpGet("Lazy")]
        public async Task<IActionResult> FindAsync(string filter)
        {
            var states = await _stateService.FindAsync(filter);

            if (states is null)
                return NotFound();

            return Ok(states);
        }
    }
}
