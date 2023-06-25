using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreUrunSitesi.Areas.ApiAdmin.Controllers
{
    [Area("ApiAdmin")] // Adres çubuğunda /ApiAdmin/Home/Index şeklinde istek yapılırsa aşağıdaki action çalışsın
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
