using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        [DisplayName("Başlık"), Required(ErrorMessage = "Başlık Boş Geçilemez!"), StringLength(50)]
        public string Name { get; set; }
        [DisplayName("İçerik"), Required(ErrorMessage = "İçerik Boş Geçilemez!"), DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DisplayName("Resim"), StringLength(150)]
        public string Image { get; set; }
        [DisplayName("Ürün Fiyatı"), Required(ErrorMessage = "Ürün Fiyatı Boş Geçilemez!")]
        public decimal Price { get; set; }
        [DisplayName("Ürün Stok"), Required(ErrorMessage = "Ürün Stok Boş Geçilemez!")]
        public int Stock { get; set; }
        [DisplayName("Durum")]
        public bool IsActive { get; set; }
        [DisplayName("Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        [DisplayName("Ürün Kategorisi")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [DisplayName("Ürün Markası")]
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
    }
}
