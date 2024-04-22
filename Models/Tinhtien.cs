using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Tinhtien
{
    public int RId { get; set; }

    public int? Userid { get; set; }

    public int? Tongtien { get; set; }

    public int? Tongngay { get; set; }

    public DateOnly? Checkin { get; set; }

    public DateOnly? Checkout { get; set; }

    public string? Tenchinhanh { get; set; }

    public string? Sophong { get; set; }

    public string? Trangthai { get; set; }

    public double? Gia { get; set; }
}
