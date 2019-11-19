using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models.Domain {
    [JsonObject(MemberSerialization.OptIn)]
    public class Cart {
        #region Fields
        [JsonProperty]
        private readonly IList<CartLine> _lines = new List<CartLine>();
        #endregion

        #region Properties
        public IEnumerable<CartLine> CartLines => _lines.AsEnumerable();
        public int NumberOfItems => _lines.Count;
        public bool IsEmpty => NumberOfItems == 0;
        public decimal TotalValue => _lines.Sum(l => l.Product.Price * l.Quantity);
        #endregion

        #region Methods
        public void AddLine(Product product, int quantity) {
            CartLine line = GetCartLine(product.ProductId);
            if (line == null)
                _lines.Add(new CartLine(product, quantity));
            else
                line.Quantity += quantity;
        }

        public void RemoveLine(Product product) {
            CartLine line = GetCartLine(product.ProductId);
            if (line == null)
                throw new ArgumentException("Product not in cart");
            _lines.Remove(line);
        }

        public void Clear() {
            _lines.Clear();
        }

        public void IncreaseQuantity(int productId) {
            CartLine line = GetCartLine(productId);
            if (line == null)
                throw new ArgumentException("Product not in cart");
            line.Quantity++;
        }

        public void DecreaseQuantity(int productId) {
            CartLine line = GetCartLine(productId);
            if (line == null)
                throw new ArgumentException("Product not in cart");
            if (line.Quantity == 1)
                _lines.Remove(line);
            else
                line.Quantity--;
        }

        private CartLine GetCartLine(int productId) {
            return _lines.SingleOrDefault(l => l.Product.ProductId == productId);
        }

        #endregion
    }
}