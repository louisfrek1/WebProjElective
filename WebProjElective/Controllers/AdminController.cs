using Microsoft.AspNetCore.Mvc;
using WebProjElective.Models;

namespace WebProjElective.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductContext _productContext;

        public AdminController(ProductContext productContext)
        {
            _productContext = productContext;
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
            return View();
        }

        public IActionResult OrdersForm()
        {
            return View();
        }


    }
}
