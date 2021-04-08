using Microsoft.AspNetCore.Mvc;

namespace dotnetredis.Controllers
{
    public class UserController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}