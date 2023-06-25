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
            services.AddHttpClient(); // Controllerda HttpClient nesnesi ile api iþlemlerini kullanabilmek için
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSession(); // Projede session kullanmak istiyoruz
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer()); // .net core da DbContext imizi bu þekilde projeye bildiriyoruz
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Dependency Injection ile projemize IRepository ile nesne oluþurulursa oraya Repository classýndan bir örnek göndermesini söyledik
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                x.LoginPath = "/ApiAdmin/Login"; // Admin giriþ ekranýmýz
            });

            //Diðer Dependency Injection yöntemleri :
            // AddSingleton : Uygulama ayaða kalkarken çalýþan ConfigureServices metodunda bu yöntem ile tanýmladýðýmýz her sýnýftan sadece bir örnek oluþturulur. Kim nereden çaðýrýrsa çaðýrsýn kendisine bu örnek gönderilir. Uygulama yeniden baþlayana kadar yenisi üretilmez.
            // AddTransient : Uygulama çalýþma zamanýnda belirli koþullarda üretilir veya varolan örneði kullanýr. 
            // AddScoped : Uygulama çalýþýrken her istek için ayrý ayrý nesne üretilir.
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

            app.UseAuthentication(); // Uygulamada oturum açmayý aktif et
            app.UseAuthorization(); // Uygulamada yetkilendirmeyi aktif et

            app.UseEndpoints(endpoints =>
            {
                // ApiAdmin klasörünü routing de kullanabilmek için bu yapýlandýrma gerekli
                endpoints.MapControllerRoute(
                name: "ApiAdmin", // diðer arealarla çakýþmamasý için klasör ismini verdik
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
              );
                endpoints.MapControllerRoute(
                name: "admin", // area adýný buraya yazýyoruz
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
              );
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
