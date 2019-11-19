using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models.Domain {
    public class Category {
        #region Fields
        private string _name;
        private readonly ICollection<Product> _products;
        #endregion

        #region Properties
        public int CategoryId { get; private set; }

        public string Name {
            get => _name;
            set {
                if (string.IsNullOrWhiteSpace(value) || value.Length > 100)
                    throw new ArgumentException("Category must have a name, and the name should not exceed 100 characters");
                _name = value;
            }
        }

        public IEnumerable<Product> Products => _products;

        #endregion

        #region Constructor
        public Category(string name) {
            _products = new List<Product>();
            Name = name;
        }
        #endregion

        #region Methods

        public void AddProduct(Product product) {
            if (Products.Any(p => p.Name == product?.Name))
                throw new ArgumentException("Product with this name already exists in this category");
            if (product?.Category != this)
                throw new ArgumentException("Product belongs to another category");
            _products.Add(product);
        }

        public void RemoveProduct(Product product) {
            if (!_products.Remove(product))
                throw new ArgumentException("Product was not found in this category");
        }

        public Product FindProduct(string name) {
            return Products.FirstOrDefault(p => p.Name == name);
        }
        #endregion
    }
}
