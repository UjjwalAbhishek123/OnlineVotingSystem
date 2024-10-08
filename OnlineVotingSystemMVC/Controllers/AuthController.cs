using Microsoft.AspNetCore.Mvc;

namespace OnlineVotingSystemMVC.Controllers
{
    // For login, logout, and registration

    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
