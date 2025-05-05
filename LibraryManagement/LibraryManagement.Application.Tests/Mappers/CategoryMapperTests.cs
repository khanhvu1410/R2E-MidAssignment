using LibraryManagement.Application.DTOs.Category;
using LibraryManagement.Application.Mappers;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Tests.Mappers
{
    [TestFixture]
    internal class CategoryMapperTests
    {
        #region ToCategoryToReturnDTO Tests

        [Test]
        public void ToCategoryToReturnDTO_WithCompleteCategory_ReturnsCorrectDTO()
        {
            // Arrange
            var category = new Category
            {
                Id = 1,
                Name = "Fiction"
            };

            // Act
            var result = category.ToCategoryToReturnDTO();

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(category.Id));
                Assert.That(result.Name, Is.EqualTo(category.Name));
            });
        }

        [Test]
        public void ToCategoryToReturnDTO_WithMinimumData_ReturnsCorrectDTO()
        {
            // Arrange
            var category = new Category
            {
                Id = 0,
            };

            // Act
            var result = category.ToCategoryToReturnDTO();

            // Assert          
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Id, Is.EqualTo(0));
                Assert.That(result.Name, Is.Null);
            });
        }

        #endregion

        #region ToCategory Tests

        [Test]
        public void ToCategory_WithCompleteCategoryToAddDTO_ReturnsCorrectCategory()
        {
            // Arrange
            var categoryToAddDTO = new CategoryToAddDTO
            {
                Name = "Science Fiction"
            };

            // Act
            var result = categoryToAddDTO.ToCategory();

            // Assert           
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo(categoryToAddDTO.Name));
                Assert.That(result.Id, Is.EqualTo(0)); // Default value for Id
            });
        }

        [Test]
        public void ToCategory_WithEmptyName_ReturnsCategoryWithNullName()
        {
            // Arrange
            var categoryToAddDTO = new CategoryToAddDTO
            {
                Name = ""
            };

            // Act
            var result = categoryToAddDTO.ToCategory();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.EqualTo(""));
            });
            
        }

        [Test]
        public void ToCategory_WithNullCategoryToAddDTO_ReturnsNullCategoryOutput()
        {
            // Arrange
            CategoryToAddDTO? categoryToAddDTO = null;

            // Act & Assert
            Assert.That(categoryToAddDTO?.ToCategory(), Is.Null);
        }

        #endregion

        #region Edge Case Tests

        [Test]
        public void ToCategoryToReturnDTO_WithNullInput_ReturnsNullOutput()
        {
            // Arrange
            Category? category = null;

            // Act & Assert
            Assert.That(category?.ToCategoryToReturnDTO(), Is.Null);
        }

        [Test]
        public void ToCategory_WithNullName_ReturnsCategoryWithNullName()
        {
            // Arrange
            var categoryToAddDTO = new CategoryToAddDTO();

            // Act
            var result = categoryToAddDTO.ToCategory();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result.Name, Is.Null);
            });
        }

        #endregion
    }
}
