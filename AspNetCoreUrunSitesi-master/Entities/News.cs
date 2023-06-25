using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class News : IEntity
    {
        public int Id { get; set; }
        [DisplayName("Başlık"), Required(ErrorMessage = "Bu alan gereklidir"), StringLength(50)]
        public string Name { get; set; }
        [DisplayName("İçerik"), Required(ErrorMessage = "Bu alan gereklidir"), DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DisplayName("Resim"), StringLength(150)]
        public string Image { get; set; }
        [DisplayName("Durum")]
        public bool IsActive { get; set; }
        [DisplayName("Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
