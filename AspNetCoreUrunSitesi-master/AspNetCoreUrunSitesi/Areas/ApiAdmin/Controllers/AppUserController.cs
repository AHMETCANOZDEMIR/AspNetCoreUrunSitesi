using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http; // Aşağıdaki IHttpClientFactory interface ini kullanabilmek için
using System.Text; // Api işlemlerinde gerekli
using System.Threading.Tasks;
using Newtonsoft.Json; // Bu paketi nuget tan yüklüyoruz. String verileri Json formatına, Json formatındaki verileri de string formata çevirmemizi sağlıyor
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreUrunSitesi.Areas.ApiAdmin.Controllers
{
    [Area("ApiAdmin"), Authorize] // Authorize attrubute u bu controller daki tüm actionlara sadece oturum açan admin kullanıcılarının erişmesini sağlamak için gerekli, bunu koymazsak panelimize adresi yazan herkes erişir!!
    public class AppUserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        string apiAdres = "https://localhost:44395/api/AppUsers";
        public AppUserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: AppUserController
        public async Task<ActionResult> IndexAsync()
        {
            var client = _httpClientFactory.CreateClient(); // Api işlemleri için bir kullanıcı oluşturduk
            var responseMessage = await client.GetAsync(apiAdres); // API mize istek yapıyoruz
            if (responseMessage.IsSuccessStatusCode) // api ye yaptığımız isteğin sonucu başarılıysa
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync(); // responseMessage içeriğini json olarak okuyoruz
                var result = JsonConvert.DeserializeObject<List<AppUser>>(jsonData); // aldığımız jsondata yı List<AppUser> ile AppUser listesine çeviriyoruz convert metoduyla
                return View(result); // Sayfa modelimiz olan appuser listesine çevirdiğimiz modeli view a gönderiyoruz
            }
            return View(null); // eğer yukardaki işlem başarısızsa null değer döndürüyoruz
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
        public async Task<ActionResult> CreateAsync(AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var jsonData = JsonConvert.SerializeObject(appUser); // parametreden gelen appUser nesnesini json a newtonsoft ile dönüştürdük
                    StringContent stringContent = new(jsonData, encoding: Encoding.UTF8, mediaType: "application/json"); // stringContent nesnesi oluşturup içine json a dönüştürdüğümüz appUser nesnesini attık ve encoding ile kodlama türünü UTF8 olarak ayarladık, mediaType kısmında ise bu nesnenin veri türünün json olduğunu belirttik.
                    var responseMessage = await client.PostAsync(apiAdres, stringContent); // client ın PostAsync metodu ile api mize post isteği yaptık
                    if (responseMessage.IsSuccessStatusCode) // eğer post isteğimiz başarılıysa
                    {
                        return RedirectToAction(nameof(Index)); // sayfayı listelemeye yönlendir
                    }
                    else ModelState.AddModelError("", $"Post İsteğinde Hata Oluştu! Hata Kodu : {(int)responseMessage.StatusCode}"); // postta hata olursa ne hatası aldığımızı görmek için
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }

            return View(appUser);
        }

        // GET: AppUserController/Edit/5
        public async Task<ActionResult> EditAsync(int? id) // int? soru işareti id bilgisin boş geçilebileceği anlamına gelir
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync(apiAdres + "/" + id); // client.GetAsync metodu ile api ye bir get isteği gönderilir. Sonuna id bilgisini de gönderiyoruz ki tüm kayıtlar gelmesin sadece gönderdiğimiz id ye ait kayıt bilgisi gelsin.
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<AppUser>(jsonData);
                return View(data);
            }
            return View();
        }

        // POST: AppUserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, AppUser appUser)
        {
            if (id != appUser.Id) // Eğer güncelleme için gönderilen url deki id ile appuser ın id si eşleşmiyorsa
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var jsonData = JsonConvert.SerializeObject(appUser); // sayfadan gelen appUser ı json a çevir
                    StringContent stringContent = new(jsonData, Encoding.UTF8, "application/json");
                    var responseMessage = await client.PutAsync(apiAdres + "/" + id, stringContent); // Api ye kayıt güncelleme için client.PutAsync metodu ile bir put isteği gönderiyoruz. Api de güncelleme metodu put çünkü.
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else ModelState.AddModelError("", "Kayıt Güncellenemedi!");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu! Kayıt Güncellenemedi!");
                }
            }
            return View(appUser); // eğer model geçersizse appUser ı sayfaya hatalarıyla beraber geri gönder
        }

        // GET: AppUserController/Delete/5
        public async Task<ActionResult> DeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync(apiAdres + "/" + id);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<AppUser>(jsonData);
                return View(data);
            }
            return View();
        }

        // POST: AppUserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                await client.DeleteAsync(apiAdres + "/" + id); // Api ye client.DeleteAsync metodu ile delete isteği gönderiyoruz, silinecek id ile birlikte isteği yaptığımızda kayıt silinecektir.

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
