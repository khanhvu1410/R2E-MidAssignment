using FluentValidation;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IValidator<CategoryToAddDTO> _categoryToAddDTOValidator;
        private readonly IValidator<CategoryDTO> _categoryDTOValidator;

        public CategoryService(IGenericRepository<Category> categoryRepository, 
            IGenericRepository<Book> bookRepository,
            IValidator<CategoryToAddDTO> categoryToAddDTOvalidator, 
            IValidator<CategoryDTO> categoryDTOValidator)
        {
            _categoryRepository = categoryRepository;
            _bookRepository = bookRepository;
            _categoryToAddDTOValidator = categoryToAddDTOvalidator;
            _categoryDTOValidator = categoryDTOValidator;
        }

        public async Task<CategoryDTO> AddCategoryAsync(CategoryToAddDTO categoryToAddDTO)
        {      
            var result = await _categoryToAddDTOValidator.ValidateAsync(categoryToAddDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var addedCategory = await _categoryRepository.AddAsync(categoryToAddDTO.ToCategory());
            return addedCategory.ToCategoryDTO();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }

            // Check if a book belongs to this category
            var books = _bookRepository.GetQueryable();
            var booksByCategoryId = books.Where(b => b.CategoryId == id);
            if (booksByCategoryId.Any())
            {
                throw new BadRequestException($"Category with ID {id} cannot be deleted.");
            }

            await _categoryRepository.DeleteAsync(id);
        }

        public async Task<PagedResponse<CategoryDTO>> GetCategoriesPaginatedAsync(int pageIndex, int pageSize)
        {
            var pagedResult = await _categoryRepository.GetPagedAsync(pageIndex, pageSize);
            var pagedResponse = new PagedResponse<CategoryDTO>
            {
                Items = pagedResult.Items?.Select(c => c.ToCategoryDTO()).ToList(),
                PageIndex = pagedResult.PageIndex,
                TotalPages = pagedResult.TotalPages,
                HasPreviousPage = pagedResult.HasPreviousPage,
                HasNextPage = pagedResult.HasNextPage,
            };
            return pagedResponse;
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {id} not found.");
            }
            return category.ToCategoryDTO();
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO categoryDTO)
        {
            var result = await _categoryDTOValidator.ValidateAsync(categoryDTO);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
            var category = await _categoryRepository.GetByIdAsync(categoryDTO.Id);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {categoryDTO.Id} not found.");
            }
            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            return updatedCategory.ToCategoryDTO();
        }
    }
}
