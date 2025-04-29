using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<CategoryDTO> AddCategoryAsync(CategoryToAddDTO categoryToAddDTO);

        public Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();

        public Task<CategoryDTO> GetCategoryByIdAsync(int id);

        public Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO categoryDTO);

        public Task DeleteCategoryAsync(int id);
    }
}
