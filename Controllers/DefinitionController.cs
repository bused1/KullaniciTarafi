﻿using KullaniciTarafi.Models;
using KullaniciTarafi.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace KullaniciTarafi.Controllers
{
	public class DefinitionController : Controller
	{
		private readonly DbKayaContext _context;
		public DefinitionController(DbKayaContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult ReferansList()
		{
			return View();
		}
		public IActionResult ReferanslarGridView()
		{
			var referans = _context.Referanslars.ToList();
			return PartialView("ReferanslarGridView", referans);
		}


		public IActionResult ReferanslarForm(int id)
		{
			Referanslar referanslar = new Referanslar();
			if (id == 0)
			{

				return PartialView("ReferanslarForm", referanslar);
			}
			else
			{
				var tblreferans = _context.Referanslars.FirstOrDefault(referanslar => referanslar.Id == id);
				referanslar.FirmaLogo = tblreferans.FirmaLogo;
				referanslar.FirmaAdi = tblreferans.FirmaAdi;
				referanslar.Id = tblreferans.Id;
				return PartialView("ReferanslarForm", referanslar);
			}
		}


		[HttpPost]
		public IActionResult SaveReferans(ReferanslarDto model)
		{

			byte[] firmaLogoBytes;

			// MemoryStream kullanarak IFormFile'den byte array'e dönüştürme
			using (var memoryStream = new MemoryStream())
			{
				model.FirmaLogo.CopyTo(memoryStream);
				firmaLogoBytes = memoryStream.ToArray();
			}
			if (model.Id == 0)
			{
				Referanslar referanslar = new Referanslar();

				referanslar.FirmaAdi = model.FirmaAdi;
				referanslar.FirmaLogo = Convert.ToBase64String(firmaLogoBytes);   // FirmaLogo'yu byte[] olarak saklıyoruz
				_context.Referanslars.Add(referanslar);

			}
			else
			{
				var existingReferans = _context.Referanslars.FirstOrDefault(r => r.Id == model.Id);
				if (existingReferans != null)
				{
					existingReferans.FirmaAdi = model.FirmaAdi;
					existingReferans.FirmaLogo = Convert.ToBase64String(firmaLogoBytes);
					_context.Referanslars.Update(existingReferans);
				}
			}

			_context.SaveChanges();
			return RedirectToAction("ReferansList");

		}


		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null)
			{
				return BadRequest("Id bulunamadı.");
			}

			var referans = _context.Referanslars.Find(id);
			if (referans == null)
			{
				return NotFound();
			}

			try
			{
				_context.Referanslars.Remove(referans);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{

				return StatusCode(500, "Kayıt silinirken bir hata oluştu.");
			}

			return RedirectToAction("ReferansList");
		}
	}
}
