using Microsoft.EntityFrameworkCore;

namespace MinikodEnvanterWeb.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Urunler> Urunler { get; set; }
        public DbSet<Kullanicilar> Kullanicilar {  get; set; }
        public DbSet<Kategoriler> Kategoriler { get; set; }
        public DbSet<GirisHareketleri> GirisHareketleri {  get; set; }
        public DbSet<Markalar> Markalar {  get; set; }
	}
}
