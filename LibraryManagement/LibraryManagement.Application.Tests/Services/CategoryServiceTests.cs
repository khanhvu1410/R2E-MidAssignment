using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Models;
using Moq;

namespace LibraryManagement.Application.Tests.Services
{
    [TestFixture]
    internal class CategoryServiceTests
    {
        private Mock<IGenericRepository<Category>> _mockCategoryRepository;
        private Mock<IGenericRepository<Book>> _mockBookRepository;
        private Mock<IValidator<CategoryToAddDTO>> _mockCategoryToAddDTOValidator;
        private Mock<IValidator<CategoryToUpdateDTO>> _mockCategoryToUpdateDTOValidator;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            _mockCategoryRepository = new Mock<IGenericRepository<Category>>();
            _mockBookRepository = new Mock<IGenericRepository<Book>>();
            _mockCategoryToAddDTOValidator = new Mock<IValidator<CategoryToAddDTO>>();
            _mockCategoryToUpdateDTOValidator = new Mock<IValidator<CategoryToUpdateDTO>>();

            _categoryService = new CategoryService(
                _mockCategoryRepository.Object,
                _mockBookRepository.Object,
                _mockCategoryToAddDTOValidator.Object,
                _mockCategoryToUpdateDTOValidator.Object
            );
        }

        #region AddCategoryAsync Tests

