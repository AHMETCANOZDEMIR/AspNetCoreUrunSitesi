using System.IO;
using Microsoft.AspNetCore.Http;

namespace WebApi.Utils
{
    public class FileHelper
    {
        public static string FileLoader(IFormFile formFile, string filePath = "/Img/") // FileLoader metodu sunucuya resim yüklemeyi sağlar. 1. parametre formFile = yüklenecek dosya bilgisi, 2. parametre filePath= dosyanın yükleneceği varsayılan klasör(eğer filepath e bilgi gönderilmezse dosya varsayılan olarak wwwroot içindeki ımg klasörüne yüklenir).
        {
            var fileName = ""; // dosya adı

            if (formFile != null && formFile.Length > 0) // dosya gerçekten var mı ve içi dolu mu kontrolü
            {
                fileName = formFile.FileName; // fileName değişkenine yüklenecek dosya adını aktardık
                string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName; // Dosyanın yükleneceği dizin = wwwroot/Img
                using var stream = new FileStream(directory, FileMode.Create); // Dosya akış nesnesi oluşturup directory = yüklenecek dizin, FileMode.Create ile de yeni ekleme modunu belirttik
                formFile.CopyTo(stream); // parametreden gelen dosyayı ilgili klasöre kopyala
            }

            return fileName; // Geriye de yüklenen dosyanın adını döndürdük ki bu ismi veritabanına kaydedebilelim.
        }
    }
}
