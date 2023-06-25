using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Entities;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreUrunSitesi.Areas.ApiAdmin.Controllers
{
    [Area("ApiAdmin"), Authorize]
    public class BrandsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BrandsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        string apiAdres = "https://localhost:44395/api/Brands";

        // GET: BrandsController
        public async Task<ActionResult> IndexAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync(apiAdres);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Brand>>(jsonData);
                return View(result);
            }
            return View(null);
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
        public async Task<ActionResult> CreateAsync(Brand brand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var jsonData = JsonConvert.SerializeObject(brand);
                    StringContent stringContent = new(jsonData, encoding: Encoding.UTF8, mediaType: "application/json");
                    var responseMessage = await client.PostAsync(apiAdres, stringContent);
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else ModelState.AddModelError("", $"Post İsteğinde Hata Oluştu! Hata Kodu : {(int)responseMessage.StatusCode}");
                }
                catch
                {
                    ModelState.AddModelError("", "Hata Oluştu!");
                }
            }
            return View(brand);
        }

        // GET: BrandsController/Edit/5
        public async Task<ActionResult> EditAsync(int? id)
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
                var data = JsonConvert.DeserializeObject<Brand>(jsonData);
                return View(data);
            }
            return View();
        }

        // POST: BrandsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var jsonData = JsonConvert.SerializeObject(brand);
                    StringContent stringContent = new(jsonData, Encoding.UTF8, "application/json");
                    var responseMessage = await client.PutAsync(apiAdres + "/" + id, stringContent);
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
            return View(brand);
        }

        // GET: BrandsController/Delete/5
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
                var data = JsonConvert.DeserializeObject<Brand>(jsonData);
                return View(data);
            }
            return View();
        }

        // POST: BrandsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id, IFormCollection collection)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                await client.DeleteAsync(apiAdres + "/" + id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
