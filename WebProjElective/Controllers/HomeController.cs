using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebProjElective.Models;

namespace WebProjElective.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
