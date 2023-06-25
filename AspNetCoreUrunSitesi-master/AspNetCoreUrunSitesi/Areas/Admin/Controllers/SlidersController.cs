using AspNetCoreUrunSitesi.Utils;
using BL;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AspNetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class SlidersController : Controller
    {
        private readonly IRepository<Slider> _repository;

        public SlidersController(IRepository<Slider> repository)
        {
            _repository = repository;
        }

        // GET: SlidersController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: SlidersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SlidersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SlidersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Slider slider, IFormFile Image)
        {
            try
            {
                slider.Image = FileHelper.FileLoader(Image);
                await _repository.AddAsync(slider);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(slider);
            }
        }

        // GET: SlidersController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: SlidersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Slider slider, IFormFile Image, bool resmiSil)
        {
            try
            {
                if (Image != null) slider.Image = FileHelper.FileLoader(Image);
                if (resmiSil == true)
                {
                    FileHelper.FileRemover(fileName: slider.Image);
                    slider.Image = string.Empty;
                }
                _repository.Update(slider);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(slider);
            }
        }

        // GET: SlidersController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: SlidersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Slider slider)
        {
            try
            {
                FileHelper.FileRemover(fileName: slider.Image);
                _repository.Delete(slider);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(slider);
            }
        }
    }
}
