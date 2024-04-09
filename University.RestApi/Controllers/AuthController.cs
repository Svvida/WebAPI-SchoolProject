using Microsoft.AspNetCore.Mvc;

namespace University.RestApi.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
