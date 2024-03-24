using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Controllers
{
    public class ReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
