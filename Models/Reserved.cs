using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Reserved
{
    public int Reservedid { get; set; }

    public DateOnly? Checkin { get; set; }

    public DateOnly? Checkout { get; set; }

    public int? Idksan { get; set; }

    public int? Idroom { get; set; }

    public int? Iduser { get; set; }

    public string? Thanhtoanchua { get; set; }
}
