using Microsoft.AspNetCore.Mvc;

namespace WebProjElective.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminForm()
        {
            return View();
        }
    }
}
