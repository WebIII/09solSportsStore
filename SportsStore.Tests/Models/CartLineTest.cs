using SportsStore.Models.Domain;
using System;
using Xunit;

namespace SportsStore.Tests.Models {
    public class CartLineTest {
        private readonly Product _p1;

        public CartLineTest() {
            _p1 = new Product("Football", 10, new Category("Soccer")) { ProductId = 1 };
        }

        #region Constructor
        [Fact]
        public void NewCartLine_ValidData_CreatesCartline() {
            CartLine cartLine = new CartLine(_p1, 5);
            Assert.Equal(5, cartLine.Quantity);
            Assert.Equal(_p1, cartLine.Product);
          }

        [Fact]
        public void NewCartLine_NoProduct_ThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new CartLine(null, 10));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void NewCartLine_InvalidQuantity_ThrowsArgumentException(int quantity) {
            Assert.Throws<ArgumentException>(() => new CartLine(_p1, quantity));
        }

        #endregion

        #region Total
        [Fact]
        public void Total_ReturnsTotalOfCartLine() {
            CartLine cartLine = new CartLine(_p1, 5);
            Assert.Equal(50, cartLine.Total);
        }

        #endregion

    }
}
