using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
