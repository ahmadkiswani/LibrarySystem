using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    public class BorrowController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
