//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MinikodEnvanterWeb.Models;

//namespace MinikodEnvanterWeb.Controllers
//{
//    [Authorize]
//	public class KategoriController : Controller
//	{
//		AppDbContext _context;
//		public KategoriController(AppDbContext context)
//		{
//			_context = context;
//		}
//		[HttpGet]
//		public IActionResult KategoriEkle()
//		{
//			return View();
//		}
//		[HttpPost]
//		public async Task<IActionResult> KategoriEkle(Kategoriler kategori)
//		{
//			try
//			{
//				await _context.Kategoriler.AddAsync(kategori);
//				await _context.SaveChangesAsync();
//				return RedirectToAction("KategoriEkle");
//			}
//			catch
//			{
//				return View(kategori);
//			}
//		}
//	}
//}