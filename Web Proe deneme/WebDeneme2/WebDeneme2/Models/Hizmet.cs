using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;

namespace WebDeneme2.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Aciklama { get; set; }
        public string? Image { get; set; }
        public int Sure { get; set; } // Dakika cinsinden

        [Column(TypeName = "decimal(18,2)")]
        public decimal Ucret { get; set; }
        public ICollection<CalisanHizmet>? HizmetCalisanlari { get; set; }
    }
}
