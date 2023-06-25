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
    public class CategoriesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CategoriesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        string apiAdres = "https://localhost:44395/api/Categories";

        // GET: CategoriesController
        public async Task<ActionResult> IndexAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync(apiAdres);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<Category>>(jsonData);
                return View(result);
            }
            return View(null);
        }

        // GET: CategoriesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CategoriesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var jsonData = JsonConvert.SerializeObject(category);
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
            return View(category);
        }

        // GET: CategoriesController/Edit/5
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
                var data = JsonConvert.DeserializeObject<Category>(jsonData);
                return View(data);
            }
            return View();
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var jsonData = JsonConvert.SerializeObject(category);
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
            return View(category);
        }

        // GET: CategoriesController/Delete/5
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
                var data = JsonConvert.DeserializeObject<Category>(jsonData);
                return View(data);
            }
            return View();
        }

        // POST: CategoriesController/Delete/5
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
