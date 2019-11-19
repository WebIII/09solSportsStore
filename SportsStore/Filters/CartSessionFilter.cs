using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SportsStore.Models.Domain;

namespace SportsStore.Filters {
    public class CartSessionFilter : ActionFilterAttribute {
        private readonly IProductRepository _productRepository;
        private Cart _cart;

        public CartSessionFilter(IProductRepository productRepository) {
            _productRepository = productRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            _cart = ReadCartFromSession(context.HttpContext);
            context.ActionArguments["cart"] = _cart;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context) {
            WriteCartToSession(_cart, context.HttpContext);
            base.OnActionExecuted(context);
        }

        private Cart ReadCartFromSession(HttpContext context) {
            Cart cartWithProductIdsOnly = context.Session.GetString("cart") == null ?
                new Cart() : JsonConvert.DeserializeObject<Cart>(context.Session.GetString("cart"));
            Cart cart = new Cart();
            foreach (var l in cartWithProductIdsOnly.CartLines)
            {
                cart.AddLine(_productRepository.GetById(l.Product.ProductId), l.Quantity);
            }
            return cart;
        }

        private void WriteCartToSession(Cart cart, HttpContext context) {
            context.Session.SetString("cart", JsonConvert.SerializeObject(cart));
        }
    }
}