using System.ComponentModel.DataAnnotations;

namespace WebDeneme2.Models
{
    public class SifreYenileViewModel
    {
        [Required(ErrorMessage = "Email adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçersiz email adresi.")]
        public string? Email { get; set; }

        // Yeni şifre
        [Required(ErrorMessage = "Yeni şifre gereklidir.")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Sifre { get; set; }

        // Şifreyi onaylamak için
        [Required(ErrorMessage = "Şifreyi onaylamak gereklidir.")]
        [Compare("Sifre", ErrorMessage = "Şifreler uyuşmuyor.")]
        [DataType(DataType.Password)]
        public string? ConfirmSifre { get; set; }
    }
}
