using Microsoft.AspNetCore.Mvc;
using WebProjElective.Models;

namespace WebProjElective.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductContext _productContext;
        private readonly UserContext _userContext;

        public AdminController(ProductContext productContext, UserContext userContext)
        {
            _productContext = productContext;
            _userContext = userContext;
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
            return View();
        }
        public IActionResult UpdateAccForm()
        {
            return View();
        }


    }
}
