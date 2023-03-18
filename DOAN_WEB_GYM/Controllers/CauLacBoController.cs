using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN_WEB_GYM.Models;
namespace DOAN_WEB_GYM.Controllers
{
    public class CauLacBoController : Controller
    {
        MyDataDataContext db = new MyDataDataContext ();
        // GET: CauLacBo
        public ActionResult CLB(string TimKiemCLB= "")
        {
            if(TimKiemCLB != "")
            {
                var searchCLB = db.CLBs.Where(n => n.CLBName.ToUpper().Contains(TimKiemCLB.ToUpper()));
                return View(searchCLB.ToList());
            }

         
          
            var all_CLB = from s in db.CLBs select s;
            return View(all_CLB);
        }


        public ActionResult CTCLB(int id)
        {
            var all =  db.KhoaTaps.ToList().Where(o=>o.idCLB==id);
            return View(all);
        }
        // Bo sung
        public ActionResult ChiTietKhoaTap1(int? id)
        {
            MyDataDataContext db = new MyDataDataContext();
            KhoaTap CTKhoaTap = db.KhoaTaps.SingleOrDefault(p => p.idCourse == id && p.dueDay>= DateTime.Now);
            if (CTKhoaTap == null)
            {
                return HttpNotFound();
            }
            return View(CTKhoaTap);
        }
        public ActionResult Details(int id)
        {
            var kt = from s in db.KhoaTaps
                       where s.idCourse == id
                       select s;
            return View(kt.Single());
        }
    }
}