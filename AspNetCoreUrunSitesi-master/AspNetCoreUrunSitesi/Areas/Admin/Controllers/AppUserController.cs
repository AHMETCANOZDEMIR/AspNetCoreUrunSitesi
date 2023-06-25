using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BL;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize] // Bu attribute u yazmazsak aşağıdaki sayfalar 404 bulunamadı hatası verir!
    public class AppUserController : Controller
    {
        AppUserManager manager = new AppUserManager(); // Normalde kullandığımız klasik yöntem
        private readonly IRepository<AppUser> _repository; // Dependency Injection ile yeni kullanım

        public AppUserController(IRepository<AppUser> repository) // Yukarıda oluşturulan _repository nesnesini controller constructor ında  Dependency Injection yöntemiyle dolduruyoruz
        {
            _repository = repository;
        }

        // GET: AppUserController
        public ActionResult Index()
        {
            return View(manager.GetAll());
        }

        // GET: AppUserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AppUserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppUserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppUser appUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sonuc = manager.Add(appUser);
                    if (sonuc > 0) // Eğer kayıt başarılıysa
                    {
                        return RedirectToAction(nameof(Index)); // Index actionuna yani appuser liste sayfasına yönlendir
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("", "Hata Oluştu! Kayıt Başarısız!"); // Eğer hata oluşursa ekrandaki hata mesajlarına bunu da ekle
            }
            return View(appUser); // yukardaki kodları geçip buraya gelirse appuser nesnesini sayfaya geri gönder
        }

        // GET: AppUserController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var appUser = await _repository.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: AppUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, AppUser appUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   var sonuc =  _repository.Update(appUser);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(appUser);
            }
        }

        // GET: AppUserController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var appUser = await _repository.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: AppUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            try
            {
                var appUser = await _repository.FindAsync(id);
                _repository.Delete(appUser);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
