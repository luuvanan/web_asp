using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Data;

namespace WebApplication1.Controllers
{
    public class NguoidungController : Controller
    {
        dbQLBansachDataContext db = new dbQLBansachDataContext();
        // GET: /Nguoidung/
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tennd = collection["TenDN"];
            var matkhau = collection["dangnhap"];
            if (String.IsNullOrEmpty(tennd))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập Password";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tennd && n.Matkhau == matkhau);
                if (kh != null)
                {
                    ViewBag.Thongbao = "Đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                }
                else

                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";

            }
            return View();
        }
        [HttpPost]
        // POST: Hàm Dangky(post) Nhận dữ liệu từ trang Dangky và thực hiện việc tạo mới dù Liệu



        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenkH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var email = collection["Email"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {

                ViewData["Loi1"] = "Họ tên khách hàng không được để trắng";

            }
            else if (String.IsNullOrEmpty(tendn))
            {

                ViewData["Loi2"] = "Phải nhập tên đăng nhập ViewData";

            }
            else if (String.IsNullOrEmpty(matkhau))
            {

                ViewData["Loi3"] = "phải nhập mật khẩu";

            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {

                ViewData["Loi4"] = "Phải nhập lại mật khẩu";

            }
            if (String.IsNullOrEmpty(email))
            {

                ViewData["Loi5"] = "Email không được bỏ trắng";

            }
            if (String.IsNullOrEmpty(diachi))
            {

                ViewData["Loi6"] = "Phải nhập địa chỉ";

            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "phải nhập điện thoại";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
    }
}