using WebProjElective.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebProjElective.Data;
using MySql.Data.MySqlClient;

namespace WebProjElective.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext _userContext;
        private readonly ProductContext _productContext;
        private readonly ProductnCart _prodandcart;
        private readonly CartContext _cartContext;
        private readonly IConfiguration _configuration;

        // Inject UserContext dependency
        public HomeController(UserContext userContext, ProductContext productContext, IConfiguration configuration)
        {
            _userContext = userContext;
            _productContext = productContext;
            _configuration = configuration;

            // Initialize CartContext with connection string
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            _cartContext = new CartContext(connectionString);
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            var products = _productContext.GetProducts().Take(10).ToList();
            return View(products);
        }

        public IActionResult ProfileForm()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var username = User.Identity.Name;
            var cartItems = _cartContext.GetCartItemsByUsername(username);
            return View(cartItems);
        }

        public IActionResult StoreProductForm(string category)
        {
            List<Product> products;

            if (!string.IsNullOrEmpty(category))
            {
                // Filter products by category
                products = _productContext.GetProductsByCategory(category);
            }
            else
            {
                // If no category is specified, get all products
                products = _productContext.GetProducts();
            }

            var cartItems = _cartContext.GetCartItemsByUsername(User.Identity.Name);

            var model = new CartProductModel
            {
                Products = products,
                CartItems = cartItems
            };

            return View(model);
        }


        // POST: Home/Register
        [HttpPost]
        public IActionResult Index(Users user)
        {
            if (ModelState.IsValid)
            {
                bool isSuccess = _userContext.InsertUsers(user);
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "User successfully registered!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to register user.";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _userContext.GetUser(email, password);
                if (user != null)
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    TempData["SuccessMessage"] = "Login successful!";

                    if (user.AcctType == "admin")
                    {
                        return RedirectToAction("AdminForm", "Admin");  // Redirect to admin form
                    }
                    else if (user.AcctType == "user")
                    {
                        return RedirectToAction("Dashboard", "Home");  // Redirect to user dashboard
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid login credentials.";
                }
            }
            return RedirectToAction("Index");
        }



        // GET: Home/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> StoreProductForm(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Home");
            }

            var product = await _productContext.GetProductByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            var cartItem = new Cart
            {
                ProdId = product.ProductId,
                ProdName = product.ProductName,
                ProdPrice = product.ProductPrice,
                ProdCategory = product.ProductCategory,
                ProdImage = product.ProductImage,
                Username = User.Identity.Name,
                ProdDateTime = DateTime.Now
            };

            if (_cartContext.InsertonCart(cartItem))
            {
                TempData["SuccessMessage"] = "Product added to cart successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "There was an error adding the product to the cart.";
            }

            return RedirectToAction("StoreProductForm", "Home");
        }


    }
}