using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KullaniciTarafi.Models;

public partial class Kullanicilar
{

    public int Id { get; set; }

    [Required]
    public string Ad { get; set; }

    [Required]
    public string Soyad { get; set; }

    [Required]
    public string Telefon { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Mesaj { get; set; }
	[Required]
	public string PasswordHash { get; set; } // Şifre hash'i için bir özellik

}
