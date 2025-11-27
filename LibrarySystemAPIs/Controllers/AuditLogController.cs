using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    public class AuditLogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
