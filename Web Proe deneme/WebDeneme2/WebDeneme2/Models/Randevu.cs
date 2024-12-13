namespace WebDeneme2.Models
{
    public class Randevu
    {
        public int Id { get; set; }
        public int MusteriId { get; set; }
        public Musteri? Musteri { get; set; }
        public int CalisanId { get; set; }
        public Calisan? Calisan { get; set; }
        public DateTime RandevuTarihi { get; set; }
    }
}
