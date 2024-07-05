using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProjElective.Models;

namespace WebProjElective.Controllers
{
    public class CartController : Controller
    {
        private readonly CartContext _cartContext;
        public IActionResult CartForm()
        {
            return View();
        }

        public CartController(CartContext cartContext)
        {
            _cartContext = cartContext;
        }

        

    }
}
