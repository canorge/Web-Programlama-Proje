namespace WebDeneme2.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public string? Email { get; set; }
        public string? Sifre { get; set; }
        public string? TelNo { get; set; }
        public string? Cinsiyet { get; set; }
        public List<CalisanHizmet> UzmanlikAlanlari { get; set; } = new List<CalisanHizmet>();
        public List<Randevu> Randevular { get; set; } = new List<Randevu>();
    }
}
