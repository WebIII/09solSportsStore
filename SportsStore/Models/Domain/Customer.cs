using System;
using System.Collections.Generic;

namespace SportsStore.Models.Domain {
    public class Customer {
        #region Fields
        private string _customerName;
        private string _name;
        private string _street;
        private string _firstName;
        private City _city;
        private readonly ICollection<Order> _orders;

        #endregion

        #region Properties

        public int CustomerId { get; private set; }

        public string CustomerName {
            get => _customerName;
            set {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 20)
                    throw new ArgumentException("CustomerName cannot be empty and should not exceed 20 characters");
                _customerName = value;
            }
        }

        public string Name {
            get => _name;
            set {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("Name cannot be empty and should not exceed 100 characters");
                _name = value;
            }
        }

        public string FirstName {
            get => _firstName;
            set {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("FirstName cannot be empty and should not exceed 100 characters");
                _firstName = value;
            }
        }

        public string Street {
            get => _street;
            set {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("Street cannot be empty and should not exceed 100 characters");
                _street = value;
            }
        }

        public City City {
            get => _city;
            set {
                _city = value ?? throw new ArgumentException("City cannot be empty");
            }
        }

        public IEnumerable<Order> Orders {
            get => _orders;
        }
        #endregion

        #region Constructors
        // Added for EF because EF cannot set navigation properties through constructor parameters
        private Customer() {
            _orders = new List<Order>();
        }

        public Customer(string customerName, string name, string firstName, string street, City city) : this() {
            CustomerName = customerName;
            Name = name;
            FirstName = firstName;
            Street = street;
            City = city;
        }
        #endregion

        #region Methods
        public void PlaceOrder(Cart cart, DateTime? deliveryDate, bool giftwrapping, string shippingStreet, City shippingCity) {
            _orders.Add(new Order(cart, deliveryDate, giftwrapping, shippingStreet, shippingCity));
        }

        #endregion
    }
}