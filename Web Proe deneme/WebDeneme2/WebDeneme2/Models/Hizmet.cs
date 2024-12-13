using System.ComponentModel.DataAnnotations.Schema;

namespace WebDeneme2.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public int Sure { get; set; } // Dakika cinsinden

        [Column(TypeName = "decimal(18,2)")]
        public decimal Ucret { get; set; }
        public ICollection<CalisanHizmet>? HizmetCalisanlari { get; set; }
    }
}
