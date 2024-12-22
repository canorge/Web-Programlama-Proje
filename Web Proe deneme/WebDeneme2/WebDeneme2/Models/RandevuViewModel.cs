using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebDeneme2.Models
{
    public class RandevuViewModel
    {
        public Randevu Randevu { get; set; }
        public Musteri Musteri { get; set; }
        public List<SelectListItem> Hizmetler { get; set; }
    }
}
