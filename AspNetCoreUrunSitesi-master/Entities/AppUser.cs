using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class AppUser : IEntity
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
        [DisplayName("Kullanıcı Adı"), StringLength(50), Required(ErrorMessage = "Bu alan gereklidir")]
        public string Username { get; set; }
        [DisplayName("Şifre"), StringLength(150), Required(ErrorMessage = "Bu alan gereklidir"), DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Durum")]
        public bool IsActive { get; set; }
        [DisplayName("Admin?")]
        public bool IsAdmin { get; set; }
        [DisplayName("Eklenme Tarihi"), ScaffoldColumn(false)]
        public DateTime? CreateDate { get; set; } = DateTime.Now;
    }
}
