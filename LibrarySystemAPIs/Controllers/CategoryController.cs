
using LibrarySystem.Service;
using LibrarySystem.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryCreateDto dto)
        {
            await _service.AddCategory(dto);
            return Ok("Category added successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.ListCategories());
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] CategoryUpdateDto dto)
        {
            await _service.EditCategory(id, dto);
            return Ok("Category updated successfully");
        }

        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteCategory(id);
            return Ok("Category deleted successfully");
        }
    }
}
