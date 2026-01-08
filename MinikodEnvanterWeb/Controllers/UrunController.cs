using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MinikodEnvanterWeb.Migrations;
using MinikodEnvanterWeb.Models;
using OfficeOpenXml;
using OfficeOpenXml.Sorting;
using System.IO.Packaging;
using System.Reflection;
using System.Security.Claims;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using DocumentFormat.OpenXml.InkML;
using Paragraph = iTextSharp.text.Paragraph;
using Table = iText.Layout.Element.Table;

namespace MinikodEnvanterWeb.Controllers
{
    [Authorize]
    public class UrunController : Controller
    {
        AppDbContext _context;
        public UrunController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string p, int? markaId, int? kategoriId)
        {
            // Önce sorguyu kuruyoruz, ardından Include ile ilişkili verileri ekliyoruz
            var urunler = _context.Urunler.AsQueryable();

            var adres = User.FindFirst("Adres")?.Value;

            if (adres != null && adres != " " && adres != "")
            {
                urunler = urunler.Where(u => u.Adres == adres);
            }
            else
            {
                ViewBag.AdresBossMu = true;
            }


            if (!string.IsNullOrEmpty(p))
            {
                urunler = urunler.Where(u => u.UrunAdi.Contains(p) ||
                                          u.Markalar.MarkaAdi.Contains(p) ||
                                          u.Kategoriler.KategoriAdi.Contains(p));
            }

            if (markaId.HasValue)
            {
                urunler = urunler.Where(u => u.MarkaID == markaId);
            }
            if (kategoriId.HasValue)
            {
                urunler = urunler.Where(u => u.KategoriID == kategoriId);
            }

            var markalar = await _context.Markalar.ToListAsync();
            ViewBag.Markalar = new SelectList(markalar, "MarkaID", "MarkaAdi");

            var kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.Kategoriler = new SelectList(kategoriler, "KategoriId", "KategoriAdi");

            // Include metodlarını en son ekliyoruz
            urunler = urunler.Include(u => u.Kategoriler)
                               .Include(u => u.Markalar);

            return View(await urunler.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> UrunEkle()
        {
            var kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.Kategoriler = new SelectList(kategoriler, "KategoriId", "KategoriAdi");
            var markalar = await _context.Markalar.ToListAsync();
            ViewBag.Markalar = new SelectList(markalar, "MarkaID", "MarkaAdi");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UrunEkle(Urunler urun, IFormFile Resim, string DigerKategori, string DigerMarka)
        {
            var kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.Kategoriler = new SelectList(kategoriler, "KategoriId", "KategoriAdi");
            var markalar = await _context.Markalar.ToListAsync();
            ViewBag.Markalar = new SelectList(markalar, "MarkaID", "MarkaAdi");

            if (urun.KategoriID == 0 && !string.IsNullOrEmpty(DigerKategori))
            {
                DigerKategori = DigerKategori.ToUpper();
                var yeniKategori = new Kategoriler { KategoriAdi = DigerKategori };
                await _context.Kategoriler.AddAsync(yeniKategori);
                await _context.SaveChangesAsync();
                urun.KategoriID = yeniKategori.KategoriId;
            }

            if (urun.MarkaID == 0 && !string.IsNullOrEmpty(DigerMarka))
            {
                DigerMarka = DigerMarka.ToUpper();
                var yeniMarka = new Markalar { MarkaAdi = DigerMarka };
                await _context.Markalar.AddAsync(yeniMarka);
                await _context.SaveChangesAsync();
                urun.MarkaID = yeniMarka.MarkaID;
            }

            if (urun.CalismayanSayisi > urun.ToplamSayi)
            {
                ModelState.AddModelError(string.Empty, "Çalışmayan ürün sayısı toplam sayıdan büyük olamaz.");
                return View(urun);
            }

            try
            {
                if (Resim != null && Resim.Length > 0)
                {
                    var dosyaAdi = Path.GetFileName(Resim.FileName);
                    var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/urungorsel");

                    if (!Directory.Exists(dosyaYolu))
                    {
                        Directory.CreateDirectory(dosyaYolu);
                    }

                    dosyaYolu = Path.Combine(dosyaYolu, dosyaAdi);

                    using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                    {
                        await Resim.CopyToAsync(stream);
                    }
                    urun.Resim = "/urungorsel/" + dosyaAdi;
                }
                

                urun.UrunAdi = urun.UrunAdi?.ToUpper(); 
                urun.CalismamaNedeni = urun.CalismamaNedeni?.ToUpper();
                urun.Aciklama = urun.Aciklama?.ToUpper();
                urun.KullaniciAdi = User.FindFirst("KullaniciAdi")?.Value; // Kullanıcı adını Claims'den alıyoruz.
                urun.EklenmeTarihi = DateTime.Now;
                urun.Adres = User.FindFirst("Adres")?.Value;

                var mevcutUrun = await _context.Urunler.FirstOrDefaultAsync(
                    u => u.UrunAdi == urun.UrunAdi && u.MarkaID == urun.MarkaID && u.KategoriID == urun.KategoriID);

                if (urun.CalismayanSayisi != 0 && urun.CalismamaNedeni == null)
                {
                    ModelState.AddModelError(string.Empty, "Lütfen çalışmama nedeni girin.");
                    return View(urun);
                }

                if (mevcutUrun != null)
                {
                    mevcutUrun.ToplamSayi += urun.ToplamSayi;
                    mevcutUrun.CalismayanSayisi += urun.CalismayanSayisi;
                    mevcutUrun.CalismamaNedeni = urun.CalismamaNedeni?.ToUpper();
                    mevcutUrun.SonDegistirilmeTarihi = DateTime.Now;
                    mevcutUrun.Resim = urun.Resim;

                    _context.Urunler.Update(mevcutUrun);
                }
                else
                {
                    await _context.Urunler.AddAsync(urun);
                }
                await _context.SaveChangesAsync();
                TempData["BasariylaEklendi"] = "Ürün(ler) başarıyla eklendi.";

                return RedirectToAction("Index");
            }
            catch
            {
                return View(urun);
            }
        }
        public async Task<IActionResult> UrunSil(int id)
        {
            var urun = await _context.Urunler.FindAsync(id);
            _context.Urunler.Remove(urun);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UrunGuncelle(int id)
        {
            var kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.Kategoriler = new SelectList(kategoriler, "KategoriId", "KategoriAdi");
            var markalar = await _context.Markalar.ToListAsync();
            ViewBag.Markalar = new SelectList(markalar, "MarkaID", "MarkaAdi");

            var urun = await _context.Urunler.FindAsync(id);
            urun.SonDegistirilmeTarihi = DateTime.Now;
            return View(urun);
        }
        [HttpPost]
        public async Task<IActionResult> UrunGuncelle(int id, Urunler urun, IFormFile Resim)
        {
            var kategoriler = await _context.Kategoriler.ToListAsync();
            ViewBag.Kategoriler = new SelectList(kategoriler, "KategoriId", "KategoriAdi");
            var markalar = await _context.Markalar.ToListAsync();
            ViewBag.Markalar = new SelectList(markalar, "MarkaID", "MarkaAdi");

            var mevcutUrun = await _context.Urunler.FindAsync(id);
            if (mevcutUrun == null)
            {
                return NotFound();
            }

            if (Resim != null && Resim.Length > 0)
            {
                if (!string.IsNullOrEmpty(mevcutUrun.Resim))
                {
                    var eskiDosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/urungorsel");
                    if (System.IO.File.Exists(eskiDosyaYolu))
                    {
                        System.IO.File.Delete(eskiDosyaYolu);
                    }
                }

                var dosyaAdi = Path.GetFileName(Resim.FileName);
                var dosyaYolu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/urungorsel");

                // Klasörün var olup olmadığını kontrol edin ve yoksa oluşturun
                if (!Directory.Exists(dosyaYolu))
                {
                    Directory.CreateDirectory(dosyaYolu);
                }

                dosyaYolu = Path.Combine(dosyaYolu, dosyaAdi);

                using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                {
                    await Resim.CopyToAsync(stream);
                }
                urun.Resim = "/urungorsel/" + dosyaAdi;
            }

            mevcutUrun.UrunAdi = urun.UrunAdi?.ToUpper();
            mevcutUrun.KategoriID = urun.KategoriID;
            mevcutUrun.MarkaID = urun.MarkaID;
            mevcutUrun.Aciklama = urun.Aciklama?.ToUpper();
            mevcutUrun.CalismamaNedeni = urun.CalismamaNedeni?.ToUpper();
            mevcutUrun.SonDegistirilmeTarihi = DateTime.Now;
            mevcutUrun.Resim = urun.Resim;

            if (mevcutUrun.ToplamSayi <= mevcutUrun.CalismayanSayisi || urun.CalismayanSayisi > mevcutUrun.ToplamSayi || mevcutUrun.ToplamSayi < mevcutUrun.CalismayanSayisi + urun.CalismayanSayisi)
            {
                ModelState.AddModelError(string.Empty, "Çalışmayan ürün sayısı toplam ürün sayısından fazla olamaz.");
                return View(mevcutUrun);
            }
            else
            {
                mevcutUrun.CalismayanSayisi += urun.CalismayanSayisi;
            }

            if (urun.CalismayanSayisi != 0 && urun.CalismamaNedeni == null)
            {
                ModelState.AddModelError(string.Empty, "Lütfen çalışmama nedeni girin.");
                return View(urun);
            }

            _context.Urunler.Update(mevcutUrun);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ExcelYukle(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["ExcelDosyaHatasi"] = "Lütfen geçerli bir Excel dosyası yükleyin.";
                return RedirectToAction("UrunEkle", "Urun");
            }

            var kategoriler = await _context.Kategoriler.ToListAsync();
            var markalar = await _context.Markalar.ToListAsync();

            using (var package = new ExcelPackage(excelFile.OpenReadStream()))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // 1. satır başlıklar için
                {
                    var urunAdi = worksheet.Cells[row, 1].Value?.ToString().Trim()?.ToUpper();
                    var urunMarkasi = worksheet.Cells[row, 2].Value?.ToString().Trim()?.ToUpper();
                    var aciklama = worksheet.Cells[row, 3].Value?.ToString().Trim()?.ToUpper();
                    var toplamSayiStr = worksheet.Cells[row, 4].Value?.ToString().Trim();
                    var calismayanSayisiStr = worksheet.Cells[row, 5].Value?.ToString().Trim();
                    var kategoriAdi = worksheet.Cells[row, 6].Value?.ToString().Trim()?.ToUpper();

                    // Kategori adı boş ya da null ise bu satırı atla
                    if (string.IsNullOrEmpty(kategoriAdi))
                    {
                        ModelState.AddModelError(string.Empty, $"Satır {row} için kategori adı boş olamaz.");
                        continue;
                    }

                    // KategoriId'yi kategoriler tablosundan al
                    var kategori = kategoriler.FirstOrDefault(k => k.KategoriAdi == kategoriAdi);
                    if (kategori == null)
                    {
                        ModelState.AddModelError(string.Empty, $"Kategori '{kategoriAdi}' bulunamadı.");
                        continue;
                    }

                    var marka = markalar.FirstOrDefault(m => m.MarkaAdi == urunMarkasi);
                    if (marka == null)
                    {
                        ModelState.AddModelError(string.Empty, $"Marka '{urunMarkasi}' bulunamadı.");
                        continue;
                    }

                    int toplamSayi = int.TryParse(toplamSayiStr, out int ts) ? ts : 0;
                    int calismayanSayisi = int.TryParse(calismayanSayisiStr, out int cs) ? cs : 0;

                    var urun = new Urunler
                    {
                        UrunAdi = urunAdi,
                        MarkaID = marka.MarkaID,
                        KategoriID = kategori.KategoriId,
                        Aciklama = aciklama,
                        ToplamSayi = toplamSayi,
                        CalismayanSayisi = calismayanSayisi,
                        EklenmeTarihi = DateTime.Now,
                        KullaniciAdi = User.FindFirst("KullaniciAdi")?.Value,
                        Resim = " ",
                        Adres = User.FindFirst("Adres")?.Value
                    };

                    if (urun.CalismayanSayisi > urun.ToplamSayi)
                    {
                        ModelState.AddModelError(string.Empty, $"{urunAdi} için Çalışmayan ürün sayısı Toplam Sayıdan büyük olamaz.");
                        continue;
                    }

                    await _context.Urunler.AddAsync(urun);
                }

                await _context.SaveChangesAsync();
            }
            TempData["BasariylaEklendi"] = "Ürün(ler) başarıyla eklendi.";
            return RedirectToAction("Index");
        }
    }
}