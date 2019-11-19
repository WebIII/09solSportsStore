using Newtonsoft.Json;
using System;

namespace SportsStore.Models.Domain {
    [JsonObject(MemberSerialization.OptIn)]
    public class CartLine {
        #region Fields
        private int _quantity;
        private Product _product;
        #endregion

        #region Properties
        [JsonProperty]
        public Product Product {
            get => _product;
            private set {
                _product = value ?? throw new ArgumentException("You must specify a product");
            }
        }

        [JsonProperty]
        public int Quantity {
            get => _quantity;
            set {
                if (value <= 0)
                    throw new ArgumentException("Quantity must be positive");
                _quantity = value;
            }
        }

        public decimal Total => Product.Price * Quantity;

        #endregion

        #region Constructors
        public CartLine(Product product, int quantity) {
            Product = product;
            Quantity = quantity;
        }

        // Added for EF because EF cannot set navigation properties through constructor parameters
        protected CartLine() {

        }
        #endregion
    }
}