        [Test]
        public async Task AddCategoryAsync_WithValidData_ReturnsCategoryToReturnDTO()
        {
            // Arrange
            var categoryToAdd = new CategoryToAddDTO { Name = "Fiction" };
            var validationResult = new ValidationResult();

            _mockCategoryToAddDTOValidator.Setup(v => v.ValidateAsync(categoryToAdd, default))
                .ReturnsAsync(validationResult);

            var expectedCategory = new Category { Id = 1, Name = "Fiction" };
            _mockCategoryRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _categoryService.AddCategoryAsync(categoryToAdd);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(expectedCategory.Id));
            Assert.That(result.Name, Is.EqualTo(expectedCategory.Name));
            _mockCategoryToAddDTOValidator.Verify(v => v.ValidateAsync(categoryToAdd, default), Times.Once);
            _mockCategoryRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
        }

        [Test]
        public void AddCategoryAsync_WithInvalidData_ThrowsValidationException()
        {
            // Arrange
            var categoryToAdd = new CategoryToAddDTO();
            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            };
            var validationResult = new ValidationResult(validationErrors);

            _mockCategoryToAddDTOValidator.Setup(v => v.ValidateAsync(categoryToAdd, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => _categoryService.AddCategoryAsync(categoryToAdd));
            Assert.That(ex.Errors.Count(), Is.EqualTo(validationErrors.Count));
            _mockCategoryRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Never);
        }

        #endregion

        #region GetAllCategoriesAsync Tests

        [Test]
        public async Task GetAllCategoriesAsync_ReturnsListOfCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Non-Fiction" }
            };

            _mockCategoryRepository.Setup(r => r.GetAllAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Name, Is.EqualTo("Fiction"));
            _mockCategoryRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        #endregion

        #region DeleteCategoryAsync Tests

        [Test]
        public async Task DeleteCategoryAsync_WithExistingCategory_DeletesCategory()
        {
            // Arrange
            var categoryId = 1;
            var existingCategory = new Category { Id = categoryId };

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(existingCategory);

            _mockBookRepository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(false);

            // Act
            await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            _mockCategoryRepository.Verify(r => r.DeleteAsync(categoryId), Times.Once);
        }

        [Test]
        public void DeleteCategoryAsync_WithNonExistingCategory_ThrowsNotFoundException()
        {
            // Arrange
            var categoryId = 999;
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(null as Category);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _categoryService.DeleteCategoryAsync(categoryId));
            Assert.That(ex.Message, Is.EqualTo($"Category with ID {categoryId} not found."));
            _mockCategoryRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void DeleteCategoryAsync_WhenBooksExistInCategory_ThrowsBadRequestException()
        {
            // Arrange
            var categoryId = 1;
            var existingCategory = new Category { Id = categoryId };

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(existingCategory);

            _mockBookRepository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() => _categoryService.DeleteCategoryAsync(categoryId));
            Assert.That(ex.Message, Is.EqualTo($"Category with ID {categoryId} cannot be deleted."));
            _mockCategoryRepository.Verify(r => r.DeleteAsync(It.IsAny<int>()), Times.Never);
        }

        #endregion

        #region GetCategoriesPaginatedAsync Tests

        [Test]
        public async Task GetCategoriesPaginatedAsync_ReturnsPagedResponse()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var totalRecords = 2;

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Non-Fiction" }
            };

            var pagedResult = new PagedResult<Category>(categories, pageIndex, pageSize, totalRecords);
            
            _mockCategoryRepository.Setup(r => r.GetPagedAsync(pageIndex, pageSize, null))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _categoryService.GetCategoriesPaginatedAsync(pageIndex, pageSize);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.PageIndex, Is.EqualTo(pageIndex));
            Assert.That(result.PageSize, Is.EqualTo(pageSize));
            Assert.That(result.Items?.Count(), Is.EqualTo(2));
            Assert.That(result.Items.ElementAt(0).Name, Is.EqualTo("Fiction"));
        }

        #endregion

        #region GetCategoryByIdAsync Tests

        [Test]
        public async Task GetCategoryByIdAsync_WithExistingCategory_ReturnsCategoryToReturnDTO()
        {
            // Arrange
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Fiction" };

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(categoryId));
            Assert.That(result.Name, Is.EqualTo("Fiction"));
        }

        [Test]
        public void GetCategoryByIdAsync_WithNonExistingCategory_ThrowsNotFoundException()
        {
            // Arrange
            var categoryId = 999;
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(null as Category);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _categoryService.GetCategoryByIdAsync(categoryId));
            Assert.That(ex.Message, Is.EqualTo($"Category with ID {categoryId} not found."));
        }

        #endregion

        #region UpdateCategoryAsync Tests

        [Test]
        public async Task UpdateCategoryAsync_WithValidData_ReturnsUpdatedCategory()
        {
            // Arrange
            var categoryId = 1;
            var categoryToUpdate = new CategoryToUpdateDTO { Name = "Updated Fiction" };

            var validationResult = new ValidationResult();
            _mockCategoryToUpdateDTOValidator.Setup(v => v.ValidateAsync(categoryToUpdate, default))
                .ReturnsAsync(validationResult);

            var existingCategory = new Category { Id = categoryId, Name = "Fiction" };
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(existingCategory);

            _mockCategoryRepository.Setup(r => r.UpdateAsync(It.IsAny<Category>()))
                .ReturnsAsync((Category c) => c);

            // Act
            var result = await _categoryService.UpdateCategoryAsync(categoryId, categoryToUpdate);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(categoryId));
            Assert.That(result.Name, Is.EqualTo("Updated Fiction"));
            _mockCategoryToUpdateDTOValidator.Verify(v => v.ValidateAsync(categoryToUpdate, default), Times.Once);
            _mockCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
        }

        [Test]
        public void UpdateCategoryAsync_WithInvalidData_ThrowsValidationException()
        {
            // Arrange
            var categoryId = 1;
            var categoryToUpdate = new CategoryToUpdateDTO();

            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("Name", "Name is required")
            };
            var validationResult = new ValidationResult(validationErrors);

            _mockCategoryToUpdateDTOValidator.Setup(v => v.ValidateAsync(categoryToUpdate, default))
                .ReturnsAsync(validationResult);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(() => _categoryService.UpdateCategoryAsync(categoryId, categoryToUpdate));
            Assert.That(ex.Errors.Count(), Is.EqualTo(1));
            _mockCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Test]
        public void UpdateCategoryAsync_WithNonExistingCategory_ThrowsNotFoundException()
        {
            // Arrange
            var categoryId = 999;
            var categoryToUpdate = new CategoryToUpdateDTO { Name = "Updated Fiction" };

            var validationResult = new ValidationResult();
            _mockCategoryToUpdateDTOValidator.Setup(v => v.ValidateAsync(categoryToUpdate, default))
                .ReturnsAsync(validationResult);

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .ReturnsAsync(null as Category);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _categoryService.UpdateCategoryAsync(categoryId, categoryToUpdate));
            Assert.That(ex.Message, Is.EqualTo($"Category with ID {categoryId} not found."));
            _mockCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }

        #endregion
    }
}
