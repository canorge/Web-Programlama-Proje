using System.ComponentModel.DataAnnotations;

namespace WebDeneme2.Models
{
    public class MusteriRegister
    {
        [Required(ErrorMessage = "Ad gereklidir.")]
        [StringLength(50, ErrorMessage = "Ad en fazla {1} karakter olabilir.")]
        public string? Ad { get; set; }

        [Required(ErrorMessage = "Soyad gereklidir.")]
        [StringLength(50, ErrorMessage = "Soyad en fazla {1} karakter olabilir.")]
        public string? Soyad { get; set; }

        [Required(ErrorMessage = "Email gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [StringLength(100, ErrorMessage = "Şifre en az {2} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string? Sifre { get; set; }

        [Required(ErrorMessage = "Şifre tekrar alanı gereklidir.")]
        [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor.")]
        [DataType(DataType.Password)]
        public string? SifreTekrar { get; set; }

        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        public string? TelNo { get; set; }

        [Required(ErrorMessage = "Cinsiyet seçimi zorunludur.")]
        public string? Cinsiyet { get; set; }
    }
}
