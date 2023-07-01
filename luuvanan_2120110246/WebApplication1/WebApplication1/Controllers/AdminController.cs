using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using PagedList;
using PagedList.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        dbQLBansachDataContext data = new dbQLBansachDataContext();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
        //

        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.Tenchude), "MaCD", "TenChude");

            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.tenNXB), "MaNXB", "TenNXB");
            return View();


        }

        //
        [HttpPost]
        public ActionResult ThemmoiSach(SACH sach, HttpPostedFileBase fileupload)
        {


            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.Tenchude), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.tenNXB), "MaNXB", "TenNXB");
            if (fileupload == null)

            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {

                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName); //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }



                    sach.Anhbia = fileName;
                    //Luu vào CSDL
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }

        }
        //
        public ActionResult Xoasach(int id)
        {


            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(sach);
        }
        //

        [HttpPost, ActionName("Xoasach")]
        public ActionResult Xacnhanxoa(int id)
        {
        

            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
          
            data.SACHes.DeleteOnSubmit(sach); 
            data.SubmitChanges();
            return RedirectToAction("Sach");
        }
        //


        public ActionResult Chitietsach(int id)
        {
            
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach ==id);
            ViewBag.Masach=sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        //

        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.Tenchude), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.tenNXB), "MaNXB", "TenNXB");
            if (fileupload == null)

            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {

                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName); //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }



                    sach.Anhbia = fileName;
                    //Luu vào CSDL
                    UpdateModel(sach);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }

        }
        //

        [HttpGet]
        public ActionResult Sach(int ?page)
        {
            int pagaNumber = (page ?? 1);
            int pageSize = 7;
            //return View(data.SACHes.ToList());
            return View(data.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pagaNumber,pageSize));
        }

        //
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if(String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";

            }
            else if(String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad =  data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad !=null)
                {

                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index","Admin");


                }
                else 
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng ";



            }


            return View();

        }
	}
}