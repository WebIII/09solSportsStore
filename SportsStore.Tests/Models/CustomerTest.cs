using SportsStore.Models.Domain;
using System;
using System.Linq;
using Xunit;

namespace SportsStore.Tests.Models {
    public class CustomerTest {
        private const string _customerName = "SportingPiet";
        private const string _name = "Pieters";
        private const string _firstName = "Piet";
        private const string _street = "V. Vaerwyckweg 1";
        private readonly City _city = new City("9000", "Gent");

        #region Constructor
        [Fact]
        public void NewCustomer_ValidData_CreatesCustomer() {
            var customer = new Customer(_customerName, _name, _firstName, _street, _city);
            Assert.Empty(customer.Orders);
            Assert.Equal(_customerName, customer.CustomerName);
            Assert.Equal(_name, customer.Name);
            Assert.Equal(_firstName, customer.FirstName);
            Assert.Equal(_street, customer.Street);
            Assert.Equal(_city, customer.City);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("1234567890123456789012")]
        public void NewCustomer_CustomerNameNotValid_ThrowsArgumentException(string customerName) {
            Assert.Throws<ArgumentException>(() => new Customer(customerName, _name, _firstName, _street, _city));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("1234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890111")]
        public void NewCustomer_InValidNameOrFirstNameOrStreet_ThrowsArgumentException(string invalidString) {
            Assert.Throws<ArgumentException>(() => new Customer(_customerName, invalidString, _firstName, _street, _city));
            Assert.Throws<ArgumentException>(() => new Customer(_customerName, _name, invalidString, _street, _city));
            Assert.Throws<ArgumentException>(() => new Customer(_customerName, _name, _firstName, invalidString, _city));
        }

        #endregion

        #region PlaceOrder
        [Fact]
        public void PlaceOrder_ValidOrder_AddsOrder() {
            var customer = new Customer(_customerName, _name, _firstName, _street, _city);
            var deliveryDate = DateTime.Today.AddDays(5);
            var shippingStreet = "Voskenslaan 1";
            var product = new Product("Football", 10, new Category("Soccer"));
            var cart = new Cart();
            cart.AddLine(product, 15);
            customer.PlaceOrder(cart, deliveryDate, true, shippingStreet, _city);
            Assert.Single(customer.Orders);
            //Assert.Single(customer.Orders, o => o.Giftwrapping);
            //Assert.Single(customer.Orders, o => o.DeliveryDate == deliveryDate);
            //Assert.Single(customer.Orders, o => o.ShippingStreet == shippingStreet);
            //Assert.Single(customer.Orders, o => o.ShippingCity == _city);
            //Assert.Single(customer.Orders.First().OrderLines, ol => ol.Product == product);
            //Assert.Single(customer.Orders.First().OrderLines, ol => ol.Price == 10);
            //Assert.Single(customer.Orders.First().OrderLines, ol => ol.Quantity == 15);
        }

        #endregion
    }
}
