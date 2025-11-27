using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
