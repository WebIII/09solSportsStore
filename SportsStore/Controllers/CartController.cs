using Microsoft.AspNetCore.Mvc;
using SportsStore.Filters;
using SportsStore.Models.Domain;

namespace SportsStore.Controllers {
    [ServiceFilter(typeof(CartSessionFilter))]
    public class CartController : Controller {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        public IActionResult Index(Cart cart) {
            if (cart.IsEmpty)
                return View("EmptyCart");
            ViewData["Total"] = cart.TotalValue;
            return View(cart.CartLines);
        }


        [HttpPost]
        public IActionResult Add(int id, int quantity, Cart cart) {
            Product product = _productRepository.GetById(id);
            if (product != null)
            {
                cart.AddLine(product, quantity);
                TempData["message"] = $"Product {product.Name} has been added to the cart";
            }
            return RedirectToAction(nameof(Index), "Store");
        }

        [HttpPost]
        public IActionResult Remove(int id, Cart cart) {
            Product product = _productRepository.GetById(id);
            cart.RemoveLine(product);
            TempData["message"] = $"{product.Name} has been removed from the cart"; 
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Plus(int id, Cart cart) {
            cart.IncreaseQuantity(id);
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        public IActionResult Min(int id, Cart cart) {
            cart.DecreaseQuantity(id);
            return RedirectToAction(nameof(Index));
        }
    }
}