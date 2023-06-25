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
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> _repository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Brand> _brandRepository;
        public ProductsController(IRepository<Product> repository, IRepository<Category> categoryRepository, IRepository<Brand> brandRepository)
        {
            _repository = repository;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }

        // GET: ProductsController
        public async Task<ActionResult> IndexAsync()
        {
            return View(await _repository.GetAllAsync()); // sayfa modeline veritabanından çektiğimiz ürünleri gönderiyoruz, göndermezsek null references hatası alırız sayfa açılırken !!!
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductsController/Create
        public async Task<ActionResult> CreateAsync()
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Product product, IFormFile Image)
        {
            try
            {
                product.Image = FileHelper.FileLoader(Image);
                await _repository.AddAsync(product);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
                ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
                ModelState.AddModelError("", "Hata Oluştu!");
                return View(product);
            }
        }

        // GET: ProductsController/Edit/5
        public async Task<ActionResult> EditAsync(int id)
        {
            ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
            ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Product product, IFormFile Image, bool resmiSil)
        {
            try
            {
                if (Image != null) product.Image = FileHelper.FileLoader(Image);
                if (resmiSil == true)
                {
                    FileHelper.FileRemover(product.Image); // dosya silme metodumuzu çağırdık ve ürün resim adını silinmek üzere parametreyle gönderdik
                    product.Image = string.Empty;
                }
                _repository.Update(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.CategoryId = new SelectList(await _categoryRepository.GetAllAsync(), "Id", "Name");
                ViewBag.BrandId = new SelectList(await _brandRepository.GetAllAsync(), "Id", "Name");
                return View(product);
            }
        }

        // GET: ProductsController/Delete/5
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var data = await _repository.FindAsync(id);
            return View(data);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                FileHelper.FileRemover(product.Image);
                _repository.Delete(product);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(product);
            }
        }
    }
}
