using Microsoft.AspNetCore.Mvc;

namespace OnlineVotingSystemMVC.Controllers
{
    // For admin-related operations

    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
