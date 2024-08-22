using KullaniciTarafi.Models;
using KullaniciTarafi.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace KullaniciTarafi.Controllers
{
	public class UserController : Controller
	{
		private readonly DbKayaContext _context;
		public UserController(DbKayaContext context)
		{
			_context = context;
		}


		
		public IActionResult Portfolyo()
		{

			var portfolioItems = _context.Referanslars.ToList(); // Veritabanından Portfolyo verilerini al
			return View(portfolioItems);

		
		}



	}
}
