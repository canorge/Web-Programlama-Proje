namespace WebDeneme2.Models
{
    public class AdminViewModel
    {
        public Admin Admin;
        public List<Calisan> Calisanlar { get; set; }
        public List<Musteri> Musteriler { get; set; }
        public List<Randevu> Randevular { get; set; }

        public List<Hizmet> TumHizmetler { get; set; }
        public List<int> HizmetIds { get; set; }
    }
}
