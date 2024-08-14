using Microsoft.AspNetCore.Mvc;
using WebProjElective.Models;

namespace WebProjElective.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductContext _productContext;
        private readonly UserContext _userContext;
        private readonly CartContext _cartContext;
        private readonly PlacedContext _placedContext;

        public AdminController(ProductContext productContext, UserContext userContext, CartContext cartContext, PlacedContext placedContext)
        {
            _productContext = productContext;
            _userContext = userContext;
            _cartContext = cartContext;
            _placedContext = placedContext;
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

        public IActionResult PlacedForm()
        {
            var carts = _placedContext.GetOrders();
            return View(carts);
        }

        public IActionResult DeliveredForm()
        {
            return View();
        }

        public IActionResult RecievedForm()
        {
            return View();
        }
        

        public IActionResult ProductOrderedForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateAccForm(int id)
        {
            var userids = _userContext.GetUsersByIds(id);

            if (userids == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("AccountsForm"); // Redirect back to the list if the user doesn't exist
            }

            return View(userids);
        }

        [HttpPost]
        public IActionResult UpdateAccForm(Users updateduser)
        {
            try
            {
                int ID = updateduser.Id;
                bool isSuccess = _userContext.UpdateUser(updateduser);

                if (isSuccess)
                {
                    TempData["SucessMessage"] = "Successfully updated..";
                }
                else
                {
                    TempData["ErrorMessage"] = ID;
                }
           
            }catch (Exception ex)
            {
                TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
            }
            return View(updateduser);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProductsForm(int id)
        {
            var product = await _productContext.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(); // Handle product not found scenario
            }

            var viewModel = new Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductPrice = product.ProductPrice,
                ProductDescription = product.ProductDescription,
                ProductImage = product.ProductImage,
                ProductCategory = product.ProductCategory,
                ProductAvailableItems = product.ProductAvailableItems,
                ProductUserName = product.ProductUserName,
                ProductDateUpload = product.ProductDateUpload
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateProductsForm(Product updateproduct)
        {
            try
            {
                int ProductId = updateproduct.ProductId;
                bool isSuccess = _productContext.UpdateProduct(updateproduct);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Successfully Updated Product";
                }
                else
                {
                    TempData["ErrorMessage"] = ProductId;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
            }

            return View(updateproduct);
        }

        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                bool isSuccess = _productContext.DeleteProduct(id);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Successfully Deleted";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to Delete";
                }
            }catch (Exception ex)
            {
                TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("ProductForm","Admin");
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                bool isSuccess = _userContext.DeleteUser(id);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Na delete na ni...";
                }
                else
                {
                    TempData["ErrorMessage"] = "Wala na delete..";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("AccountsForm", "Admin");
        }

        [HttpPost]
        public IActionResult DeleteOrder(int id)
        {
            try
            {
                bool isSuccess = _cartContext.DeleteOrder(id);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Na delete na ni...";
                }
                else
                {
                    TempData["ErrorMessage"] = "Wala na delete..";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("OrdersForm", "Admin");
        }
    }
}
