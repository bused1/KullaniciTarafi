using KullaniciTarafi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Diagnostics.Eventing.Reader;
using System.Net.Mail;
using System.Net;
using System;
using BCrypt.Net;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Data.Entity;
using System.Web.Helpers;
using System.Web.WebPages.Html;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
			var portfolioItems = _context.Referanslars.ToList();
			return View(portfolioItems);
		}
		public IActionResult Hakkimizda()
		{
			var hakkimizda = _context.Referanslars
											 .Where(r => !string.IsNullOrEmpty(r.Hakkimizda))
											 .ToList();
			return View(hakkimizda);
		}

		[HttpGet]
		public IActionResult UyeOl()
		{
			return View();
		}
		[HttpPost]
		public IActionResult UyeOl(string ad, string soyad, string telefon, string email, string Sifre)
		{
			var existingUser = _context.Kullanicilars.SingleOrDefault(u => u.Email == email);
			if (existingUser != null)
			{
				ViewData["ErrorMessage"] = "Bu e-posta ile kayıtlı bir kullanıcı zaten var.";
				return View();
			}
			string salt = BCrypt.Net.BCrypt.GenerateSalt(12); // 12, salt'ın karmaşıklığını temsil eder
			string passwordHash = BCrypt.Net.BCrypt.HashPassword(Sifre, salt);

			if (telefon.Length != 11 || !telefon.All(char.IsDigit))
			{
				ViewData["ErrorMessage"] = "Telefon numarası 11 basamaklı ve sadece rakamlardan oluşmalı!";
				return View();
			}
			else
			{
				var user = new Kullanicilar
				{
					Ad = ad,
					Soyad = soyad,
					Telefon = telefon,
					Email = email,
					PasswordHash = passwordHash
				};

				_context.Kullanicilars.Add(user);
				_context.SaveChanges();
				ViewData["SuccessMessage"] = "Kullanıcı başarıyla eklendi.";
				return View();
			}
		}

		[HttpGet]
		public IActionResult Giris()
		{
			return View();

		}
		[HttpPost]
		public IActionResult Giris(string email, string Sifre)
		{
			var user = _context.Kullanicilars.SingleOrDefault(u => u.Email == email);
			if (user != null)
			{
				// 2. Kullanıcının girdiği şifreyi doğrula
				bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(Sifre, user.PasswordHash);

				if (isPasswordCorrect)
				{
			        return RedirectToAction("ReferansList", "Definition");
                }
				else
				{
					ViewData["ErrorMessage"] = "Şifre Yanlış!"; 
				}
			}
			else
			{
				ViewData["ErrorMessage"] = "Geçersiz giriş bilgileri";
				return View();
			}

			return View();
		}


		public IActionResult Iletisim()
		{
			return View();
		}


		[HttpPost]
		public IActionResult Iletisim(string ad = null, string soyad = null, string subject = null, string body = null, string email = null, string mesaj = null)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var mailAddress = new MailAddress(email); // E-posta adresini doğrulama

					var smtpClient = new SmtpClient("smtp.gmail.com")
					{
						Port = 587,
						Credentials = new NetworkCredential("bused890@gmail.com", "usyb vhyu izqr ousk\r\n"),
						EnableSsl = true,
					};

					var mailMessage = new MailMessage
					{
						From = mailAddress, // Kullanıcının girdiği geçerli e-posta adresi
						Subject = "test",
						Body = email + "</br>" + mesaj,
						IsBodyHtml = true,
					};
					mailMessage.To.Add("bused890@gmail.com");

					smtpClient.Send(mailMessage);
					ViewBag.Uyari = "Mesajınız başarıyla iletilmiştir.";
				}
				catch (FormatException)
				{
					ViewBag.Uyari = "Geçersiz e-posta adresi. Lütfen geçerli bir e-posta adresi giriniz.";
				}
			}
			else
			{
				ViewBag.Uyari = "Hata oluştu. Lütefen tekrar deneyiniz.";
			}
			return View();
		}
	}
}