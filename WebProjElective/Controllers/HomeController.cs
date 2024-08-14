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
        private readonly PlacedContext _placedContext;
        private readonly IConfiguration _configuration;

        // Inject UserContext dependency
        public HomeController(UserContext userContext, ProductContext productContext, IConfiguration configuration, PlacedContext placedContext)
        {
            _userContext = userContext;
            _productContext = productContext;
            _configuration = configuration;
            _placedContext = placedContext;

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

        [HttpGet]
        public IActionResult ProfileForm()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (!userId.HasValue)
            {
                TempData["ErrorMessage"] = "User is not logged in.";
                return RedirectToAction("Index");
            }

            var user = _userContext.GetUsersById(userId.Value);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User information is not available.";
                return RedirectToAction("Index");
            }

            var cartItems = _cartContext.GetCartItemsByUsername(user.UserName);

            var viewModel = new UserCart
            {
                User = user,
                Carts = cartItems
            };

            return View(viewModel);
        }



        [HttpPost]
        public IActionResult ProfileForm(int id)
        {
            if (id == 0) // Check if the id is not provided or invalid
            {
                TempData["ErrorMessage"] = "User information is not available.";
                return RedirectToAction("Index"); // Redirect to a safe page or return an error view
            }

            var user = _userContext.GetUsersById(id); // Fetch user data
            if (user == null)
            {
                TempData["ErrorMessage"] = "User information is not available.";
                return RedirectToAction("Index"); // Redirect or return an error view
            }

            var cartItems = _cartContext.GetCartItemsByUsername(user.UserName); // Fetch cart items

            var viewModel = new UserCart
            {
                User = user,
                Carts = cartItems
            };

            return View(viewModel);
        }

        public IActionResult CheckoutForm()
        {
            var username = User.Identity.Name;
            var user = _userContext.GetUserByUsername(username); // Assuming you have a method to get the user by username
            var cartItems = _cartContext.GetCartItemsByUsername(username);

            var model = new UserCart
            {
                User = user,
                Carts = cartItems
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CheckoutForm(PlacedOrder po)
        {
            if (ModelState.IsValid)
            {
                // Insert the placed order into the database
                bool isSuccess = _placedContext.InsertPlacedOrder(po);
                if (isSuccess)
                {
                    // Transfer products from cart to another table
                    var cartItems = _cartContext.GetCartItemsByUsername(po.UN);
                    _cartContext.TransferCartItemsToOrderedProducts(cartItems, po.UN);

                    // Delete the cart items for the logged-in user
                    _cartContext.DeleteCartItemsByUsername(po.UN);

                    TempData["SuccessMessage"] = "Order placed successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to place the order.";
                }
            }
            else
            {
                // Capture the validation errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["ErrorMessage"] = "Validation failed: " + string.Join(", ", errors);
            }

            return RedirectToAction("Dashboard");
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

        public IActionResult UserCart(int userId)
        {
            var user = _userContext.GetUsersById(userId); // Fetch user data by userId
            var cartItems = _cartContext.GetCartsByUserId(userId); // Fetch cart data by userId

            if (user != null && cartItems != null)
            {
                var viewModel = new UserCart
                {
                    User = user,
                    Carts = cartItems
                };
                return View(viewModel);
            }

            return View("Error"); // Handle error case
        }


    }
}