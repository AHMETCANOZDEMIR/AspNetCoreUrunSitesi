using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Slider : IEntity
    {
        public int Id { get; set; }
        [DisplayName("Başlık"), StringLength(50)]
        public string Name { get; set; }
        [DisplayName("İçerik"), DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DisplayName("Resim"), StringLength(150), Required(ErrorMessage = "Bu alan gereklidir")]
        public string Image { get; set; }
        [DisplayName("Resim Link"), StringLength(100)]
        public string Link { get; set; }
    }
}
