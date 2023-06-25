using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Contact : IEntity
    {
        public int Id { get; set; }
        [DisplayName("Adınız"), Required(ErrorMessage = "Bu alan gereklidir"), StringLength(50)]
        public string Name { get; set; }
        [DisplayName("Soyadınız"), Required(ErrorMessage = "Bu alan gereklidir"), StringLength(50)]
        public string Surname { get; set; }
        [DisplayName("Mail Adresiniz"), Required(ErrorMessage = "Bu alan gereklidir"), StringLength(50)]
        public string Email { get; set; }
        [DisplayName("Telefon"), StringLength(15)]
        public string Phone { get; set; }
        [DisplayName("Mesajınız"), Required(ErrorMessage = "Bu alan gereklidir"), DataType(DataType.MultilineText)]
        public string Message { get; set; }
        [DisplayName("Mesaj Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
