using LibrarySystem.DTOs;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class CategoryService
    {
        private readonly List<Category> _category;
        private int _idCounter = 1;

        public CategoryService(List<Category> category)
        {
            _category = category;
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

        public void RemoveCategory(int id)
        {
            Category existingcategory = _category.FirstOrDefault(c => c.Id == id);

            if (existingcategory != null)
            {
                _category.Remove(existingcategory);
            }
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

        public List<BookListDto> GetBooksByCategoryId(int categoryId)
        {
            List<BookListDto> result = new List<BookListDto>();

            Category category = _category.FirstOrDefault(c => c.Id == categoryId);

            if (category != null)
            {
                foreach (var b in category.Books)
                {
                    BookListDto dto = new BookListDto();
                    dto.Id = b.Id;
                    dto.Title = b.Title;

                    result.Add(dto);
                }
            }

            return result;
        }

        public List<CategoryListDto> ListCategories()
        {
            List<CategoryListDto> result = new List<CategoryListDto>();

            return _category
           .Select(category => new CategoryListDto
           {
               Id = category.Id,
               Name = category.Name
           }).ToList();
        }
        public CategoryDetailsDto GetCategoryById(int id)
        {
            Category category = _category.FirstOrDefault(c => c.Id == id);

            if (category == null)
                return null;

            CategoryDetailsDto dto = new CategoryDetailsDto();
            dto.Id = category.Id;
            dto.Name = category.Name;
            dto.BooksCount = category.Books.Count;

            return dto;
        }
    }
}