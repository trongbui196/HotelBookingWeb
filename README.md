[Route("xoasanpham")]
        [HttpPost]
        public IActionResult xoasp(string masp)
        {
            db.Database.ExecuteSqlRaw("delete from Sanpham where Masp = '" + masp + "'");
            return RedirectToAction("Quanlysanpham");
        }
        [Route("Login")]
        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View("~/Areas/Admin/Views/Login/AdminLogin.cshtml");
        }
        [Route("Login")]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {

            var account = db.Nguoidungs.SingleOrDefault(x => x.Tendangnhap == username && x.Matkhau == password);

            if (account != null)
            {
                if (account.Loainguoidung == "Admin")
                {

                    return RedirectToAction("Admin");
                }
                else if (account.Loainguoidung == "Khách hàng")
                {
                    System.Console.WriteLine(1);
                    ModelState.AddModelError("", "khong phai admin");
                }

            }
            else
            {
                System.Console.WriteLine(2);
                ModelState.AddModelError("", "sai thong tin");
            }

            return View("~/Areas/Admin/Views/Login/AdminLogin.cshtml");
        }
///////////////////////
quanlysanpham.cshtml 

<td style="text-align: center">
                        <a class="btn btn-sm btn-edit" asp-action="Suasanpham" asp-route-masp="@item.Masp">Sửa</a>
                        <form asp-action="xoasp" asp-route-masp="@item.Masp" method="post">
                            <button type="submit" class="btn btn-sm btn-delete">Xóa</button>
                        </form>
                    </td>

adminlogin.cshtml

@if (!ViewData.ModelState.IsValid)
{
    <div class="alert alert-danger">
        @foreach (var error in ViewData.ModelState.Values.SelectMany(v => v.Errors))
        {
            <p>@error.ErrorMessage</p>
        }
    </div>
}
<div class="container">
    <h2>Login</h2>
    <form asp-action="Login" method="post">
        <div class="form-group">
            <label asp-for="username"></label>
            <input name="username" asp-for="username" class="form-control" />
            <span asp-validation-for="Username" class="text-danger"></span>
        </div>
        <div class="form-group">
            <label asp-for="Password"></label>
            <input name="password" asp-for="Matkhau" type="password" class="form-control" />
            <span asp-validation-for="Password" class="text-danger"></span>
        </div>
        <button type="submit" class="btn btn-primary">Login</button>
    </form>
</div>
<script src="~/js/site.js"></script>
