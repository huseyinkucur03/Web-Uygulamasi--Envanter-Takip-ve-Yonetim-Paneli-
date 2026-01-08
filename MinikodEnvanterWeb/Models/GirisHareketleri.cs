using System.ComponentModel.DataAnnotations;

namespace MinikodEnvanterWeb.Models
{
    public class GirisHareketleri
    {
        [Key]
        public int GirisID {  get; set; }
        public string KullaniciAdi {  get; set; }
        public DateTime GirisTarihi { get; set; }
        public string Adres {  get; set; }
    }
}
