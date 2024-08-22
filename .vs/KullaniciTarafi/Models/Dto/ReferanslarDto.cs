using System;
using System.Collections.Generic;

namespace KayaHukukAdmin.Models.Dto;

public partial class ReferanslarDto
{
    public int Id { get; set; }

    public string? FirmaAdi { get; set; }
   
    public IFormFile? FirmaLogo { get; set; }
}
