using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using JetBrains.Annotations;



namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public static int idcheck = 0;
        DuctronghotelDatabaseContext db = new DuctronghotelDatabaseContext();
        private readonly ILogger<HomeController> _logger;
        private readonly IVnPayService _vpnPayService;
        public HomeController(ILogger<HomeController> logger, IVnPayService vpnPayService)
        {
            _logger = logger;
            _vpnPayService = vpnPayService;
        }

        public IActionResult Index()
        {

            return View();
        }
        [Authorize(Policy = "CustomerOnly")]

        public IActionResult PaymentPbReturn()
        {
            var response = _vpnPayService.PaymentExecute(Request.Query);
            if(response==null||response.VnPayResponseCode!="00")
            {
                return RedirectToAction("PaymentFail");
            }
            
            var x = db.Tinhtiens.Find(idcheck);
            if(x is not null)
            {   
                
                x.Trangthai = "Đã thanh toán";
                db.SaveChanges();
            }
            var xx = db.Reserveds.Find(idcheck);
            if(x is not null)
            {
                xx.Thanhtoanchua = "Đã thanh toán";
                db.SaveChanges();
            }
            
            return RedirectToAction("PaymentSuccess");
           
        }
        [Authorize(Policy = "CustomerOnly")]

        public IActionResult PaymentFail()
        {
            return View();
        }
        [Authorize(Policy = "CustomerOnly")]

        public IActionResult PaymentSuccess()
        {
            return View();
        }
        [Authorize(Policy = "CustomerOnly")]

        public IActionResult thanhtoanvnpay(int id)
        {
            idcheck = id;
            var x = db.Tinhtiens.SingleOrDefault(x => x.RId == id);
            var vnpaymodel = new VnPaymentRequestModel
            {
                ammount = x.Tongtien,
                CreatedDate = DateTime.Now.ToString(),
                Description=$"THANH TOÁN {x.RId}- phong {x.Sophong} - chi nhanh {x.Tenchinhanh}",
                Name="khach hang",
                OrderID=x.RId
            };
            return Redirect(_vpnPayService.CreatePaymentUrl(HttpContext,vnpaymodel)); }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Rooms()
        {
            return View();
        }
        public IActionResult RoomDetails(int rID)
        {   // tao table chua thong tin phong
            
            var x = db.Loaiphongs.SingleOrDefault(x => x.Idtype == rID);
            return View(x);
        }
        public IActionResult Restaurant()
        {
            return View();
        }
        public IActionResult Cities()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Check()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Check(int RId)
        {   
            idcheck = RId;
            ViewBag.id = RId;
            
            var x = db.Tinhtiens.SingleOrDefault(x => x.RId ==RId);
          
            return View(x);
        }
        [Authorize(Policy = "CustomerOnly")]
        [HttpGet]
        public IActionResult Reserved()
        {
            
            return View();
        }
        [Authorize(Policy = "CustomerOnly")]
        [HttpPost]
        public async Task<IActionResult> Reserved(Reserved rs)
        {
            var iduser = db.Userrs.FirstOrDefault(x => x.Emailuser == HttpContext.Session.GetString("name"));
            var xxx = db.Phongs.SingleOrDefault(x => x.Idksan == rs.Idksan && x.Sophong == rs.Thanhtoanchua).Idroom;

            await db.Reserveds.AddRangeAsync(
                new Reserved
                {
                    Checkin = rs.Checkin,
                    Checkout = rs.Checkout,
                    Idksan = rs.Idksan,
                    Idroom = xxx,
                    Iduser = iduser.Userid,
                    Thanhtoanchua = "Chưa thanh toán"

                }); ;

            await db.SaveChangesAsync();
            var caca = db.Reserveds.Select(x => x.Reservedid).Max();
            db.Database.ExecuteSqlRaw("exec tinhtienproc {0}", caca);
            _logger.LogInformation("da dat phong thanh cong");

            return RedirectToAction("Check", "Home");
            //return View();


        }
      
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Userr ngdung, string? ReturnUrl)
        {

            ViewBag.ReturnUrl = ReturnUrl;
            var account = db.Userrs.SingleOrDefault(x => x.Emailuser == ngdung.Emailuser && x.Userpassword == ngdung.Userpassword);
            if (account == null)
            {

                return RedirectToAction("Register");
            }
            else
            {
                var claims = new List<Claim>
                 {
                   new Claim(ClaimTypes.Email, ngdung.Emailuser),
                   new Claim("FullName", account.Tenuser),
                   new Claim(ClaimTypes.Role, "Customer"),
                  };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                { };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity), authProperties);
                _logger.LogInformation("Tai khoan {0} tai luc {1}", account.Tenuser, DateTime.UtcNow);
                HttpContext.Session.SetString("name", ngdung.Emailuser);
             

                if (HttpContext.Session.GetString("name") != null)
                {
                    _logger.LogInformation("da co session");
                    
                }

                if (Url.IsLocalUrl(ReturnUrl))
                {
                    
                    return RedirectToAction("Reserved");
                }
                else
                {
                    return RedirectToAction("Reserved");
                }



            }

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Userr ngdung)
        {
            await db.Userrs.AddRangeAsync(
                 new Userr
                 {
                     Tenuser = ngdung.Tenuser,
                     Emailuser = ngdung.Emailuser,
                     Sdtuser = ngdung.Sdtuser,
                     Userpassword = ngdung.Userpassword

                 });
            await db.SaveChangesAsync();

            return RedirectToAction("Login");
        }
        [Authorize(Policy = "CustomerOnly")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            var x = HttpContext.Session.GetString("name");
            HttpContext.Session.Clear();
            _logger.LogInformation("{0} da dang xuat", x);
            return RedirectToAction("Index");
        }
        [Authorize(Policy = "CustomerOnly")]

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [Authorize(Policy = "CustomerOnly")]

        [HttpPost]
        public async Task<IActionResult> ResetPassword(Userr uss)
        {   
            int a= db.Userrs.Where(p =>p.Emailuser==uss.Emailuser).FirstOrDefault().Userid;
            var x = await db.Userrs.FindAsync(a);
            if(x is null)
            {
                _logger.LogInformation("Email không tồn tại trong hệ thống");
            }
            else if (x is not null)
            {
                x.Userpassword = "123456";
                await db.SaveChangesAsync();
                TempData["message"]="Mật khẩu mới của bạn là 123456! Hãy đổi mật khẩu ngay nhé!";
            }
            return RedirectToAction("Resetpassdone");
        }
        [Authorize(Policy = "CustomerOnly")]

        public IActionResult Resetpassdone()
        {
            ViewBag.message = TempData["message"].ToString();
            return View();
        }
        [Authorize(Policy = "CustomerOnly")]

        public IActionResult Profile()
        {   var x=HttpContext.Session.GetString("name");
            var xx = db.Userrs.FirstOrDefault(y=>y.Emailuser==x);
            
            return View(xx);
        }
        [Authorize(Policy = "CustomerOnly")]

        public IActionResult Checklichsu(int eid) {
            var x = db.Tinhtiens.Where(x => x.Userid == eid);
            
            return View(x);
        }
        [HttpGet]
        [Authorize(Policy = "CustomerOnly")]

        public IActionResult ChangePassword(int eid)
        {
            var x = db.Userrs.FirstOrDefault(x => x.Userid == eid);
            return View(x);
        }
        [HttpPost]
        [Authorize(Policy = "CustomerOnly")]

        public async Task<IActionResult> ChangePassword(Userr acc)
        {
            var x = await db.Userrs.FindAsync(acc.Userid);
            if(x.Userpassword==acc.Userpassword)
            {
                x.Userpassword = acc.Emailuser;
                TempData["mess"] = "Đã đổi mật khẩu thành công";
            await db.SaveChangesAsync();
            }
            else
            {
                TempData["mess"] = "Đổi mật khẩu không thành công";
            }
           
            return RedirectToAction("Profile");
        }
    }
}
