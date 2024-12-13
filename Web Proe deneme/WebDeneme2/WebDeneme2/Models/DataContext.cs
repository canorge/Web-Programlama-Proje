using Microsoft.EntityFrameworkCore;

namespace WebDeneme2.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        }

        public DbSet<Calisan> Calisanlar { get; set; }
        public DbSet<Musteri> Musteriler { get; set; }
        public DbSet<Admin> Adminler { get; set; }
        public DbSet<Hizmet> Hizmetler { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<CalisanHizmet> CalisanHizmetleri { get; set; }
    }
}
