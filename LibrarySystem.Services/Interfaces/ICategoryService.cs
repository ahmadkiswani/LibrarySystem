using LibrarySystem.Shared.DTOs;

namespace LibrarySystem.Services.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategory(CategoryCreateDto dto);
        Task EditCategory(int id, CategoryUpdateDto dto);
        Task DeleteCategory(int id);
        Task<List<CategoryListDto>> ListCategories();
    }
}
