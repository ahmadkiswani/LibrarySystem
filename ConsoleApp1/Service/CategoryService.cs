using LibrarySystem.DTOs;
using LibrarySystem.DTOs.CategoryDtos;
using LibrarySystem.Models;
using LibrarySystem.Repository;

namespace LibrarySystem.Service
{
    public class CategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;

        public CategoryService(IGenericRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task AddCategory(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveAsync();
        }

        public async Task EditCategory(int id, CategoryUpdateDto dto)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null || category.IsDeleted)
                throw new Exception("Category not found");

            category.Name = dto.Name;
            category.LastModifiedBy = 1;
            category.LastModifiedDate = DateTime.Now;

            await _categoryRepo.Update(category);
            await _categoryRepo.SaveAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");

            category.IsDeleted = true;
            category.DeletedBy = 1;
            category.DeletedDate = DateTime.Now;

            await _categoryRepo.Update(category);
            await _categoryRepo.SaveAsync();
        }

        public async Task<List<CategoryListDto>> ListCategories()
        {
            var categories = await _categoryRepo.FindAsync(c => !c.IsDeleted);
            return categories.Select(c => new CategoryListDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}
