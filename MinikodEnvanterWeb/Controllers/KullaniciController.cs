using iText.Commons.Bouncycastle.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinikodEnvanterWeb.Models;

namespace MinikodEnvanterWeb.Controllers
{
	public class KullaniciController : Controller
	{
		AppDbContext _context;
		public KullaniciController(AppDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Kullanicilar()
		{
			var kullanicilar = await _context.Kullanicilar.ToListAsync();
			return View(kullanicilar);
		}
		[HttpGet]
		public async Task<IActionResult> KullaniciGuncelle(int id)
		{
			var kullanici = await _context.Kullanicilar.FindAsync(id);
			return View(kullanici);
		}
		[HttpPost]
		public async Task<IActionResult> KullaniciGuncelle(int id, Kullanicilar kullanici)
		{
			if (string.IsNullOrEmpty(kullanici.Ad) || string.IsNullOrEmpty(kullanici.Soyad) ||
				string.IsNullOrEmpty(kullanici.KullaniciAdi) || string.IsNullOrEmpty(kullanici.Eposta) ||
				string.IsNullOrEmpty(kullanici.TelNo))
			{
				// Alanlar boş, kullanıcıyı uyar
				ModelState.AddModelError("", "Lütfen tüm alanları doldurun.");
				return View("KullaniciGuncelle", kullanici); // Profil görüntüleme sayfasına geri döndür
			}

			var mevcutUrun = await _context.Kullanicilar.FindAsync(id);
			mevcutUrun.Ad = kullanici.Ad;
			mevcutUrun.Soyad = kullanici.Soyad;
			mevcutUrun.KullaniciAdi = kullanici.KullaniciAdi;
			mevcutUrun.Eposta = kullanici.Eposta;
			mevcutUrun.TelNo = kullanici.TelNo;
			mevcutUrun.Adres = kullanici.Adres;

			_context.Kullanicilar.Update(mevcutUrun);
			await _context.SaveChangesAsync();
			TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
			return RedirectToAction("Kullanicilar");
		}
		public async Task<IActionResult> KullaniciSil(int id)
		{
			var kullanici = await _context.Kullanicilar.FindAsync(id);
			_context.Kullanicilar.Remove(kullanici);
			await _context.SaveChangesAsync();
			TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
			return RedirectToAction("Kullanicilar");
		}
		[HttpGet]
		public async Task<IActionResult> Profil(int id)
		{
			var kullaniciAdi = User.FindFirst("KullaniciAdi")?.Value;
			var profil = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi);

			return View(profil);
		}
		[HttpPost]
		public async Task<IActionResult> Profil(string SifreTekrar, Kullanicilar kullanici)
		{
			var kullaniciAdi = User.FindFirst("KullaniciAdi")?.Value;
			var profil = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciAdi == kullaniciAdi);


			if (string.IsNullOrEmpty(kullanici.Ad) || string.IsNullOrEmpty(kullanici.Soyad) ||
	  string.IsNullOrEmpty(kullanici.KullaniciAdi) || string.IsNullOrEmpty(kullanici.Eposta) ||
	  string.IsNullOrEmpty(kullanici.TelNo) || string.IsNullOrEmpty(kullanici.Sifre))
			{
				// Alanlar boş, kullanıcıyı uyar
				ModelState.AddModelError("", "Lütfen tüm alanları doldurun.");
				return View("Profil", kullanici); // Profil görüntüleme sayfasına geri döndür
			}


            if (kullanici.Sifre != SifreTekrar)
            {
                // Şifreler uyuşmuyor
                TempData["SifreUyusmuyor"] = "Şifreler uyuşmuyor.";
                return View("Profil", kullanici); // Profil görüntüleme sayfasına geri döndür
            }

            profil.Ad = kullanici.Ad;
			profil.Soyad = kullanici.Soyad;
			profil.KullaniciAdi = kullanici.KullaniciAdi;
			profil.Eposta = kullanici.Eposta;
			profil.TelNo = kullanici.TelNo;
			profil.Sifre = kullanici.Sifre;

			_context.Kullanicilar.Update(profil);
			await _context.SaveChangesAsync();

			return View("Profil");
		}
	}
}
