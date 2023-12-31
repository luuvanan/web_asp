﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

using PagedList;
using PagedList.Mvc;
namespace WebApplication1.Controllers
{
    public class BookStoreController : Controller
    {


        dbQLBansachDataContext data = new dbQLBansachDataContext();


        private List<SACH>  Laysachmoi(int count)
        {

            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();

        }
        public ActionResult chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult nhaxuatban()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);
        }
        public ActionResult SPTheochude(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes where s.Masach == id select s;
            return View(sach.Single());
        }
        //
        // GET: /BookStore/
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var sachmoi = Laysachmoi(15);


            return View(sachmoi.ToPagedList(pageNum,pageSize));
        }
	}
}