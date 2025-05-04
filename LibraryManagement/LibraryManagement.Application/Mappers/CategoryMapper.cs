using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryToReturnDTO ToCategoryToReturnDTO(this Category category)
        {
            return new CategoryToReturnDTO
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public static Category ToCategory(this CategoryToAddDTO categoryToAddDTO)
        {
            return new Category
            {
                Name = categoryToAddDTO.Name,
            };
        }
    }
}
