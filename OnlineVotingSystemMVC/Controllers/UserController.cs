using Microsoft.AspNetCore.Mvc;

namespace OnlineVotingSystemMVC.Controllers
{
    // For user management

    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
