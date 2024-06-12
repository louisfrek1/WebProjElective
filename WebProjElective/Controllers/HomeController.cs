using WebProjElective.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace WebProjElective.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext _userContext;
        private readonly ProductContext _productContext;

        // Inject UserContext dependency
        public HomeController(UserContext userContext,  ProductContext productContext)
        {
            _userContext = userContext;
            _productContext = productContext;
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            var products = _productContext.GetProducts().Take(10).ToList();
            return View(products);
        }

        public IActionResult Dashboard()
        {
            var products = _productContext.GetProducts().Take(10).ToList();
            return View(products);
        }


        // POST: Home/Register
        [HttpPost]
        public IActionResult Register(Users user)
        {
            if (ModelState.IsValid)
            {
                // Insert user into database
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
    }
}
