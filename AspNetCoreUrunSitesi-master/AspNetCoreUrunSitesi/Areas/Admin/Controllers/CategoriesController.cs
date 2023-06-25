using BL;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace AspNetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class CategoriesController : Controller
    {
        private readonly IRepository<Category> _repository;

        public CategoriesController(IRepository<Category> repository)
        {
            _repository = repository;
        }

        // GET: CategoriesController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriesController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.ParentId = new SelectList(await _repository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Category category)
        {
            try
            {
                await _repository.AddAsync(category);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
        }

        // GET: CategoriesController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            ViewBag.ParentId = new SelectList(await _repository.GetAllAsync(), "Id", "Name");
            return View(data);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Category category)
        {
            try
            {
                _repository.Update(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(category);
            }
        }

        // GET: CategoriesController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            try
            {
                var data = await _repository.FindAsync(id);
                var sonuc = _repository.Delete(data);
                if (sonuc > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                ModelState.AddModelError("","Kayıt Silinemedi!");
            }
            return View();
        }
    }
}
