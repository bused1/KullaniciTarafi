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
            // Veritabanından Portfolyo verilerini al
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
        public async Task<IActionResult> UyeOl(string Email, string Sifre)
        {
            var user = new Kullanicilar
            {
                Email = Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(Sifre)
            };

            _context.Kullanicilars.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Kayıt başarılı");
        }

        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(string Email, string Sifre)
        {
            var user = await _context.Kullanicilars.SingleOrDefaultAsync(u => u.Email == Email);
            if (user != null && BCrypt.Net.BCrypt.Verify(Sifre, user.PasswordHash))
            {
                return Ok("Giriş başarılı");
            }

            return Unauthorized("Geçersiz giriş bilgileri");
        }
  

        public IActionResult Iletisim()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Iletisim(string ad, string soyad, string email, string mesaj)
        {
            try
            {
                if (ad != null && soyad != null && email != null && mesaj != null)
                {
                    var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential("bused890@gmail.com", "Busbus016."),
                        EnableSsl = true,
                        UseDefaultCredentials = false
                    };
                    smtpClient.Timeout = 20000; // 20 saniye

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("bused890@gmail.com"),
                        Subject = "Yeni Mesaj",
                        Body = $"Ad: {ad}\n Soyad: {soyad}\n E-posta: {email}\n Mesaj: {mesaj}",
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add("bused890@gmail.com");
                    smtpClient.Send(mailMessage);
                    ViewBag.Uyari = "Mesajınız başarıyla gönderilmiştir.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Uyari = "Hata oluştu. Lütfen tekrar deneyiniz. Hata: " + ex.Message;
                Console.WriteLine("Hata: " + ex.Message);
            }
            return View("Iletisim");
        }

     

    }
}
