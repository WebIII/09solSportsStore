using Microsoft.EntityFrameworkCore;
using SportsStore.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Data.Repositories {
    public class CategoryRepository : ICategoryRepository {
        private readonly DbSet<Category> _categories;

        public CategoryRepository(ApplicationDbContext context) {
            _categories = context.Categories;
        }

        public IEnumerable<Category> GetAll() {
            return _categories.AsNoTracking().ToList();
        }

        public Category GetById(int categoryId) {
            return _categories.SingleOrDefault(c => c.CategoryId == categoryId);
        }

        public Category GetByIdIncludingProducts(int categoryId) {
            return _categories.Include(c => c.Products).SingleOrDefault(c => c.CategoryId == categoryId);
        }
    }
}