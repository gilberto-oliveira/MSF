﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Product;
using System.Threading.Tasks;

namespace MSF.Api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("Lazy")]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> LazyProducts(string filter, int take, int skip)
        {
            var products = await _productService.LazyProductsViewModelAsync(filter, take, skip);
            return Ok(products);
        }

        [HttpGet("Find")]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> LazyProducts(string filter)
        {
            var products = await _productService.FindByFilter(filter);
            return Ok(products);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Product product)
        {
            await _productService.AddAsync(product);
            return Created($"/api/product/{product.Id}", new Domain.Models.Product { Id = product.Id });
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Product product)
        {
            await _productService.UpdateAsync(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.FindAsync(id);

            if (product is null)
                return NotFound();

            await _productService.DeleteAsync(product);

            return Ok();
        }

        [HttpGet("LazyStats")]
        [Authorize(Roles = "Admin, Vendedor")]
        public async Task<IActionResult> LazyProductStats(string filter, int take, int skip)
        {
            var products = await _productService.LazyProductStatsViewModelAsync(filter, take, skip);
            return Ok(products);
        }
    }
}
