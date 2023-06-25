using System.IO;
using Microsoft.AspNetCore.Http;

namespace AspNetCoreUrunSitesi.Utils
{
    public class FileHelper
    {
        public static string FileLoader(IFormFile formFile, string filePath = "/Img/")
        {
            var fileName = "";

            if (formFile != null && formFile.Length > 0)
            {
                fileName = formFile.FileName;
                string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;
                using var stream = new FileStream(directory, FileMode.Create);
                formFile.CopyTo(stream);
            }

            return fileName;
        }
        public static bool FileRemover(string fileName, string filePath = "/Img/")
        {
            string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;
            if (File.Exists(directory)) // File.Exists metodu kendisine parametrede verilen dosyanın var olup olmadığını kontrol eder ve buna göre geriye dosya varsa true, yoksa false döndürür.
            {
                File.Delete(directory); // verilen dizindeki dosyayı sunucudan sil.
                return true; // silme başarılıysa geriye true dön
            }
            return false; // Buraya düştüyse silme başarısızdır geriye false dön ki metodu kullanacağımız yerde işlemin başarısız olduğunu bilelim.
        }
    }
}
