using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DatabaseContext : DbContext
    {
        // .Net Core da Entity framework core mevcut fakat sql işlemleri için nugettan 4 farklı paket yüklemeliyiz bunlar; EntityFrameworkCore, EntityFrameworkCore.Desgin, EntityFrameworkCore.SqlServer, EntityFrameworkCore.Tools. Bunları yüklememiz gerekiyor
        public DatabaseContext()
        {

        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }
        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // OnConfiguring metodunda uygulamamızda Sql Server kullanacağımızı bildiriyoruz
            optionsBuilder.UseSqlServer(@"Server=(LocalDB)\MSSQLLocalDB; Database=AspNetCoreUrunSitesi; Trusted_Connection=True; MultipleActiveResultSets=True"); // optionsBuilder.UseSqlServer metoduna bu şekilde parametreyle connection stringimizi yazıyoruz

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // OnModelCreating metodu da modelimiz oluşurken çalışır ve burada oluşacak veritabanı tablolarına başlangıç değerleri atayabiliriz. Standart kullanıcı vb gibi..
            modelBuilder.Entity<AppUser>()
            .HasData(
            new AppUser
            {
                Id = 1,
                CreateDate = DateTime.Now,
                Email = "admin@AspNetCoreUrunSitesi",
                IsActive = true,
                IsAdmin = true,
                Name = "Admin",
                Surname = "User",
                Username = "Admin",
                Password = "123456"
            }); // HasData metoduyla başlangıç kullanıcısı 
            base.OnModelCreating(modelBuilder);
        }
    }
}
