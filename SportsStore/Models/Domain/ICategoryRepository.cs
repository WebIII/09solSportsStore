using System.Collections.Generic;

namespace SportsStore.Models.Domain {
    public interface ICategoryRepository {
        IEnumerable<Category> GetAll();
        Category GetById(int categoryId);
        Category GetByIdIncludingProducts(int categoryId);
    }
}