using SportsStore.Models.Domain;
using System;
using Xunit;

namespace SportsStore.Tests.Models {
    public class CategoryTest {
        private readonly Category _soccer;
        private readonly Product _football;
        private readonly Product _cornerFlag;

        public CategoryTest() {
            _soccer = new Category("Soccer");
            _football = new Product("Football", 10, _soccer) { ProductId = 1 };
            _cornerFlag = new Product("Corner Flag", 10, _soccer) { ProductId = 2 };
        }

        #region Constructor
        [Fact]
        public void NewCategory_ValidData_CreatesCategory() {
            var category = new Category("Soccer");
            Assert.Equal("Soccer", category.Name);
            Assert.Empty(category.Products);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
        public void NewCategory_InvalidName_ThrowsArgumentException(string name) {
            Assert.Throws<ArgumentException>(() => new Category(name));
        }
        #endregion

        #region AddProduct
        [Fact]
        public void AddProduct_ValidProduct_AddsProduct() {
            _soccer.RemoveProduct(_football);
            _soccer.AddProduct(_football);
            Assert.Contains(_football, _soccer.Products);
        }

        [Fact]
        public void AddProduct_ProductFromAnotherCategory_ThrowsArgumentException() {
            var product = new Product("Kayak", 10, new Category("Watersports"));
            Assert.Throws<ArgumentException>(() => _soccer.AddProduct(product));
        }

        [Fact]
        public void AddProduct_ProductWithANameThatAlreadyExistsInCategory_ThrowsArgumentException() {
            var anotherFootball = new Product("Football", 10, new Category("abc"));
            Assert.Throws<ArgumentException>(() => _soccer.AddProduct(anotherFootball));
        }
        #endregion

        #region RemoveProduct
        [Fact]
        public void RemoveProduct_ProductInCategory_RemovesProductBasedOnItsName() {
            _soccer.RemoveProduct(_football);
            Assert.Single(_soccer.Products, _cornerFlag);
        }

        [Fact]
        public void RemoveProduct_LastProductInCategory_RemovesProduct() {
            _soccer.RemoveProduct(_football);
            _soccer.RemoveProduct(_cornerFlag);
            Assert.Empty(_soccer.Products);
        }

        [Fact]
        public void RemoveProduct_ProductNotInCategory_ThrowsArgumentException() {
            var product = new Product("NewProduct", 10, new Category("NewCategory"));
            Assert.Throws<ArgumentException>(() => _soccer.RemoveProduct(product));
        }

        [Fact]
        public void RemoveProduct_EmptyCategory_ThrowsArgumentException() {
            Category emptyCategory = new Category("Empty");
            Assert.Throws<ArgumentException>(() => emptyCategory.RemoveProduct(_football));
        }

        #endregion

        #region FindProduct
        [Fact]
        public void FindProduct_ProductInCategory_ReturnsProductWithGivenName() {
            Assert.NotNull(_soccer.FindProduct("Football"));
        }

        [Fact]
        public void FindProduct_ProductNotInCategory_ReturnsNull() {
            Assert.Null(_soccer.FindProduct("No product with this name"));
        }

        [Fact]
        public void FindProduct_EmptyCategory_ReturnsNull() {
            Category emptyCategory = new Category("Empty");
            Assert.Null(emptyCategory.FindProduct("Football"));
        }
        #endregion
    }
}
