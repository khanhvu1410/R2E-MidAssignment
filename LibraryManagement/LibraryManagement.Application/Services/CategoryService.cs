using FluentValidation;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IValidator<CategoryToAddDTO> _categoryToAddDTOValidator;
        private readonly IValidator<CategoryToUpdateDTO> _categoryToUpdateDTOValidator;

        public CategoryService(IGenericRepository<Category> categoryRepository, 
            IGenericRepository<Book> bookRepository,
            IValidator<CategoryToAddDTO> categoryToAddDTOvalidator, 
            IValidator<CategoryToUpdateDTO> categoryToUpdateDTOValidator)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _categoryToAddDTOValidator = categoryToAddDTOvalidator;
            _categoryToUpdateDTOValidator = categoryToUpdateDTOValidator;
        }

        public async Task<CategoryToReturnDTO> AddCategoryAsync(CategoryToAddDTO categoryToAddDTO)
        {      
            var result = await _categoryToAddDTOValidator.ValidateAsync(categoryToAddDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var addedCategory = await _categoryRepository.AddAsync(categoryToAddDTO.ToCategory());
            return addedCategory.ToCategoryToReturnDTO();
        }

        public async Task<IEnumerable<CategoryToReturnDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => c.ToCategoryToReturnDTO());
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }

            // Check if a book belongs to this category
            var bookExists = await _bookRepository.ExistsAsync(b => b.CategoryId == id);
            if (bookExists)
            {
                throw new BadRequestException($"Category with ID {id} cannot be deleted.");
            }

            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<PagedResponse<CategoryToReturnDTO>> GetCategoriesPaginatedAsync(int pageIndex, int pageSize)
        {
            var pagedResult = await _categoryRepository.GetPagedAsync(pageIndex, pageSize, null);
            var pagedResponse = new PagedResponse<CategoryToReturnDTO>
            {
                Items = pagedResult.Items?.Select(c => c.ToCategoryToReturnDTO()).ToList() ?? default!,
                PageIndex = pagedResult.PageIndex,
                PageSize = pagedResult.PageSize,
                TotalRecords = pagedResult.TotalRecords,
                TotalPages = pagedResult.TotalPages,
                HasPreviousPage = pagedResult.HasPreviousPage,
                HasNextPage = pagedResult.HasNextPage,
            };
            return pagedResponse;
        }

        public async Task<CategoryToReturnDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }
            return category.ToCategoryToReturnDTO();
        }

        public async Task<CategoryToReturnDTO> UpdateCategoryAsync(int id, CategoryToUpdateDTO categoryToUpdateDTO)
        {
            var result = await _categoryToUpdateDTOValidator.ValidateAsync(categoryToUpdateDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }

            category.Name = categoryToUpdateDTO.Name!;

            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            return updatedCategory.ToCategoryToReturnDTO();
        }
    }
}
