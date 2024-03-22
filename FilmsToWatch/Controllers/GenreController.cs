using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Controllers
{
    public class GenreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
