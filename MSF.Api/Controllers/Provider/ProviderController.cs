using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Provider;

namespace MSF.Api.Controllers.Provider
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet("Lazy")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LazyProviders(string filter, int take, int skip)
        {
            var providers = await _providerService.LazyProvidersViewModelAsync(filter, take, skip);
            return Ok(providers);
        }

        [HttpGet("Find")]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> FindByFilter(string filter)
        {
            var providers = await _providerService.FindByFilter(filter);
            return Ok(providers);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Provider provider)
        {
            await _providerService.AddAsync(provider);
            return Created($"/api/provider/{provider.Id}", new Domain.Models.Provider { Id = provider.Id });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Provider provider)
        {
            await _providerService.UpdateAsync(provider);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var provider = await _providerService.FindAsync(id);

            if (provider is null)
                return NotFound();

            await _providerService.DeleteAsync(provider);

            return Ok();
        }

    }
}
