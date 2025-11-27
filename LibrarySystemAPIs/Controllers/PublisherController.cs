    using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    public class PublisherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
