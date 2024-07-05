using Microsoft.AspNetCore.Mvc;
using WebProjElective.Models;

namespace WebProjElective.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductContext _productContext;
        private readonly UserContext _userContext;
        private readonly CartContext _cartContext;

        public AdminController(ProductContext productContext, UserContext userContext, CartContext cartContext)
        {
            _productContext = productContext;
            _userContext = userContext;
            _cartContext = cartContext;
        }
        public IActionResult AdminForm()
        {
            return View();
        }

        public IActionResult ProductForm()
        {
            var products = _productContext.GetProducts();
            return View(products);
        }

        public IActionResult ProfileForm()
        {
            return View();
        }

        public IActionResult AccountsForm()
        {
            var users = _userContext.GetUsers();
            return View(users);
        }

        public IActionResult OrdersForm()
        {
            var carts = _cartContext.GetOrders();
            return View(carts);
        }
        public IActionResult UpdateAccForm()
        {
            return View();
        }


    }
}
