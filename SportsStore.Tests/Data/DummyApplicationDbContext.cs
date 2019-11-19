using SportsStore.Models.Domain;
using System.Collections.Generic;
using System.Linq;
namespace SportsStore.Tests.Data {
    public class DummyApplicationDbContext {
        private readonly City _gent;
        private readonly City _antwerpen;
        private readonly Category _watersports;
        private readonly Category _chess;
        private readonly IList<Product> _products;

        public IEnumerable<City> Cities => new List<City> { _gent, _antwerpen };
        public IEnumerable<Category> Categories => new List<Category> { _watersports, Soccer, _chess };
        public Category Soccer { get; }
        public IEnumerable<Product> Products => _products;
        public IEnumerable<Product> ProductsOnline => _products.Where(p => p.Availability == Availability.OnlineOnly || p.Availability == Availability.ShopAndOnline);
        public Product Football { get; }
        public Product RunningShoes { get; }
        public Customer Customer => new Customer("jan@hogent.be", "Janneman", "Jan", "Nieuwstraat 100", _gent);

        public DummyApplicationDbContext() {
            // Cities
            _gent = new City("9000", "Gent");
            _antwerpen = new City("3000", "Antwerpen");

            // Categories
            Soccer = new Category("Soccer");
            _watersports = new Category("WaterSports");
            _chess = new Category("Chess");

            // Products
            Football = new Product("Football", 25, Soccer, "WK colors") { ProductId = 1 };
            RunningShoes = new Product("Running shoes", 95, Soccer, "Protective and fashionable") { ProductId = 2 };
            _products = new List<Product>() {
               Football,
               RunningShoes,
               new Product("Stadium", 75, Soccer, "Flat-packed 35000-seat stadium miniature"),
               new Product("Corner flags", 34, Soccer, "Give your playing field that professional touch"),
               new Product("Surf board", 275, _watersports, "A boat for one person"),
               new Product("Kayak", 170, _watersports, "High quality", true, Availability.ShopOnly),
               new Product("Lifejacket", 49, _watersports, "Protective and fashionable"),
               new Product("Thinking cap", 16, _chess, "Improve your brain efficiency by 75%"),
               new Product("Unsteady chair", 30, _chess, "Secretly give your opponent a disadvantage"),
               new Product("Human chess board", 75, _chess, "A fun game for the whole extended family!"),
               new Product("Bling-bling King", 1200, _chess, "Gold plated, diamond-studded king")
            };
        }
    }
}