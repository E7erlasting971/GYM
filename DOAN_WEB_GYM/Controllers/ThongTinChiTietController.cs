using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN_WEB_GYM.Models;
namespace DOAN_WEB_GYM.Controllers
{
    public class ThongTinChiTietController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: ThongTinChiTiet
        public ActionResult Details()
        {
            var all_KT = from s in db.KhoaTaps select s;
            return View(all_KT);
        }
        public ActionResult ChiTietKhoaTap(int? id)
        {
            MyDataDataContext db = new MyDataDataContext();
            KhoaTap ctKhoaTap = db.KhoaTaps.SingleOrDefault(p => p.idCourse == id);
            if (ctKhoaTap == null)
            {
                return HttpNotFound();
            }
            return View(ctKhoaTap);
        }
    }
}