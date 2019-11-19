using SportsStore.Models.Domain;
using System;
using Xunit;

namespace SportsStore.Tests.Models
{
    public class OrderTest
    {
        private readonly Order _order;
        private readonly Product _p1;
        private readonly Product _p3;
        private readonly DateTime _deliveryDate = DateTime.Today.AddDays(3);
        private readonly string _shippingStreet = "Voskenslaan 1";
        private readonly City _shippingCity = new City("9000", " Gent");
        private readonly Cart _cart;

        public OrderTest()
        {
            var category = new Category("Category1");
            _cart = new Cart();
            var p2 = new Product("Short", 5, category) { ProductId = 2 };
            _p1 = new Product("Football", 10, category) { ProductId = 1 };
            _p3 = new Product ("NotInOrder",  5, category) { ProductId = 3 };
            _cart.AddLine(_p1, 1);
            _cart.AddLine(p2, 10);
            _order = new Order(_cart, _deliveryDate, false, _shippingStreet, _shippingCity);
        }

        #region Constructor
        [Fact]
        public void NewOrder_ValidCart_TurnsCartIntoOrder() {
            Assert.False(_order.Giftwrapping);
            Assert.Equal(_deliveryDate, _order.DeliveryDate);
            Assert.Equal(_shippingStreet, _order.ShippingStreet);
            Assert.Equal(_shippingCity, _order.ShippingCity);
            Assert.Equal(2, _order.OrderLines.Count);
        }

        [Fact]
        public void NewOrder_EmptyCart_ThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Order(new Cart(), null, false, _shippingStreet, _shippingCity));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]

        public void NewOrder_InvalidShippingStreet_ThrowsArgumentException(string shippingStreet) {
            Assert.Throws<ArgumentException>(() => new Order(_cart, null, false, shippingStreet, _shippingCity));
        }

        [Fact]
        public void NewOrder_NoShippingCity_ThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Order(new Cart(), null, false, _shippingStreet, null));
        }

        [Fact]
        public void NewOrder_DeliveryDateTooEarly_ThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new Order(new Cart(), DateTime.Today.AddDays(1), false, _shippingStreet, _shippingCity));
        }

        #endregion

        #region Total
        [Fact]
        public void Total_ReturnsSumOfOrderlines() {
            Assert.Equal(60, _order.Total);
        }
        #endregion

        #region HasOrdered
        [Fact]
        public void HasOrdered_ProductInOrder_ReturnsTrue() {
            Assert.True(_order.HasOrdered(_p1));
        }

        [Fact]
        public void HasOrdered_ProductNotInOrder_ReturnsFalse() {
            Assert.False(_order.HasOrdered(_p3));
        }
        #endregion
    }
}
