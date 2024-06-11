using Microsoft.AspNetCore.Mvc;
using WebProjElective.Models;
using System;

namespace WebProjElective.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductContext _productContext;

        public ProductController(ProductContext productContext)
        {
            _productContext = productContext;
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
            return View();
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

            bool insertionResult = _productContext.InsertProduct(product);

            if (insertionResult)
            {
                return RedirectToAction("ProductForm","Admin"); // Redirect to product list after successful insertion
            }
            else
            {
                // Handle insertion failure
                return View("Error");
            }
        }
    }
}
