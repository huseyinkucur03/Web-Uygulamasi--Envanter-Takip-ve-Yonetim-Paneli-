using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinikodEnvanterWeb.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MinikodEnvanterWeb.Controllers
{
	public class GirisController : Controller
	{
		private AppDbContext _context;

		public GirisController(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Login()
		{
			var kullanicilarVerisiVarMi = await _context.Kullanicilar.AnyAsync();

			if (!kullanicilarVerisiVarMi)
			{
				var yeniKullanici = new Kullanicilar
				{
					Ad = "Naim",
					Soyad = "Karasekreter",
					KullaniciAdi = "boss",
					Sifre = "123456",
					Eposta = "boss@hotmail.com",
					TelNo = "05464157896",
					Adres = ""
				};

				_context.Kullanicilar.Add(yeniKullanici);
				await _context.SaveChangesAsync();

				return View(yeniKullanici);
			}


			var model = new Kullanicilar();
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Login(Kullanicilar k)
		{
			var bilgiler = await _context.Kullanicilar
				.FirstOrDefaultAsync(x => x.KullaniciAdi == k.KullaniciAdi && x.Sifre == k.Sifre);

			if (bilgiler != null)
			{
				var claims = new List<Claim>
				{
					new Claim("KullaniciAdi", bilgiler.KullaniciAdi), // Kullanıcı adını Claims'e ekleyin
					new Claim("Ad", bilgiler.Ad),
					new Claim("Soyad", bilgiler.Soyad),
					new Claim("Adres",bilgiler.Adres)
				};

				var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
					new ClaimsPrincipal(claimsIdentity));

				var girisHareketleri = new GirisHareketleri();
				girisHareketleri.KullaniciAdi = bilgiler.KullaniciAdi;
				girisHareketleri.GirisTarihi = DateTime.Now;
				girisHareketleri.Adres = bilgiler.Adres;

				_context.GirisHareketleri.Add(girisHareketleri);
				await _context.SaveChangesAsync();

				return RedirectToAction("Index", "Urun");
			}
			else
			{
				ViewBag.hata = "Kullanıcı adı veya şifre hatalı.";
			}
			return View();
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Giris");
		}
		public async Task<IActionResult> Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(Kullanicilar veri,string SifreTekrar)
		{
			var mevcutKullaniciAdi = await _context.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciAdi == veri.KullaniciAdi);
			if (mevcutKullaniciAdi != null)
			{
				TempData["MevcutKullaniciHatasi"] = "Bu kullanıcı adı zaten mevcut.";
				return View(veri);
			}
			var mevcutEposta = await _context.Kullanicilar.FirstOrDefaultAsync(e => e.Eposta == veri.Eposta);
			if (mevcutEposta != null)
			{
				TempData["MevcutEpostaHatasi"] = "Bu E-Posta adresi zaten kullanılıyor.";
				return View(veri);
			}
			if(veri.Sifre != SifreTekrar)
			{
				TempData["SifreUyusmuyor"] = "Şifreler uyuşmuyor.";
				return View(veri);
			}

			_context.Kullanicilar.Add(veri);
			await _context.SaveChangesAsync();

			TempData["Kayit"] = "Kullanıcı başarıyla kaydedildi";
			return RedirectToAction("Kullanicilar", "Kullanici");
		}
		public async Task<IActionResult> GirisHareketleri()
		{
			var girisHareketi = await _context.GirisHareketleri.ToListAsync();
			return View(girisHareketi);
		}
	}
}