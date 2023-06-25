using BL;
using Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreUrunSitesi.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IRepository<AppUser> _repository;

        public LoginController(IRepository<AppUser> repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(AppUser appUser)
        {
            try
            {
                var account = _repository.Get(x => x.Username == appUser.Username & x.Password == appUser.Password & x.IsActive & x.IsAdmin);
                if (account == null) // Girilen bilgilere göre eşleşen kayıt yoksa
                {
                    ModelState.AddModelError("", "Giriş Başarısız!");
                }
                else
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Email, account.Email)
                    };
                    var userIdentity = new ClaimsIdentity(claims, "Login");
                    ClaimsPrincipal principal = new(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    return Redirect("/Admin/Home");
                }
            }
            catch (Exception hata)
            {
                ModelState.AddModelError("", "Hata Oluştu!");
            }
            return View();
        }
        [Route("Admin/Logout")] // Bu adrese gelen istek olursa çıkış yap
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
