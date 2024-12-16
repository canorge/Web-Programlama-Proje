using System.ComponentModel.DataAnnotations;

namespace WebDeneme2.Models
{
    public class GirisViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        public string? Sifre { get; set; }
    }
}
