//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MinikodEnvanterWeb.Models;

//namespace MinikodEnvanterWeb.Controllers
//{
//    [Authorize]
//	public class MarkaController : Controller
//	{
//		AppDbContext _context;
//		public MarkaController(AppDbContext context)
//		{
//			_context = context;	
//		}
//		[HttpGet]
//		public IActionResult MarkaEkle()
//		{
//			return View();
//		}
//		[HttpPost]
//		public async Task<IActionResult> MarkaEkle(Markalar marka)
//		{
//			try
//			{
//				await _context.Markalar.AddAsync(marka);
//				await _context.SaveChangesAsync();
//				return RedirectToAction("MarkaEkle");
//			}
//			catch
//			{
//				return View(marka);
//			}
//		}
//	}
//}
