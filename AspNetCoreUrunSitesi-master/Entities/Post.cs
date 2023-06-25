using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Post : IEntity
    {
        public int Id { get; set; }
        [DisplayName("Başlık"), Required(ErrorMessage = "Başlık Boş Geçilemez!"), StringLength(50)]
        public string Name { get; set; }
        [DisplayName("İçerik"), Required(ErrorMessage = "İçerik Boş Geçilemez!"), DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DisplayName("Resim"), StringLength(150)]
        public string Image { get; set; }
        [DisplayName("Durum")]
        public bool IsActive { get; set; }
        [DisplayName("Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
        [DisplayName("İçerik Kategorisi")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
