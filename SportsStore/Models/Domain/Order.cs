using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models.Domain {
    public class Order {
        #region Fields
        private DateTime? _deliveryDate;
        private string _shippingStreet;
        private City _shippingCity;
        #endregion

        #region Properties
        public int OrderId { get; private set; }
        public DateTime OrderDate { get; private set; }

        public DateTime? DeliveryDate {
            get => _deliveryDate;
            private set {
                if (value.HasValue && value.Value < DateTime.Today.AddDays(3))
                    throw new ArgumentException("Delivery date should be at least three days from today...");
                _deliveryDate = value;
            }
        }

        public bool Giftwrapping { get; private set; }

        public string ShippingStreet {
            get => _shippingStreet;
            private set {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("Category must have a name, and the name should not exceed 100 characters");
                _shippingStreet = value;
            }
        }

        public City ShippingCity {
            get => _shippingCity;
            private set {
                _shippingCity = value ?? throw new ArgumentException("You must provide a city");
            }
        }

        public ICollection<OrderLine> OrderLines { get; }
        public decimal Total => OrderLines.Sum(o => o.Total);
        #endregion

        #region Constructors
        private Order() {
            OrderLines = new HashSet<OrderLine>();
            OrderDate = DateTime.Today;
        }

        public Order(Cart cart, DateTime? deliveryDate, bool giftwrapping, string shippingStreet, City shippingCity)
            : this() {
            if (!cart.CartLines.Any())
                throw new ArgumentException("Cannot place order for an empty cart");

            foreach (CartLine line in cart.CartLines) {
                OrderLines.Add(new OrderLine(line.Product, line.Quantity));
            }

            OrderDate = DateTime.Today;
            DeliveryDate = deliveryDate;
            Giftwrapping = giftwrapping;
            ShippingStreet = shippingStreet;
            ShippingCity = shippingCity;
        }
        #endregion

        #region Methods
        public bool HasOrdered(Product p) => OrderLines.Any(l => l.Product.Equals(p));
        #endregion

    }
}