using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using X.PagedList;
using Azure;
using System.Collections.Generic;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WebApplication1.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class AdminController : Controller
    {
       
        DuctronghotelDatabaseContext db = new DuctronghotelDatabaseContext();
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            return View();

        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(Admin adm)
        {
            var account = db.Admins.SingleOrDefault(x => x.Emailadmin == adm.Emailadmin && x.Passwordadmin == adm.Passwordadmin);
            if (account == null)
            {
                ModelState.AddModelError("Error", "sai thong tin dang nhap");
                return View(adm);

            }
            else
            {
                var claims = new List<Claim>
                 {
                   new Claim(ClaimTypes.Email, adm.Emailadmin),
                   new Claim("Email", account.Emailadmin),
                   new Claim(ClaimTypes.Role, "Admin"),
                  };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                { };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);

                HttpContext.Session.SetString("email", adm.Emailadmin);


                return RedirectToAction("Dashboard","Admin");
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
           
            HttpContext.Session.Clear();
           
            return RedirectToAction("Index");
        }
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Dashboard()
        {
            ViewBag.email = HttpContext.Session.GetString("email");
                    // tong user
                    //tong reservation
                    //tong tien
                    //latest user
                    //latest rserved
            ViewBag.usercount=db.Userrs.Count();
            ViewBag.reservationcount = db.Reserveds.Count();
            ViewBag.totalcash = db.Tinhtiens.Select(x=>x.Tongtien).Sum();
           
           
            ViewBag.paid = db.Tinhtiens.Where(x => x.Trangthai == "Đã thanh toán").Select(p => p.Tongtien).Sum();
            ViewBag.unpaid = db.Tinhtiens.Where(x => x.Trangthai == "Chưa  thanh toán").Select(p => p.Tongtien).Sum();
            var piechart1 = db.Tinhtiens
    .Join(
        db.Userrs, // Assuming the table containing user emails is named Users
        reservation => reservation.Userid,
        user => user.Userid,
        (reservation, user) => new { UserEmail = user.Emailuser, TotalMoneySpent = reservation.Tongtien }
    )
    .GroupBy(result => result.UserEmail)
    .Select(group => new { UserEmail = group.Key, TotalMoneySpent = group.Sum(item => item.TotalMoneySpent) })
    .ToList();

            ViewBag.UserExpenses = piechart1;
            var piechart2 = db.Tinhtiens.GroupBy(res => res.Trangthai).Select(g => new { Trangthai = g.Key, Tongtien = g.Sum(resr => resr.Tongtien) }).ToList();
            ViewBag.Trangthaichart = piechart2;
            return View();
        }
      
        public IActionResult Thanhpho()

        {   
            var x=db.Thanhphos.ToList();
            return View(x); }
        public IActionResult Chinhanh(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lst = db.Khachsans.ToArray();
            PagedList<Khachsan> lstt = new PagedList<Khachsan>(lst, pageNumber, pageSize);
            return View(lstt);
        }
        public IActionResult phongtheokhuvuc(int cid,int? page)
        {
            int pagesize = 9;
            int pagenumber = page == null || page < 0 ? 1 : page.Value;
            var y = db.Phongs.AsNoTracking().Where(x => x.Idksan == cid).OrderBy(x => x.Idksan);
            PagedList<Phong> z = new PagedList<Phong>(y, pagenumber, pagesize);

            return View(z);
        }
        [HttpGet]
        public IActionResult EditHotel(int eid)
        {
            var x = db.Khachsans.SingleOrDefault(x => x.Idkhachsan == eid);
            
            return View(x);
        }
        [HttpPost]
        public async Task<IActionResult> EditHotel(Khachsan ks)
        {
            var x = await db.Khachsans.FindAsync(ks.Idkhachsan);
            if(x is not null)
            {
                x.Idthanhpho = ks.Idthanhpho;
                x.Idkhachsan = ks.Idkhachsan;
                x.Tenchinhanh = ks.Tenchinhanh;
                x.Diachiks = ks.Diachiks;
                x.Img=ks.Img;
                x.Sdtks = ks.Sdtks;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Chinhanh");
        }
        public IActionResult Phong(int? page)
        {
           
            int pageSize = 15;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lst = db.Phongs.ToArray();
            PagedList<Phong> lstt = new PagedList<Phong>(lst, pageNumber, pageSize);
            return View(lstt);
        }
      
        [HttpGet]
        public IActionResult Datphong(int? page)
        {
            int pageSize = 10;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lst = db.Reserveds.ToArray();
            PagedList<Reserved> lstt = new PagedList<Reserved>(lst, pageNumber, pageSize);
            return View(lstt);
        }
        [HttpGet]
        public IActionResult AddDatPhong()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddDatPhong(Reserved rs)
        {
            var xxx = db.Phongs.SingleOrDefault(x => x.Idksan == rs.Idksan && x.Sophong == rs.Thanhtoanchua).Idroom;
            await db.Reserveds.AddRangeAsync(
                new Reserved
                {
                    Checkin = rs.Checkin,
                    Checkout = rs.Checkout,
                    Idksan = rs.Idksan,
                    Idroom = xxx,
                    Iduser = rs.Iduser,
                    Thanhtoanchua = "Chưa thanh toán"

                });

            await db.SaveChangesAsync();
            var caca = db.Reserveds.Select(x => x.Reservedid).Max();
            db.Database.ExecuteSqlRaw("exec tinhtienproc {0}", caca);
            return RedirectToAction("DatPhong");
        }
        [HttpGet]
        public IActionResult EditDatPhong(int eid)
        {   var x=db.Reserveds.SingleOrDefault(x=>x.Reservedid==eid);
            return View(x);
        }
        [HttpPost]
        public async Task<IActionResult> EditDatPhong(Reserved rs)
        {
            _logger.LogInformation("{0}", rs.Thanhtoanchua);
            var x = await db.Reserveds.FindAsync(rs.Reservedid);
            if (x is not null)
            {
                x.Idroom = rs.Idroom;
               x.Checkin = rs.Checkin;
                x.Checkout= rs.Checkout;
                x.Idksan = rs.Idksan;
                x.Thanhtoanchua= rs.Thanhtoanchua;
                x.Iduser = rs.Iduser;

                await db.SaveChangesAsync();
            }
            return RedirectToAction("DatPhong");
        }
        [HttpGet]
        public IActionResult EditRoom(int eid)
        {
            var x = db.Phongs.SingleOrDefault(x => x.Idroom == eid);

            return View(x);
        }
        [HttpPost]
        public async Task<IActionResult> EditRoom(Phong ks)
        {
            var x = await db.Phongs.FindAsync(ks.Idroom);
            if (x is not null)
            {
                x.Idroom = ks.Idroom;
                x.Sophong = ks.Sophong  ;
                x.Trangthai = ks.Trangthai;
                x.Roomtypeid = ks.Roomtypeid;
                x.Idksan = ks.Idksan;
                
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Phong");
        }

        public async Task<IActionResult> XoaDatPhong(int rs)
        {
            
            db.Database.ExecuteSqlRaw("Delete from reserved where reservedid={0}",rs);
            return RedirectToAction("DatPhong");
        }


    }
}

