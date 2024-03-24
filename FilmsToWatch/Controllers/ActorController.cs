using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Controllers
{
    public class ActorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
