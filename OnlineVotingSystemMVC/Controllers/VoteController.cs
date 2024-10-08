using Microsoft.AspNetCore.Mvc;

namespace OnlineVotingSystemMVC.Controllers
{
    // For vote management

    public class VoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
