using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Service;
using LibrarySystem.DTOs;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/Category")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _service;

        public CategoryController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Add(CategoryCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");

            _service.AddCategory(dto);
            return Ok("Category added successfully");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.ListCategories());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _service.GetCategoryById(id);
            {
                try
                {
                    return Ok(_service.GetCategoryById(id));
                }
                catch (Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult Edit(int id, [FromBody] CategoryUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            _service.EditCategory(id, dto);
            return Ok("Category updated successfully");
        }

        [HttpPut("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteCategory(id);
                return Ok("Category deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
