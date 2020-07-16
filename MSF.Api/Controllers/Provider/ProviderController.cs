using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Provider;

namespace MSF.Api.Controllers.Provider
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [HttpGet("Lazy")]
        public async Task<IActionResult> LazyProviders(string filter, int take, int skip)
        {
            var providers = await _providerService.LazyProvidersViewModelAsync(filter, take, skip);
            return Ok(providers);
        }

    }
}
