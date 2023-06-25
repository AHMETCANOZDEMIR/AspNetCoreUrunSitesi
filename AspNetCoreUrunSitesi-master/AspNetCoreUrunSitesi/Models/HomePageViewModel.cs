using Entities;
using System.Collections.Generic;

namespace AspNetCoreUrunSitesi.Models
{
    public class HomePageViewModel // Anasayfada kullanacağımız sayfa modeli
    {
        public List<Slider> Sliders { get; set; } // Anasayfada gösterilecek sliderların listesini tutacak nesnemiz
        public List<Product> Products { get; set; }
        public List<News> News { get; set; }
    }
}
