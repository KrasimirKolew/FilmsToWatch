using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Controllers
{
    public class ReviewController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
