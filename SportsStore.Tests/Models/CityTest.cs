using SportsStore.Models.Domain;
using System;
using Xunit;

namespace SportsStore.Tests.Models {
    public class CityTest {
        #region Constructor
        [Fact]
        public void NewCity_ValidData_CreatesCity() {
            City city = new City("9000", "Gent");
            Assert.Equal("9000", city.Postalcode);
            Assert.Equal("Gent", city.Name);
          }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("abcd")]
        [InlineData("a123")]
        [InlineData("123a")]
        [InlineData("12345")]
        [InlineData("123")]
        [InlineData("a1234")]
        public void NewCity_InvalidPostalCode_ThrowsArgumentException(string postalCode) {
            Assert.Throws<ArgumentException>(() => new City(postalCode, "Gent"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("12345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
        public void NewCity_InvalidName_ThrowsArgumentException(string name) {
            Assert.Throws<ArgumentException>(() => new City("9000", name));
        }

        #endregion

    }
}
