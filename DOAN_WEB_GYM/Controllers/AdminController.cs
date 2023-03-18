using DOAN_WEB_GYM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace DOAN_WEB_GYM.Controllers
{
    public class AdminController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["account"];
            var matkhau = collection["password"];
            var user = db.TaiKhoanQTVs.SingleOrDefault(p => p.Account == tendn);
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
                return this.Login();
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
                return this.Login();
            }
            else if (user == null)
            {
                ViewData["1"] = "Tài Khoản không tồn tại";
                return this.Login();
            }
            else if (!String.Equals(ConvertMD5.MD5Hash(matkhau), user.Password))
            {
                ViewData["2"] = "Sai mật khẩu";
                return this.Login();
            }
            else
            {
                Session["admin"] = user;
                return RedirectToAction("ManHinhAdmin", "Admin");
            }
        }
        public ActionResult LogOut()
        {
            Session["admin"] = null;
            return RedirectToAction("Login", "Admin");
        }
        public ActionResult ManHinhAdmin()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            TaiKhoanKH kh = new TaiKhoanKH();
            int hdDaThanhToan = db.DangKyKhoaTaps.Where(p=>p.status == true).Count();
            var year = db.DangKyKhoaTaps.Where(p => p.status == true);
            int doanhThu =Convert.ToInt32(db.ChiTietDangKyKhoaTaps.Sum(p => p.price));
            int khachMoi = db.DangKyKhoaTaps.Where(p=>p.idDate == DateTime.Now).Count();
            int hdTrongHangCho = db.DangKyKhoaTaps.Where(p => p.status !=true).Count();
            ViewData["hdDaThanhToan"] = hdDaThanhToan;
            ViewData["doanhThu"] = doanhThu;
            ViewData["khachMoi"] = khachMoi;
            ViewData["hdTrongHangCho"] = hdTrongHangCho;
            return View();
        }
        [HttpGet]
        public ActionResult ThemKhoaTap()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.idCLB = new SelectList(db.CLBs.ToList().OrderBy(n => n.CLBName), "idCLB", "CLBName");
            ViewBag.idPT = new SelectList(db.PTs.ToList().OrderBy(n => n.namePT), "idPT", "namePT");
            ViewBag.idDOW = new SelectList(db.ThuTrongTuans.ToList().OrderBy(n => n.idDOW), "idDOW", "dateInWeek");
            ViewBag.idTime = new SelectList(db.ThoiGians.ToList().OrderBy(n => n.idTime), "idTime", "timeStartEnd");
            return View();
        }
        [HttpPost]
        public ActionResult ThemKhoaTap(FormCollection collection, KhoaTap kt ,CLB clb)
        {
            var nameCourse = collection["nameCourse"];
            var startDay =Convert.ToDateTime(collection["startDay"]);
            var dueDay = Convert.ToDateTime(collection["dueDay"]);
            var price = Convert.ToInt32 (collection["price"]);
            var descriptionKT = collection["descriptionKT"];
            var urlImg = collection["urlImg"];
            var idTime = Convert.ToInt32 ( collection["idTime"]);
            var idPT = Convert.ToInt32(collection["idPT"]);
            var idDOW = Convert.ToInt32(collection["idDOW"]);
            if (string.IsNullOrEmpty(nameCourse))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else if (startDay > dueDay)
            {
                return RedirectToAction("ThemKhoaTap", "Admin");
            }
            else
            {
                kt.nameCourse = nameCourse.ToString();
                kt.startDay = startDay;
                kt.dueDay = dueDay;
                kt.price = price;
                kt.descriptionKT = descriptionKT.ToString();
                kt.urlImg = urlImg.ToString();
                kt.idTime = idTime;
                if (idPT > 0)
                    kt.idPT = idPT;
                else
                    kt.idPT = null;
                kt.idDOW = idDOW;
                db.KhoaTaps.InsertOnSubmit(kt);
                db.SubmitChanges();
                return RedirectToAction("ThemKhoaTap", "Admin");
            }
            return this.ThemKhoaTap();
        }
        public ActionResult SuaKT(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.idCLB = new SelectList(db.CLBs.ToList().OrderBy(n => n.CLBName), "idCLB", "CLBName");
            ViewBag.idPT = new SelectList(db.PTs.ToList().OrderBy(n => n.namePT), "idPT", "namePT");
            ViewBag.idDOW = new SelectList(db.ThuTrongTuans.ToList().OrderBy(n => n.idDOW), "idDOW", "dateInWeek");
            ViewBag.idTime = new SelectList(db.ThoiGians.ToList().OrderBy(n => n.idTime), "idTime", "timeStartEnd");
            var E_KT = db.KhoaTaps.First(m => m.idCourse == id);
            return View(E_KT);
        }
        [HttpPost]
        public ActionResult SuaKT(int id, FormCollection collection)
        {
            var E_KT = db.KhoaTaps.First(m => m.idCourse == id);
            var nameCourse = collection["nameCourse"];
            var startDay = Convert.ToDateTime( collection["startDay"]);
            var dueDay = Convert.ToDateTime(collection["dueDay"]);
            var price = Convert.ToInt32(collection["price"]);
            var descriptionKT = collection["descriptionKT"];
            var idPT = Convert.ToInt32(collection["PT"]);

            var urlImg = collection["urlImg"];
         /*   var idCLB = Convert.ToInt32(collection["idCLB"]);
            var idPT = Convert.ToInt32(collection["idPT"]);*/

            E_KT.idCLB = id;
            if (string.IsNullOrEmpty(nameCourse))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_KT.nameCourse = nameCourse;
                E_KT.startDay = startDay;
                E_KT.dueDay = dueDay;
                E_KT.price = price;
                E_KT.descriptionKT = descriptionKT;
                E_KT.urlImg = urlImg;
                if (idPT > 0)
                    E_KT.idPT = idPT;
                else
                    E_KT.idPT = null;

                E_KT.urlImg = urlImg;
            /*    E_KT.idCLB = idCLB;
                E_KT.idPT = idPT;*/
                UpdateModel(E_KT);
                db.SubmitChanges();
                return RedirectToAction("DanhSachKhoaTap");
            }
            return this.SuaKT(id);
        }
        [HttpGet]
        public ActionResult ThemTinTuc()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            ViewBag.idNewsType = new SelectList(db.TenLoaiTinTucs.ToList().OrderBy(n => n.newsTypeName), "idNewsType", "newsTypeName");
            return View();
        }
        /* [HttpPost]*/
        [HttpPost, ValidateInput(false)]
        public ActionResult ThemTinTuc(FormCollection collection, TinTuc tt)
        {
            var Title = collection["title"];
            var description = collection["description"];
            var urlImg = collection["urlImg"];
         /*   var newsTypeId = Convert.ToInt32(collection["newsTypeId"]);*/
            if (string.IsNullOrEmpty(Title))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                tt.Title = Title.ToString();
                tt.description = description.ToString();
                tt.urlImg = urlImg.ToString();
               /* tt.newsTypeId = newsTypeId;*/
                db.TinTucs.InsertOnSubmit(tt);
                db.SubmitChanges();
                return RedirectToAction("ThemTinTuc", "Admin");
            }
            return this.ThemTinTuc();
        }
        public ActionResult Sua_TinTuc(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var E_TinTuc = db.TinTucs.First(m => m.idType == id);
            return View(E_TinTuc);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Sua_TinTuc(int id, FormCollection collection)
        {
            var E_clb = db.TinTucs.First(m => m.idType == id);
            var Title = collection["Title"];
            var description = collection["description"];
            var urlImg = collection["urlImg"];
            E_clb.idType = id;
            if (string.IsNullOrEmpty(Title))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_clb.Title = Title;
                E_clb.description = description;
                E_clb.urlImg = urlImg;
    
                UpdateModel(E_clb);
                db.SubmitChanges();
                return RedirectToAction("DanhSachTinTuc");
            }
            return this.Sua_TinTuc(id);
        }

        public ActionResult Details_TinTuc(int? id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            MyDataDataContext db = new MyDataDataContext();
            TinTuc tintuc = db.TinTucs.SingleOrDefault(p => p.idType == id);
            if (tintuc == null)
            {
                return HttpNotFound();
            }
            return View(tintuc);
        }
         public ActionResult Xoa_TinTuc(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var D_tintuc = db.TinTucs.First(m => m.idType == id);
            return View(D_tintuc);
        }
        [HttpPost]
        public ActionResult Xoa_TinTuc(int id, FormCollection collection)
        {
            var D_tintuc = db.TinTucs.Where(m => m.idType == id).First();
            db.TinTucs.DeleteOnSubmit(D_tintuc);
            db.SubmitChanges();
            return RedirectToAction("DanhSachCLB");
        }
        public ActionResult DanhSachKhoaTap()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_DSktap = from ss in db.KhoaTaps select ss;
            return View(all_DSktap);
        }
        public ActionResult DanhSachHocVien()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_hv = from ss in db.TaiKhoanKHs select ss;
            return View(all_hv);
        }
      
        // thêm clb
        public ActionResult ThemCLB()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ThemCLB(FormCollection collection, CLB s)
        {
            var E_tenCLB = collection["CLBName"];
            var E_addressCLB = collection["addressCLB"];
            var E_urlImage = collection["urlImg"];
            var E_phoneNumber = collection["phoneNumber"];
            if (string.IsNullOrEmpty(E_tenCLB))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.CLBName = E_tenCLB.ToString();
                s.addressCLB = E_addressCLB.ToString();
                s.urlImg = E_urlImage.ToString();
                s.phoneNumber = E_phoneNumber.ToString();
                db.CLBs.InsertOnSubmit(s);
                db.SubmitChanges();
                return RedirectToAction("ThemCLB", "Admin");
            }
            return this.ThemCLB();
        }
        // end them clb
        
       
        public ActionResult DetailsCLB(int? id)
        {
            MyDataDataContext db = new MyDataDataContext();
            CLB clb = db.CLBs.SingleOrDefault(p => p.idCLB == id);
            if (clb == null)
            {
                return HttpNotFound();
            }
            return View(clb);
        }
        // EDit CLB
        public ActionResult SuaCLB(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var E_sach = db.CLBs.First(m => m.idCLB == id);
            return View(E_sach);
        }
        [HttpPost]
        public ActionResult SuaCLB(int id, FormCollection collection)
        {
            var E_clb = db.CLBs.First(m => m.idCLB == id);
            var E_tenCLB = collection["CLBName"];
            var E_addressCLB = collection["addressCLB"];
            var E_urlImage = collection["urlImg"];
            var E_phoneNumber = collection["phoneNumber"];
            E_clb.idCLB = id;
            if (string.IsNullOrEmpty(E_tenCLB))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_clb.CLBName = E_tenCLB;
                E_clb.addressCLB = E_addressCLB;
                E_clb.urlImg = E_urlImage;
                E_clb.phoneNumber = E_phoneNumber;
                UpdateModel(E_clb);
                db.SubmitChanges();
                return RedirectToAction("DanhSachCLB");
            }
            return this.SuaCLB(id);
        }
        //end Edit CLB
        // delete clb
        public ActionResult XoaCLB(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var D_clb = db.CLBs.First(m => m.idCLB == id);
            return View(D_clb);
        }
        [HttpPost]
        public ActionResult XoaCLB(int id, FormCollection collection)
        {
            var D_clb = db.CLBs.Where(m => m.idCLB == id).First();
            db.CLBs.DeleteOnSubmit(D_clb);
            db.SubmitChanges();
            return RedirectToAction("DanhSachCLB");
        }
        //end delete clb
        // list CLB
        public ActionResult DanhSachCLB()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_clb = from ss in db.CLBs select ss;
            return View(all_clb);
        }
        //end list CLB
        //them PT
        public ActionResult ThemPT()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult ThemPT(FormCollection collection, PT s)
        {
            var E_tenPT = collection["namePT"];
            var E_phoneNumber = collection["phoneNumber"];
            var E_urlImage = collection["urlImage"];
            var E_gmail = collection["gmail"];
            if (string.IsNullOrEmpty(E_tenPT))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.namePT = E_tenPT.ToString();
                s.phoneNumber = E_phoneNumber.ToString();
                //s.urlImage = E_urlImage.ToString();
                s.gmail = E_gmail.ToString();
                db.PTs.InsertOnSubmit(s);
                db.SubmitChanges();
                return RedirectToAction("ListPT", "Admin");
            }
            return this.ThemPT();
        }
        //end ThemPT
        // xoa PT
        public ActionResult XoaPT(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var D_pt = db.PTs.First(m => m.idPT == id);
            return View(D_pt);
        }
        [HttpPost]
        public ActionResult XoaPT(int id, FormCollection collection)
        {
            var D_pt = db.PTs.Where(m => m.idPT == id).First();
            db.PTs.DeleteOnSubmit(D_pt);
            db.SubmitChanges();
            return RedirectToAction("ListPT");
        }
        // end xoa PT
        public ActionResult Details_PT(int? id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            MyDataDataContext db = new MyDataDataContext();
            PT pt = db.PTs.SingleOrDefault(p => p.idPT == id);
            if (pt == null)
            {
                return HttpNotFound();
            }
            return View(pt);
        }
        //Sua PT
        public ActionResult SuaPT(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var E_PT = db.PTs.First(m => m.idPT == id);
            return View(E_PT);
        }
        [HttpPost]
        public ActionResult SuaPT(int id, FormCollection collection)
        {
            var E_pt = db.PTs.First(m => m.idPT == id);
            var E_tenPT = collection["namePT"];
            var E_phoneNumber = collection["phoneNumber"];
            var E_urlImage = collection["urlImage"];
            var E_gmail = collection["gmail"];
            E_pt.idPT = id;
            if (string.IsNullOrEmpty(E_tenPT))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_pt.namePT = E_tenPT;
                E_pt.phoneNumber = E_phoneNumber;
                E_pt.urlImage = E_urlImage.ToString();
                E_pt.gmail = E_gmail.ToString();
                UpdateModel(E_pt);
                db.SubmitChanges();
                return RedirectToAction("ListPT", "Admin");
            }
            return this.SuaPT(id);
        }
        //end Sua PT
        // ListPT

        private List<PT> LayPTmoi(int count )
        {
            return db.PTs.OrderByDescending(p => p.gmail).Take(count).ToList();
        }
        public ActionResult ListPT(int ? page)
        {
            /* var all_pt = from ss in db.PTs select ss;
             return View(all_pt);*/
            int pageSize = 5;
            int pageNum =(page ?? 1);
            //
            var PTmoi = LayPTmoi(15);
            return View(PTmoi.ToPagedList(pageNum, pageSize));

        }
        // end ListPT
        // danh sách hoa don 
        public ActionResult DanhSachTinTuc()
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_Tintuc = from ss in db.TinTucs select ss;
            return View(all_Tintuc);
        }
        public ActionResult DanhSachHoaDon()
        {
            /*ViewBag.idGuest = new SelectList(db.TaiKhoanKHs.ToList().OrderBy(n => n.fullName), "idGuest", "fullName");*/
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var all_DSHD = from ss in db.DangKyKhoaTaps select ss;
            return View(all_DSHD);
        }
        //// end danh sach hoa don
        //// chit tiet hoa don 
        public ActionResult ThongTinDanhSachHoaDon(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var all = db.ChiTietDangKyKhoaTaps.ToList().Where(o => o.idInvoice == id);
            return View(all);
          
        }
        //// end chit tiet hoa don 
        [HttpGet]
        public ActionResult SuaDanhSachHoaDon(int id)
        {
            if (Session["admin"] == null || Session["admin"].ToString() == "")
            {
                return RedirectToAction("Login", "Admin");
            }
            var ctdkkt = db.ChiTietDangKyKhoaTaps.Where(o => o.idInvoice == id).FirstOrDefault();
            var E_HD = db.DangKyKhoaTaps.SingleOrDefault(m => m.idInvoice == id);
            ViewData["UpdateHoaDon"] = E_HD;
            var ttctdkkt = db.ChiTietDangKyKhoaTaps.Where(o => o.idInvoice == id).ToList();
            ViewData["ttctdkkt"] = ttctdkkt;
            return View(E_HD);

        }
        // end chit tiet hoa don 
        // cap nhat danh sach hoa don
        [HttpPost]
        public ActionResult SuaDanhSachHoaDon(int id, FormCollection collection)
        {
            ViewData["ttdshd"] = db.ChiTietDangKyKhoaTaps.ToList().Where(o => o.idInvoice == id);
            var E_dkkt = db.DangKyKhoaTaps.First(m => m.idInvoice == id);
            var E_status = Convert.ToString(collection["status"]);
            E_dkkt.idInvoice = id;

            UpdateModel(E_dkkt);
            db.SubmitChanges();
            return RedirectToAction("DanhSachHoaDon", "Admin");
        }

        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }

            file.SaveAs(Server.MapPath("~/Content/Images/" + file.FileName));

            return "/Content/Images/" + file.FileName;
        }
    }
}