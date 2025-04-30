using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<CategoryToReturnDTO>> CreateCategory(CategoryToAddDTO categoryToAddDTO)
        {
            var createdCategory = await _categoryService.AddCategoryAsync(categoryToAddDTO);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<CategoryToReturnDTO>>> GetCatgories(int pageIndex, int pageSize)
        {
            var pagedResponse = await _categoryService.GetCategoriesPaginatedAsync(pageIndex, pageSize);
            return Ok(pagedResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryToReturnDTO>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<ActionResult<CategoryToReturnDTO>> UpdateCategory(int id, CategoryToUpdateDTO categoryToUpdateDTO)
        {
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryToUpdateDTO);
            return Ok(updatedCategory);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
