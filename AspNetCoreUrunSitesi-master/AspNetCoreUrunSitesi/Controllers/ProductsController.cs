using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreUrunSitesi.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;

        public ProductsController(IRepository<Product> productRepository, IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index(int? id) // Index action ı id parametresi alabilir ? işareti buranın null olabileceği yani gönderilmeyebileceği anlamına gelir
        {
            if (id != null)
            {
                var kategori = _categoryRepository.Find(id.Value);
                ViewBag.KategoriAdi = kategori.Name;
                ViewBag.KategoriAciklamasi = kategori.Descripton;
                return View(_productRepository.GetAll(x => x.CategoryId == id)); // Eğer id parametresine bir kategori id si gönderilmişse o zaman bu gelen kategori id ye eşit kategori id değeri atanmış ürünleri filtrele ve listele
            }
            ViewBag.KategoriAdi = "Tüm Ürünlerimiz";
            return View(_productRepository.GetAll()); // eğer bir kategori id si gönderilmemişse tüm ürünleri listele
        }

        public async Task<IActionResult> DetailAsync(int? id)
        {
            if (id == null) // Eğer detay sayfasına url den id gelmemişse
            {
                return BadRequest(); // Geçersiz istek hatası döndür çünkü id kesinlikle lazım
            }
            return View(await _productRepository.FindAsync(id.Value));
        }

        public IActionResult Search(string search) // Arama için kullanılan textbox a name olarak ne isim verdiysek burada "string textboxaverilenisim" ile yakalayabiliriz.
        {
            var aramasonucu = _productRepository.GetAll(s => s.Name.Contains(search));

            if (aramasonucu.Count() > 0) // Eğer gelen kelimeyi içeren ürün veya ürünler bulunmuşsa (count metodu sonuçları saymaya yarar)
            {
                ViewBag.AramaSonucu = search + " kelimesine ait ürünler";
            }
            else
            {
                ViewBag.AramaSonucu = search + " kelimesine ait ürün bulunamamıştır!";
            }

            return View(aramasonucu); // Ürünleri search ile query stringden gelen anahtar kelimeyi içerenleri kapsayacak şekilde filtrele
        }

    }
}
