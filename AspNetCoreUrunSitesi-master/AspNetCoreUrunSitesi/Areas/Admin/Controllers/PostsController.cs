using AspNetCoreUrunSitesi.Utils;
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
    public class PostsController : Controller
    {
        private readonly IRepository<Post> _repository;
        private readonly IRepository<Category> _categoryRepository; // kategorileri çekmek için
        // sonradan 2. repository i constructor a eklemek için yukardaki _categoryRepository e sağ tıklayıp açılan menüden qucik actions.. ampulüne tıklayıp açılan menüden add parameters to.. ile başlayan menüye tıkladığımızda visual studio bizim için aşağıdaki constructor a dependency injection işlemini gerçekleştiriyor.
        public PostsController(IRepository<Post> repository, IRepository<Category> categoryRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
        }

        // GET: PostsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync());
        }

        // GET: PostsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PostsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name"); // Ön yüzde kategori seçmek için gerekli select elemanına kategori listesini _categoryRepository ile çekip ViewBag üzerinden gönderdik
            return View();
        }

        // POST: PostsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Post post, IFormFile Image)
        {
            try
            {
                post.Image = FileHelper.FileLoader(Image);
                await _repository.AddAsync(post);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name"); // İçerik eklerken eğer hata oluşursa return view dan önce kategroileri selectlist olarak yeniden yollamamız gerekir aksi taktirde post işlemi sonrası ön yüzdeki kategori select list i boş gelir!
                return View(post);
            }
        }

        // GET: PostsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: PostsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Post post, IFormFile Image, bool resmiSil)
        {
            try
            {
                if (Image != null) post.Image = FileHelper.FileLoader(Image);
                if (resmiSil == true)
                {
                    FileHelper.FileRemover(fileName: post.Image);
                    post.Image = string.Empty;
                }
                _repository.Update(post);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
                return View(post);
            }
        }

        // GET: PostsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: PostsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            try
            {
                _repository.Delete(post);
                FileHelper.FileRemover(fileName: post.Image);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(post);
            }
        }
    }
}
