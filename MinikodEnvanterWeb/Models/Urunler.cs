using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinikodEnvanterWeb.Models
{
    public class Urunler
    {
        [Key]
        public int UrunID { get; set; }
        [ForeignKey("Kategoriler")]
        public int KategoriID { get; set; }
        public Kategoriler Kategoriler { get; set; }
        [ForeignKey("Markalar")]
        public int MarkaID { get; set; }
        public  Markalar Markalar { get; set; }
        public string UrunAdi {  get; set; }
        public DateTime EklenmeTarihi { get; set; }
        public string? Aciklama {  get; set; }
        public int ToplamSayi {  get; set; }
        public int CalismayanSayisi { get; set; }
        public DateTime SonDegistirilmeTarihi { get; set; }
        public string? CalismamaNedeni {  get; set; }
        public string? Resim {  get; set; }
        public string KullaniciAdi {  get; set; }
        public string Adres {  get; set; }
    }
}
