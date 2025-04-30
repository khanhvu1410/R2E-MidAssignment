using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Category;

namespace LibraryManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<CategoryToReturnDTO> AddCategoryAsync(CategoryToAddDTO categoryToAddDTO);

        public Task<CategoryToReturnDTO> GetCategoryByIdAsync(int id);

        public Task<PagedResponse<CategoryToReturnDTO>> GetCategoriesPaginatedAsync(int pageIndex, int pageSize);

        public Task<CategoryToReturnDTO> UpdateCategoryAsync(int id, CategoryToUpdateDTO categoryToUpdateDTO);

        public Task DeleteCategoryAsync(int id);
    }
}
