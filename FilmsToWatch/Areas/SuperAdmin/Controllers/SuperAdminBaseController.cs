using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmsToWatch.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles ="SuperAdmin")]
    public class SuperAdminBaseController : Controller
    {
    }
}
