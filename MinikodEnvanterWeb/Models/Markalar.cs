using System.ComponentModel.DataAnnotations;

namespace MinikodEnvanterWeb.Models
{
	public class Markalar
	{
		[Key]
		public int MarkaID { get; set; }
		public string MarkaAdi { get; set; }
		public string? DigerMarka { get; set; }
		public ICollection<Kategoriler> Kategoriler { get; set; }

	}
}
