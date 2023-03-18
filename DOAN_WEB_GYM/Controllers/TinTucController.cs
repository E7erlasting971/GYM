using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN_WEB_GYM.Models;
namespace DOAN_WEB_GYM.Controllers
{
    public class TinTucController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
       
        // GET: TinTuc
        public ActionResult Blog()
        {
            var loaitintuc = db.TenLoaiTinTucs.SingleOrDefault(a => a.newsTypeName.Equals("Blog"));
            var blog = db.TinTucs.Where(a => a.idNewsType == loaitintuc.idNewsType);

            return View(blog);
        }
        public ActionResult SuKien()
        {
            var loaitintuc = db.TenLoaiTinTucs.SingleOrDefault(a => a.newsTypeName.Equals("Sự Kiện"));
            var tintuc = db.TinTucs.Where(a => a.idNewsType == loaitintuc.idNewsType);
            return View(tintuc);
        }
        public ActionResult KienThucSucKhoe()
        {
            var loaitintuc = db.TenLoaiTinTucs.SingleOrDefault(a => a.newsTypeName.Equals("Kiến Thức và Sức Khỏe"));
            var kienthucvasuckhoe = db.TinTucs.Where(a => a.idNewsType == loaitintuc.idNewsType);
            return View(kienthucvasuckhoe);
        }

        public ActionResult Details_Blog(int? id)
        {
            MyDataDataContext db = new MyDataDataContext();
            TinTuc blogtt = db.TinTucs.SingleOrDefault(p => p.idType == id);
            if (blogtt == null)
            {
                return HttpNotFound();
            }
            return View(blogtt);
        }
        public ActionResult Details_SuKien(int? id)
        {
            MyDataDataContext db = new MyDataDataContext();
            TinTuc SuKientt = db.TinTucs.SingleOrDefault(p => p.idType == id);
            if (SuKientt == null)
            {
                return HttpNotFound();
            }
            return View(SuKientt);
        }
        public ActionResult Details_KienThucSucKhoe(int? id)
        {
            MyDataDataContext db = new MyDataDataContext();
            TinTuc KienThucSucKhoett = db.TinTucs.SingleOrDefault(p => p.idType == id);
            if (KienThucSucKhoett == null)
            {
                return HttpNotFound();
            }
            return View(KienThucSucKhoett);
        }
    }
}