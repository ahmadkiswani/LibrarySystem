using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
