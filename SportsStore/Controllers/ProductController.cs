using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsStore.Models.Domain;
using SportsStore.Models.ProductViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(int categoryId = 0)
        {
            IEnumerable<Product> products;
            if (categoryId == 0)
                products = _productRepository.GetAll();
            else
                products = _categoryRepository.GetByIdIncludingProducts(categoryId).Products;
            products = products.OrderBy(b => b.Name).ToList();
            ViewData["Categories"] = GetCategoriesSelectList(categoryId);
            return View(products);
        }

        public IActionResult Edit(int id)
        {
            Product product = _productRepository.GetById(id);
            if (product == null)
                return NotFound();
            ViewData["IsEdit"] = true;
            ViewData["Categories"] = GetCategoriesSelectList();
            return View(new EditViewModel(product));
        }

        [HttpPost]
        public IActionResult Edit(int id, EditViewModel editViewModel)
        {
            Product product = _productRepository.GetById(id);
            if (product == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    product.EditProduct(editViewModel.Name, editViewModel.Description, editViewModel.Price, editViewModel.InStock, _categoryRepository.GetById(editViewModel.CategoryId), editViewModel.Availability, editViewModel.AvailableTill);
                    _productRepository.SaveChanges();
                    TempData["message"] = $"You successfully updated product {product.Name}.";
                }
                catch
                {
                    TempData["error"] = "Sorry, something went wrong, product was not updated...";
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IsEdit"] = true;
            ViewData["Categories"] = GetCategoriesSelectList();
            return View(editViewModel);

        }

        public IActionResult Create()
        {
            ViewData["IsEdit"] = false;
            ViewData["Categories"] = GetCategoriesSelectList();
            return View(nameof(Edit), new EditViewModel());
        }

        [HttpPost]
        public IActionResult Create(EditViewModel editViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = new Product(editViewModel.Name, editViewModel.Price, _categoryRepository.GetById(editViewModel.CategoryId), editViewModel.Description, editViewModel.InStock, editViewModel.Availability, editViewModel.AvailableTill);
                    _productRepository.Add(product);
                    _productRepository.SaveChanges();
                    TempData["message"] = $"You successfully added product {product.Name}.";
                }
                catch
                {
                    TempData["error"] = "Sorry, something went wrong, the product was not added...";
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IsEdit"] = false;
            ViewData["Categories"] = GetCategoriesSelectList();
            return View(nameof(Edit), editViewModel);
        }

        public IActionResult Delete(int id)
        {
            Product product = _productRepository.GetById(id);
            if (product == null)
                return NotFound();
            ViewData["ProductName"] = product.Name;
            return View();
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                Product product = _productRepository.GetById(id);
                if (product == null)
                    return NotFound();
                _productRepository.Delete(product);
                _productRepository.SaveChanges();
                TempData["message"] = $"You successfully deleted product {product.Name}.";
            }
            catch
            {
                TempData["error"] = "Sorry, something went wrong, the product was not deleted...";
            }
            return RedirectToAction(nameof(Index));
        }

        private SelectList GetCategoriesSelectList(int selected = 0)
        {
            return new SelectList(_categoryRepository.GetAll().OrderBy(g => g.Name).ToList(),
                nameof(Category.CategoryId), nameof(Category.Name), selected);
        }
    }
}
