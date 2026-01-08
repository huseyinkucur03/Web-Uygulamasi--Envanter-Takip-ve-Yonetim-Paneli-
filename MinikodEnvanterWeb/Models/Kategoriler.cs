using System.ComponentModel.DataAnnotations;

namespace MinikodEnvanterWeb.Models
{
    public class Kategoriler
    {
        [Key]
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
		public string? DigerKategori { get; set; }
		public ICollection<Urunler> Urunler { get; set; }
    }
}
