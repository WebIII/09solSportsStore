
namespace SportsStore.Models.Domain {
    public class OrderLine : CartLine {
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public decimal Price { get; private set; }

        public OrderLine(Product product, int quantity) : base(product, quantity) {
            Price = product.Price;
        }

        // Added for EF because EF cannot set navigation properties through constructor parameters
        private OrderLine() {

        }
    }
}