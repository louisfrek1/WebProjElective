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
        public IActionResult Insert(Product product, IFormFile ProductImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (ProductImage != null && ProductImage.Length > 0)
                    {
                        using (var ms = new System.IO.MemoryStream())
                        {
                            ProductImage.CopyTo(ms);
                            product.ProductImage = ms.ToArray(); // Convert image to byte array
                        }
                    }

                    // Insert product into database
                    bool isSuccess = _productContext.InsertProduct(product);
                    if (isSuccess)
                    {
                        TempData["SuccessMessage"] = "Product successfully created!";
                        return RedirectToAction("ProductForm", "Admin"); // Redirect to the list of products
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to create product.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
                }
            }
            // If ModelState is not valid or any error occurred, redirect to Error action
            return RedirectToAction("Error", "Product");
        }
    }
}
