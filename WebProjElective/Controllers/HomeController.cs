using WebProjElective.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebProjElective.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserContext _userContext;

        // Inject UserContext dependency
        public HomeController(UserContext userContext)
        {
            _userContext = userContext;
        }

        // GET: Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // POST: Home/Register
        [HttpPost]
        public IActionResult Index(Users user)
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
    }
}
