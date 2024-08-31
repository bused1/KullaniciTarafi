using System;
using System.Collections.Generic;

namespace KullaniciTarafi.Models;

public partial class Referanslar
{
    public int Id { get; set; }

    public string? FirmaAdi { get; set; }

    public string? FirmaLogo { get; set; }

    public string? Hakkimizda { get; set; }

    public string? Vizyon { get; set; }

    public string? Misyon { get; set; }
}
