using LibrarySystem.Data;
using LibrarySystem.DTOs;
using LibrarySystem.DTOs.CategoryDtos;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class CategoryService
    {
        private readonly LibraryDbContext _context;

        public CategoryService(LibraryDbContext context)
        {
            _context = context;
        }

        public void AddCategory(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
                throw new Exception("❗ Category not found");

            category.IsDeleted = true;
            category.DeletedBy = 1;
            category.DeletedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public void EditCategory(int id, CategoryUpdateDto dto)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted);

            if (category == null)
                throw new Exception("❗ Category not found");

            category.Name = dto.Name;
            category.LastModifiedBy = 1;
            category.LastModifiedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public List<CategoryListDto> ListCategories()
        {
            return _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new CategoryListDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }

        public CategoryDetailsDto GetCategoryById(int id)
        {
            var category = _context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.Id == id);

            if (category == null)
                return null;

            return new CategoryDetailsDto
            {
                Id = category.Id,
                Name = category.IsDeleted ? "Unknown" : category.Name,
                BooksCount = category.Books.Count(b => !b.IsDeleted)
            };
        }

        public List<CategoryListDto> Search(CategorySearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;

            return _context.Categories
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
    }
}
