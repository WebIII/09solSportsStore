using SportsStore.Models.Domain;
using System;
using Xunit;

namespace SportsStore.Tests.Models {
    public class OrderLineTest {
        private readonly Product _p1;

        public OrderLineTest() {
            _p1 = new Product("Football", 10, new Category("Soccer")) { ProductId = 1 };
        }

        #region Constructor
        [Fact]
        public void NewOrderLine_ValidData_CreatesOrderline() {
            OrderLine orderLine = new OrderLine(_p1, 5);
            Assert.Equal(5, orderLine.Quantity);
            Assert.Equal(_p1, orderLine.Product);
            Assert.Equal(10, orderLine.Price);
        }

        [Fact]
        public void NewOrderLine_NoProduct_ThrowsArgumentException() {
            Assert.Throws<ArgumentException>(() => new OrderLine(null, 10));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void NewOrderLine_InvalidQuantity_ThrowsArgumentException(int quantity) {
            Assert.Throws<ArgumentException>(() => new OrderLine(_p1, quantity));
        }

        #endregion

    }
}
