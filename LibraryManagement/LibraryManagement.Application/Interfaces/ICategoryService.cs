using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<CategoryDTO> AddCategoryAsync(CategoryToAddDTO categoryToAddDTO);

        public Task<CategoryDTO> GetCategoryByIdAsync(int id);

        public Task<PagedResponse<CategoryDTO>> GetCategoriesPaginatedAsync(int pageIndex, int pageSize);

        public Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO categoryDTO);

        public Task DeleteCategoryAsync(int id);
    }
}
