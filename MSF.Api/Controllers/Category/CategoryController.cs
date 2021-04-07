using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSF.Service.Category;

namespace MSF.Api.Controllers.Category
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("Lazy")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LazyCategories(string filter, int take, int skip)
        {
            var categories = await _categoryService.LazyCategoriesViewModelAsync(filter, take, skip);
            return Ok(categories);
        }

        [HttpGet("Find")]
        [Authorize(Roles = "Admin, Estoque")]
        public async Task<IActionResult> FindByFilter(string filter)
        {
            var result = await _categoryService.FindByFilter(filter);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Category category)
        {
            await _categoryService.AddAsync(category);
            return Created($"/api/category/{category.Id}", new Domain.Models.Category { Id = category.Id });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Category category)
        {
            await _categoryService.UpdateAsync(category);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.FindAsync(id);

            if (category is null)
                return NotFound();

            await _categoryService.DeleteAsync(category);

            return Ok();
        }

    }
}
