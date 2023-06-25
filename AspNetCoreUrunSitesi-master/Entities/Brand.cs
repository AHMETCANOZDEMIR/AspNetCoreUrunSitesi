using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Brand : IEntity
    {
        public int Id { get; set; }
        [DisplayName("Marka Adı"), Required(ErrorMessage = "Bu alan gereklidir"), StringLength(50)]
        public string Name { get; set; }
        [DisplayName("Marka Logosu"), StringLength(150)]
        public string Logo { get; set; }
        [DisplayName("Marka Açıklaması"), DataType(DataType.MultilineText)] // çoklu satır desteği
        public string Descripton { get; set; }
        [DisplayName("Durum")]
        public bool IsActive { get; set; }
        [DisplayName("Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
