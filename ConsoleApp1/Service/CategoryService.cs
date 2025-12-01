using LibrarySystem.DTOs;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.DTOs.CategoryDtos;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class CategoryService
    {
        private readonly LibraryContext _context;
        private List<Category> _category => _context.Categories;

        public CategoryService(LibraryContext context)
        {
            _context = context;
        }
        public void AddCategory(CategoryCreateDto dto)
        {
            Category category = new Category();
            category.Id = _category.Count + 1;
            category.Name = dto.Name;
            category.CreatedBy = 1;
            category.CreatedDate = DateTime.Now;
            _category.Add(category);
        }
        public void DeleteCategory(int id)
        {
            var existingcategory = _category.FirstOrDefault(c => c.Id == id);

            if (existingcategory == null)
                throw new Exception("❗ Category not found");

            existingcategory.IsDeleted = true;
            existingcategory.DeletedBy = id;
            existingcategory.DeletedDate = DateTime.Now;
            
        }
        public void EditCategory(int id, CategoryUpdateDto dto)
        {
            Category existingcategory = _category.FirstOrDefault(p => p.Id == id);

            if (existingcategory != null)
            {
                existingcategory.Name = dto.Name;
                existingcategory.LastModifiedDate = DateTime.Now;
                existingcategory.LastModifiedBy = 1;
            }
        }


        public List<CategoryListDto> Search(CategorySearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;

            return _category
                .Where(c => !c.IsDeleted)
                .Where(c =>
                    (dto.Text == null || c.Name.ToLower().Contains(dto.Text.ToLower())) &&
                    (dto.Number == null || c.Id == dto.Number)
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryListDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }


        public List<CategoryListDto> ListCategories()
        {
            List<CategoryListDto> result = new List<CategoryListDto>();

            return _category
                .Where(c => !c.IsDeleted)
                .Select(category => new CategoryListDto
                {
                    Id = category.Id,
                    Name = category.Name
                })
                .ToList();
        }
        public CategoryDetailsDto GetCategoryById(int id)
        {
            Category category = _category.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return null;
            CategoryDetailsDto dto = new CategoryDetailsDto();
            dto.Id = category.Id;
            dto.Name = category.IsDeleted ? "Unknown" : category.Name;
            dto.BooksCount = category.Books.Count;

            return dto;
        }
    }
}