using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Controllers
{
    public class FilmController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
