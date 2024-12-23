namespace WebDeneme2.Models
{
    public class CalisanViewModel
    {
        public Calisan Calisan { get; set; }
        public List<Hizmet>? TumHizmetler { get; set; }
        public List<int> HizmetIds { get; set; }
    }
}
