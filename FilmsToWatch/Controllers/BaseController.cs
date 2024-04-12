using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
     
    }
}
