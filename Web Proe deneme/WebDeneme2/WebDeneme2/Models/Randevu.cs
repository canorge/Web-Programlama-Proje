using System.ComponentModel.DataAnnotations;

namespace WebDeneme2.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public int MusteriId { get; set; }
        public Musteri? Musteri { get; set; }
        [Required(ErrorMessage ="Calışan seçimi zorunludur")]
        public int CalisanId { get; set; }
        public Calisan? Calisan { get; set; }
        [Required(ErrorMessage = "Hizmet seçimi zorunludur")]
        public int HizmetId { get; set; }

        public Hizmet? Hizmet { get; set; }
        [Required(ErrorMessage ="Tarih ve saat seçimi zorunludur")]
        public DateTime RandevuTarihi { get; set; }

        public string? RandevuDurum { get; set; }
    }
}
