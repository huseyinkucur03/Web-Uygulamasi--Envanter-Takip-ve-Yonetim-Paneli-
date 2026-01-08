using ClosedXML.Excel;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinikodEnvanterWeb.Models;
using OfficeOpenXml;

namespace MinikodEnvanterWeb.Controllers
{
	[Authorize]
	public class RaporlamaController : Controller
	{
		AppDbContext _context;

		public RaporlamaController(AppDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public async Task<IActionResult> ExcelRapor()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> ExcelRapor(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				TempData["HataMesaji"] = "Lütfen dosya adı giriniz.";
				return RedirectToAction("ExcelRapor");
			}

			fileName = $"{fileName}.xlsx";

			var urunler = await _context.Urunler
				.Include(u => u.Kategoriler)
				.Include(u => u.Markalar)
				.ToListAsync();

			using (var excel = new ExcelPackage())
			{
				var workSheet = excel.Workbook.Worksheets.Add("Ürünler");

				workSheet.Cells[1, 1].Value = "Urun ID";
				workSheet.Cells[1, 2].Value = "Ürün Adı";
				workSheet.Cells[1, 3].Value = "Ürün Kategorisi";
				workSheet.Cells[1, 4].Value = "Ürün Markası";
				workSheet.Cells[1, 5].Value = "Özellikler";
				workSheet.Cells[1, 6].Value = "Toplam Ürün Sayısı";
				workSheet.Cells[1, 7].Value = "Bozuk Ürün Sayısı";
				workSheet.Cells[1, 8].Value = "Çalışmama Nedeni";
				workSheet.Cells[1, 9].Value = "Eklenme Tarihi";
				workSheet.Cells[1, 10].Value = "Son Değiştirilme Tarihi";

				int satir = 2;
				foreach (var urun in urunler)
				{
					workSheet.Cells[satir, 1].Value = urun.UrunID;
					workSheet.Cells[satir, 2].Value = urun.UrunAdi;
					workSheet.Cells[satir, 3].Value = urun.Kategoriler?.KategoriAdi ?? "Kategori yok";
					workSheet.Cells[satir, 4].Value = urun.Markalar?.MarkaAdi ?? "Marka yok";
					workSheet.Cells[satir, 5].Value = urun.Aciklama;
					workSheet.Cells[satir, 6].Value = urun.ToplamSayi;
					workSheet.Cells[satir, 7].Value = urun.CalismayanSayisi;
					workSheet.Cells[satir, 8].Value = urun.CalismamaNedeni;
					workSheet.Cells[satir, 9].Value = urun.EklenmeTarihi.ToString();
					workSheet.Cells[satir, 10].Value = urun.SonDegistirilmeTarihi.ToString();
					satir++;
				}

				var stream = new MemoryStream();
				excel.SaveAs(stream);
				stream.Position = 0;

				return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
			}
		}
	}
}
