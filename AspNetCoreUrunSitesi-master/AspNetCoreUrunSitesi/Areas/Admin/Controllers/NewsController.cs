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
    public class NewsController : Controller
    {
        private readonly IRepository<News> _repository;

        public NewsController(IRepository<News> repository)
        {
            _repository = repository;
        }

        // GET: NewsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: NewsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: NewsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(News news, IFormFile Image)
        {
            try
            {
                news.Image = FileHelper.FileLoader(Image);
                await _repository.AddAsync(news);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(news); // hataya düşerse patlamaması için news i sayfaya geri gönder
            }
        }

        // GET: NewsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: NewsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, News news, IFormFile Image, bool resmiSil)
        {
            try
            {
                if (Image != null)
                {
                    news.Image = FileHelper.FileLoader(Image);
                }
                if (resmiSil == true)
                {
                    FileHelper.FileRemover(news.Image);
                    news.Image = string.Empty;
                }
                _repository.Update(news);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(news);
            }
        }

        // GET: NewsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: NewsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, News news)
        {
            try
            {
                FileHelper.FileRemover(news.Image);
                _repository.Delete(news);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(news);
            }
        }
    }
}
