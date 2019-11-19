using SportsStore.Models.Domain;
using System;
using System.Linq;
using Xunit;

namespace SportsStore.Tests.Models {
    public class CartTest {
        private readonly Cart _cart;
        private readonly Product _p1;
        private readonly Product _p2;

        public CartTest() {
            var category = new Category("New category");
            _p1 = new Product("Product1", 10, category) { ProductId = 1 };
            _p2 = new Product("Product2", 5, category) { ProductId = 2 };
            _cart = new Cart();
        }

        #region Constructor
        [Fact]
        public void NewCart_StartsEmpty() {
            Assert.Empty(_cart.CartLines);
        }

        #endregion

        #region AddLine
        [Fact]
        public void AddLine_ProductNotInCart_AddsProductToCart() {
            _cart.AddLine(_p1, 1);
            _cart.AddLine(_p2, 10);
            Assert.Equal(2, _cart.NumberOfItems);
            Assert.Equal(1, _cart.CartLines.First(l => l.Product.Equals(_p1)).Quantity);
            Assert.Equal(10, _cart.CartLines.First(l => l.Product.Equals(_p2)).Quantity);
        }

        [Fact]
        public void AddLine_ProductInCart_AdjustsQuantityOfCartLine() {
            _cart.AddLine(_p1, 1);
            _cart.AddLine(_p2, 10);
            _cart.AddLine(_p1, 3);
            Assert.Equal(2, _cart.NumberOfItems);
            Assert.Equal(4, _cart.CartLines.First(l => l.Product.Equals(_p1)).Quantity);
            Assert.Equal(10, _cart.CartLines.First(l => l.Product.Equals(_p2)).Quantity);
        }
        #endregion

        #region RemoveLine
        [Fact]
        public void RemoveLine_ProductInCart_RemovesProduct() {
            _cart.AddLine(_p1, 1);
            _cart.AddLine(_p2, 10);
            _cart.RemoveLine(_p2);
            Assert.Equal(1, _cart.NumberOfItems);
            Assert.Equal(1, _cart.CartLines.First(l => l.Product.Equals(_p1)).Quantity);
        }

        [Fact]
        public void RemoveLine_ProductNotInCart_ThrowsArgumentException() {
            _cart.AddLine(_p1, 1);
            Assert.Throws<ArgumentException>(() => _cart.RemoveLine(_p2));
        }

        #endregion

        #region Clear
        [Fact]
        public void Clear_ProductsInCart_ClearsCart() {
            _cart.AddLine(_p1, 1);
            _cart.AddLine(_p2, 10);
            _cart.AddLine(_p1, 3);
            _cart.Clear();
            Assert.Empty(_cart.CartLines);
        }

        #endregion

        #region TotalValue
        [Fact]
        public void TotalValue_CartWithCartLines_ReturnstheTotalValue() {
            _cart.AddLine(_p1, 1);
            _cart.AddLine(_p2, 10);
            _cart.AddLine(_p1, 3);
            Assert.Equal(90, _cart.TotalValue);
        }

        [Fact]
        public void TotalValue_EmptyCart_ReturnsZero() {
            Assert.Equal(0, _cart.TotalValue);
        }
        #endregion

        #region IncreaseQuantity
        [Fact]
        public void IncreaseQuantity_ExistingLine_IncreasesQuantity() {
            _cart.AddLine(_p1, 10);
            _cart.IncreaseQuantity(_p1.ProductId);
            Assert.Equal(11, _cart.CartLines.FirstOrDefault(l => l.Product.Equals(_p1))?.Quantity);
        }

        [Fact]
        public void IncreaseQuantity_NonExistingLine_ThrowsArgumentException() {
            _cart.AddLine(_p1, 10);
            Assert.Throws<ArgumentException>(() => _cart.IncreaseQuantity(2000));
        }
        #endregion
        
        #region DecreaseQuantity
        [Fact]
        public void DecreaseQuantity_ExistingLine_DecreasesQuantity() {
            _cart.AddLine(_p1, 10);
            _cart.DecreaseQuantity(_p1.ProductId);
            Assert.Equal(9, _cart.CartLines.FirstOrDefault(l => l.Product.Equals(_p1))?.Quantity);
        }

        [Fact]
        public void DecreaseQuantity_ExistingLineWithQuantity1_RemovesLine() {
            _cart.AddLine(_p1, 1);
            _cart.DecreaseQuantity(_p1.ProductId);
            Assert.Empty(_cart.CartLines);
        }

        [Fact]
        public void DecreaseQuantity_NonExistingLine_ThrowsArgumentException() {
            _cart.AddLine(_p1, 1);
            Assert.Throws<ArgumentException>(() => _cart.DecreaseQuantity(2000));
        }

        #endregion

    }
}
