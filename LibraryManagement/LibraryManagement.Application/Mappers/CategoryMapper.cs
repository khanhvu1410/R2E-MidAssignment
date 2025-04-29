using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappers
{
    internal static class CategoryMapper
    {
        public static CategoryDTO ToCategoryDTO(this Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
            };
        }

        public static Category ToCategory(this CategoryDTO categoryDTO)
        {
            return new Category
            {
                Id = categoryDTO.Id,
                Name = categoryDTO.Name,
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
