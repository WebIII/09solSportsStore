using SportsStore.Models.Domain;
using System;
using System.Collections.Generic;

namespace SportsStore.Data {
    public class SportsStoreDataInitializer {
        private readonly ApplicationDbContext _dbContext;

        public SportsStoreDataInitializer(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void InitializeData() {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated()) {
                Category watersports = new Category("WaterSports");
                Category soccer = new Category("Soccer");
                Category chess = new Category("Chess");
                var categories = new List<Category> { watersports, soccer, chess };
                _dbContext.Categories.AddRange(categories);

                var products = new List<Product> {
                    new Product("Football", 25, soccer, "WK colors"),
                    new Product("Corner flags", 34, soccer, "Give your playing field that professional touch"),
                    new Product("Stadium", 800, soccer, "Flat-packed 35000-seat stadium"),
                    new Product("Running shoes", 95, soccer, "Protective and fashionable"),
                    new Product("Surf board", 275, watersports, "A boat for one person"),
                    new Product("Kayak", 170, watersports, "High quality", true, Availability.OnlineOnly, DateTime.Today.AddDays(14)),
                    new Product("Lifejacket", 49, watersports, "Protective and fashionable", true, Availability.ShopOnly),
                    new Product("Thinking cap", 16, chess, "Improve your brain efficiency by 75%"),
                    new Product("Unsteady chair", 30, chess, "Secretly give your opponent a disadvantage"),
                    new Product("Human chess board", 75, chess, "A fun game for the whole extended family!"),
                    new Product("Bling-bling King", 1200, chess, "Gold plated, diamond-studded king", false)
                };
                _dbContext.Products.AddRange(products);

                City gent = new City("9000", "Gent");
                City antwerpen = new City("3000", "Antwerpen");
                City[] cities = new City[] { gent, antwerpen };
                _dbContext.Cities.AddRange(cities);

                Random r = new Random();
                for (int i = 1; i < 10; i++) {
                    Customer klant = new Customer("student" + i, "Student" + i, "Jan", "Nieuwstraat 10", cities[r.Next(2)]);

                    if (i <= 5) {
                        Cart cart = new Cart();
                        cart.AddLine(soccer.FindProduct("Football"), 1);
                        cart.AddLine(soccer.FindProduct("Corner flags"), 2);
                        klant.PlaceOrder(cart, DateTime.Today.AddDays(10), false, klant.Street, klant.City);
                    }
                    _dbContext.Customers.Add(klant);
                }
                _dbContext.SaveChanges();
            }
        }
    }
}
