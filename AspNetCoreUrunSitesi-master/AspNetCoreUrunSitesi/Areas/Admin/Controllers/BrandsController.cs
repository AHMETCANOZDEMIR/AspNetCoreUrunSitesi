using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Entities;
using BL;
using AspNetCoreUrunSitesi.Utils;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize] // Authorize attribute u bu controller a gelecek isteklerin admin girişi yapılmış olması şartını koşar, giriş yapmamış istekler engellenir.
    public class BrandsController : Controller
    {
        private readonly IRepository<Brand> _repository;

        public BrandsController(IRepository<Brand> repository)
        {
            _repository = repository;
        }

        // GET: BrandsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: BrandsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: BrandsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Brand brand, IFormFile Logo) // Resim yükleme için IFormFile Logo diyerek ön yüzden gelecek file upload inputunun adını parametrede geçmeliyiz
        {
            try
            {
                if (ModelState.IsValid)
                {
                    brand.Logo = FileHelper.FileLoader(Logo); // FileHelper sınıfındaki FileLoader metoduna logo içerisindeki resmi gönderiyoruz
                    await _repository.AddAsync(brand);
                    await _repository.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(brand);
            }
        }

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Brand brand, IFormFile Logo, bool resmiSil)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (resmiSil == true)
                    {
                        FileHelper.FileRemover(brand.Logo);
                        brand.Logo = string.Empty;
                    }

                    if (Logo != null) brand.Logo = FileHelper.FileLoader(Logo);
                    _repository.Update(brand);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(brand);
            }
        }

        // GET: BrandsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            try
            {
                var data = await _repository.FindAsync(id);
                FileHelper.FileRemover(data.Logo);
                _repository.Delete(data);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
