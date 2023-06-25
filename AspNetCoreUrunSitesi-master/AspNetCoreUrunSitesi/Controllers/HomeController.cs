using AspNetCoreUrunSitesi.Models;
using BL;
using Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AspNetCoreUrunSitesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IRepository<Slider> _sliderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<News> _newsRepository;

        public HomeController(IRepository<Contact> contactRepository, IRepository<Slider> sliderRepository, IRepository<Product> productRepository, IRepository<News> newsRepository)
        {
            _contactRepository = contactRepository;
            _sliderRepository = sliderRepository;
            _productRepository = productRepository;
            _newsRepository = newsRepository;
        }

        public IActionResult Index()
        {
            var model = new HomePageViewModel() // Anasayfa modelimizden bir nesne oluşturduk
            {
                Sliders = _sliderRepository.GetAll(), // Modelimizin içindeki slider listesini doldurduk
                Products = _productRepository.GetAll(),
                News = _newsRepository.GetAll()
            };
            return View(model); // İçini doldurduğumuz modelimizi view a gönderdik
        }

        public IActionResult Contacts()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ContactsAsync(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Utils.MailHelper.SendMail(contact); // parametreden gelen contact nesnesi içindeki bilgileri mail ile göndermek için.
                    await _contactRepository.AddAsync(contact);
                    var sonuc = await _contactRepository.SaveChangesAsync();
                    if (sonuc > 0)
                    {
                        TempData["Mesaj"] = "<div class='alert alert-success'>Teşekkürler.. Mesajınız İletildi!</div>";
                    }
                    else TempData["Mesaj"] = "<div class='alert alert-info'>Mesajınız Gönderilemedi!</div>";
                    return RedirectToAction("Contacts");
                }
                catch (Exception)
                {
                    TempData["Mesaj"] = "<div class='alert alert-danger'>Hata Oluştu! <br /> Mesajınız Gönderilemedi!</div>";
                }
            }
            return View(contact);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
