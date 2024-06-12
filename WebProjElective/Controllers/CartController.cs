using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult CreateForm(Cart cart, IFormFile ProductImage)
        {
            if (ProductImage != null && ProductImage.Length > 0)
            {
                // Convert IFormFile to byte array
                using (var memoryStream = new MemoryStream())
                {
                    ProductImage.CopyTo(memoryStream);
                    cart.ProdImage = memoryStream.ToArray();
                }
            }

            // Set the UserName
            cart.Username = User.Identity.Name;

            bool insertionResult = _cartContext.InsertonCart(cart);

            if (insertionResult)
            {
                return RedirectToAction("CartForm"); // Redirect to product list after successful insertion
            }
            else
            {
                // Handle insertion failure
                return View("Error");
            }
        }
    }
}
