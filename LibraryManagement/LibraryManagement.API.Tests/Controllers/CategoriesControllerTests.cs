using LibraryManagement.API.Controllers;
using LibraryManagement.Application.Common;
using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace LibraryManagement.API.Tests.Controllers
{
    [TestFixture]
    internal class CategoriesControllerTests
    {
        private Mock<ICategoryService> _mockCategoryService;
        private CategoriesController _categoriesController;

        [SetUp]
        public void Setup()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _categoriesController = new CategoriesController(_mockCategoryService.Object);

            // Setup authorized user context
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "SuperUser")
            }, "TestAuthentication"));

            _categoriesController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        #region CreateCategory Tests

        [Test]
        public async Task CreateCategory_WithValidInput_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var categoryToAdd = new CategoryToAddDTO { Name = "New Category" };
            var expectedCategory = new CategoryToReturnDTO { Id = 1, Name = "New Category" };

            _mockCategoryService.Setup(x => x.AddCategoryAsync(categoryToAdd))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _categoriesController.CreateCategory(categoryToAdd);

            // Assert            
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
                var createdAtResult = result.Result as CreatedAtActionResult;
                Assert.That(createdAtResult?.ActionName, Is.EqualTo(nameof(_categoriesController.GetCategoryById)));
                Assert.That((createdAtResult?.RouteValues?["id"]), Is.EqualTo(expectedCategory.Id));
                Assert.That(createdAtResult?.Value, Is.EqualTo(expectedCategory));
            });
        }

        [Test]
        public void CreateCategory_WhenServiceThrowsException_ReturnsProblem()
        {
            // Arrange
            var categoryToAdd = new CategoryToAddDTO { Name = "New Category" };
            _mockCategoryService.Setup(x => x.AddCategoryAsync(categoryToAdd))
                .ThrowsAsync(new Exception("Test exception"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<Exception>(() => _categoriesController.CreateCategory(categoryToAdd));
            Assert.That(ex.Message, Is.EqualTo("Test exception"));
        }

        #endregion

        #region GetAllCategories Tests

        [Test]
        public async Task GetAllCategories_ReturnsListOfCategories()
        {
            // Arrange
            var expectedCategories = new List<CategoryToReturnDTO>
            {
                new CategoryToReturnDTO { Id = 1, Name = "Category 1" },
                new CategoryToReturnDTO { Id = 2, Name = "Category 2" }
            };

            _mockCategoryService.Setup(x => x.GetAllCategoriesAsync())
                .ReturnsAsync(expectedCategories);

            // Act
            var result = await _categoriesController.GetAllCategories();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedCategories));
            });
        }

        #endregion

        #region GetCategoriesPaginated Tests

        [Test]
        public async Task GetCategories_WithValidPagination_ReturnsPagedResponse()
        {
            // Arrange
            var pageIndex = 1;
            var pageSize = 10;
            var expectedResponse = new PagedResponse<CategoryToReturnDTO>
            {
                Items = new List<CategoryToReturnDTO>
                {
                    new CategoryToReturnDTO { Id = 1, Name = "Category 1" }
                },
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            _mockCategoryService.Setup(x => x.GetCategoriesPaginatedAsync(pageIndex, pageSize))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _categoriesController.GetCatgories(pageIndex, pageSize);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedResponse));
            });
        }

        #endregion

        #region GetCategoryById Tests

        [Test]
        public async Task GetCategoryById_WithExistingId_ReturnsCategory()
        {
            // Arrange
            var categoryId = 1;
            var expectedCategory = new CategoryToReturnDTO { Id = categoryId, Name = "Existing Category" };

            _mockCategoryService.Setup(x => x.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _categoriesController.GetCategoryById(categoryId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedCategory));
            });
        }

        [Test]
        public void GetCategoryById_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var categoryId = 999;
            _mockCategoryService.Setup(x => x.GetCategoryByIdAsync(categoryId))
                .ThrowsAsync(new NotFoundException("Category not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _categoriesController.GetCategoryById(categoryId));
            Assert.That(ex.Message, Is.EqualTo("Category not found"));
        }

        #endregion

        #region UpdateCategory Tests

        [Test]
        public async Task UpdateCategory_WithValidInput_ReturnsUpdatedCategory()
        {
            // Arrange
            var categoryId = 1;
            var categoryToUpdate = new CategoryToUpdateDTO { Name = "Updated Name" };
            var expectedCategory = new CategoryToReturnDTO { Id = categoryId, Name = "Updated Name" };

            _mockCategoryService.Setup(x => x.UpdateCategoryAsync(categoryId, categoryToUpdate))
                .ReturnsAsync(expectedCategory);

            // Act
            var result = await _categoriesController.UpdateCategory(categoryId, categoryToUpdate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
                var okResult = result.Result as OkObjectResult;
                Assert.That(okResult?.Value, Is.EqualTo(expectedCategory));
            });
        }

        [Test]
        public void UpdateCategory_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var categoryId = 999;
            var categoryToUpdate = new CategoryToUpdateDTO { Name = "Updated Name" };
            _mockCategoryService.Setup(x => x.UpdateCategoryAsync(categoryId, categoryToUpdate))
                .ThrowsAsync(new NotFoundException("Category not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _categoriesController.UpdateCategory(categoryId, categoryToUpdate));
            Assert.That(ex.Message, Is.EqualTo("Category not found"));
        }

        #endregion

        #region DeleteCategory Tests

        [Test]
        public async Task DeleteCategory_WithExistingId_ReturnsNoContent()
        {
            // Arrange
            var categoryId = 1;

            // Act
            var result = await _categoriesController.DeleteCategory(categoryId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mockCategoryService.Verify(x => x.DeleteCategoryAsync(categoryId), Times.Once);
        }

        [Test]
        public void DeleteCategory_WithNonExistingId_ReturnsNotFound()
        {
            // Arrange
            var categoryId = 999;
            _mockCategoryService.Setup(x => x.DeleteCategoryAsync(categoryId))
                .ThrowsAsync(new NotFoundException("Category not found"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<NotFoundException>(() => _categoriesController.DeleteCategory(categoryId));
            Assert.That(ex.Message, Is.EqualTo("Category not found"));
        }

        [Test]
        public void DeleteCategory_WhenCategoryInUse_ReturnsBadRequest()
        {
            // Arrange
            var categoryId = 1;
            _mockCategoryService.Setup(x => x.DeleteCategoryAsync(categoryId))
                .ThrowsAsync(new BadRequestException("Category cannot be deleted"));

            // Act & Assert
            var ex = Assert.ThrowsAsync<BadRequestException>(() => _categoriesController.DeleteCategory(categoryId));
            Assert.That(ex.Message, Is.EqualTo("Category cannot be deleted"));
        }

        #endregion

        #region Authorization Tests

        [Test]
        public void AllEndpoints_WithoutSuperUserRole_ReturnUnauthorized()
        {
            // Arrange
            var categoryToAdd = new CategoryToAddDTO { Name = "New Category" };
            _mockCategoryService.Setup(x => x.AddCategoryAsync(categoryToAdd))
            .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            _mockCategoryService.Setup(x => x.GetAllCategoriesAsync())
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            _mockCategoryService.Setup(x => x.GetCategoriesPaginatedAsync(1, 10))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            _mockCategoryService.Setup(x => x.GetCategoryByIdAsync(1))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            var categoryToUpdate = new CategoryToUpdateDTO { Name = "Updated Category" };
            _mockCategoryService.Setup(x => x.UpdateCategoryAsync(1, categoryToUpdate))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            _mockCategoryService.Setup(x => x.DeleteCategoryAsync(1))
                .ThrowsAsync(new UnauthorizedException("Unauthorized request"));

            // Act & Assert for each endpoint
            Assert.Multiple(() =>
            {               
                var exCreate = Assert.ThrowsAsync<UnauthorizedException>(() => _categoriesController.CreateCategory(categoryToAdd));
                Assert.That(exCreate?.Message, Is.EqualTo("Unauthorized request"));                
                
                var exGetAll = Assert.ThrowsAsync<UnauthorizedException>(() => _categoriesController.GetAllCategories());
                Assert.That(exGetAll?.Message, Is.EqualTo("Unauthorized request"));
                
                var exGetPaged = Assert.ThrowsAsync<UnauthorizedException>(() => _categoriesController.GetCatgories(1, 10));
                Assert.That(exGetPaged?.Message, Is.EqualTo("Unauthorized request"));
                
                var exGetById = Assert.ThrowsAsync<UnauthorizedException>(() => _categoriesController.GetCategoryById(1));
                Assert.That(exGetById?.Message, Is.EqualTo("Unauthorized request"));
                
                var exUpdate = Assert.ThrowsAsync<UnauthorizedException>(() => _categoriesController.UpdateCategory(1, categoryToUpdate));
                Assert.That(exUpdate?.Message, Is.EqualTo("Unauthorized request"));
                
                var exDelete = Assert.ThrowsAsync<UnauthorizedException>(() => _categoriesController.DeleteCategory(1));
                Assert.That(exDelete?.Message, Is.EqualTo("Unauthorized request"));
            });
        }

        #endregion
    }
}
