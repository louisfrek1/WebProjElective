using Microsoft.AspNetCore.Mvc;
using WebProjElective.Models;
using System;
using System.IO;

namespace WebProjElective.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _productContext;
        private readonly UserContext _userContext;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ProductContext productContext, UserContext userContext)
        {
            _productContext = productContext;
            _userContext = userContext;
        }

        // GET: Products
        public IActionResult Index()
        {
            var products = _productContext.GetProducts();
            return View(products);
        }

        // GET: Products/Create
        public IActionResult CreateForm()
        {
            var users = _userContext.GetUsersni();
            return View(users);
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateForm(Product product, IFormFile ProductImage)
        {
            if (ProductImage != null && ProductImage.Length > 0)
            {
                // Convert IFormFile to byte array
                using (var memoryStream = new MemoryStream())
                {
                    ProductImage.CopyTo(memoryStream);
                    product.ProductImage = memoryStream.ToArray();
                }
            }

            // Set the UserName
            product.ProductUserName = User.Identity.Name;

            bool insertionResult = _productContext.InsertProduct(product);

            if (insertionResult)
            {
                return RedirectToAction("ProductForm", "Admin"); // Redirect to product list after successful insertion
            }
            else
            {
                // Handle insertion failure
                return View("Error");
            }
        }
    }
}
