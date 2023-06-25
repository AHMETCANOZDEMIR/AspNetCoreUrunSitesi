using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize] // admin area içindeki controller ların çalışması için bu gerekli!!
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
