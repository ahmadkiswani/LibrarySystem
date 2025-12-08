using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepo;

        public CategoryService(IGenericRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task AddCategory(CategoryCreateDto dto)
        {
            bool exists = await _categoryRepo.Query()
                .AnyAsync(c => c.Name == dto.Name);

            if (exists)
                throw new Exception("Category already exists");

            var category = new Category
            {
                Name = dto.Name
            };

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveAsync();
        }

        public async Task EditCategory(int id, CategoryUpdateDto dto)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");

            category.Name = dto.Name;

            await _categoryRepo.UpdateAsync(category);
            await _categoryRepo.SaveAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");

            await _categoryRepo.SoftDeleteAsync(category);
            await _categoryRepo.SaveAsync();
        }

        public async Task<List<CategoryListDto>> ListCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();

            return categories
                .Select(c => new CategoryListDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}
