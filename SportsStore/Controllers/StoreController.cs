using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Controllers {
    public class StoreController : Controller {
        private readonly IProductRepository _productRepository;

        public StoreController(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        public IActionResult Index() {
            return View(_productRepository.GetByAvailability(new List<Availability>() { Availability.ShopAndOnline, Availability.OnlineOnly }).ToList());
        }
    }
}