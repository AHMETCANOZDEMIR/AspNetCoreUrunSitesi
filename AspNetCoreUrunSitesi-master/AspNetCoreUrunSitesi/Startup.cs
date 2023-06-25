using BL;
using DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies; // Login

namespace AspNetCoreUrunSitesi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient(); // Controllerda HttpClient nesnesi ile api i�lemlerini kullanabilmek i�in
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession(); // Projede session kullanmak istiyoruz
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer()); // .net core da DbContext imizi bu �ekilde projeye bildiriyoruz
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Dependency Injection ile projemize IRepository ile nesne olu�urulursa oraya Repository class�ndan bir �rnek g�ndermesini s�yledik
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                x.LoginPath = "/ApiAdmin/Login"; // Admin giri� ekran�m�z
            });

            //Di�er Dependency Injection y�ntemleri :
            // AddSingleton : Uygulama aya�a kalkarken �al��an ConfigureServices metodunda bu y�ntem ile tan�mlad���m�z her s�n�ftan sadece bir �rnek olu�turulur. Kim nereden �a��r�rsa �a��rs�n kendisine bu �rnek g�nderilir. Uygulama yeniden ba�layana kadar yenisi �retilmez.
            // AddTransient : Uygulama �al��ma zaman�nda belirli ko�ullarda �retilir veya varolan �rne�i kullan�r. 
            // AddScoped : Uygulama �al���rken her istek i�in ayr� ayr� nesne �retilir.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Uygulamada oturum a�may� aktif et
            app.UseAuthorization(); // Uygulamada yetkilendirmeyi aktif et

            app.UseEndpoints(endpoints =>
            {
                // ApiAdmin klas�r�n� routing de kullanabilmek i�in bu yap�land�rma gerekli
                endpoints.MapControllerRoute(
                name: "ApiAdmin", // di�er arealarla �ak��mamas� i�in klas�r ismini verdik
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
              );
                endpoints.MapControllerRoute(
                name: "admin", // area ad�n� buraya yaz�yoruz
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
              );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
