using System;
using System.Collections.Generic;

namespace KullaniciTarafi.Models;

public partial class Kullanicilar
{
    public int Id { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }
}
