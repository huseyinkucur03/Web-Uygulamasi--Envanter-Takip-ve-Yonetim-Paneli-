using System.ComponentModel.DataAnnotations;

namespace MinikodEnvanterWeb.Models
{
    public class Kullanicilar
    {
        [Key]
        public int KullaniciID { get; set; }
        public string KullaniciAdi {  get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Sifre {  get; set; }
        public string Eposta {  get; set; }
        public string TelNo { get; set; }
        public string? Adres {  get; set; }
    }
}
