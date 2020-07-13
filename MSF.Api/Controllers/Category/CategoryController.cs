﻿using System;
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
    [AllowAnonymous]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("Lazy")]
        public async Task<IActionResult> LazyCategories(string filter, int take, int skip)
        {
            var categories = await _categoryService.LazyCategoriesViewModelAsync(filter, take, skip);
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Domain.Models.Category category)
        {
            await _categoryService.AddAsync(category);
            return Created($"/api/driver/{category.Id}", new Domain.Models.Category { Id = category.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Domain.Models.Category category)
        {
            await _categoryService.UpdateAsync(category);
            return Ok();
        }

        [HttpDelete("{id}")]
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
