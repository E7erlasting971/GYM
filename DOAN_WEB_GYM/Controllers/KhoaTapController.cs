using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN_WEB_GYM.Models;
namespace DOAN_WEB_GYM.Controllers
{
    public class KhoaTapController : Controller
    {
        MyDataDataContext mydata = new MyDataDataContext();

        public ActionResult LichHoc()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            TaiKhoanKH kh = (TaiKhoanKH)Session["Taikhoan"];
            string[] listThu = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            ViewData["listThu"] = listThu.ToList();

            var KhachHangDKKT = mydata.DangKyKhoaTaps.Where(w => w.idGuest == kh.idGuest && w.status == true ).ToList();
            if (KhachHangDKKT == null)
            {
                ViewData["Notice"] = "không có khóa tập/ đăng ký?";
                return RedirectToAction("CLB", "CauLacBo");
            }
            List<ChiTietDangKyKhoaTap> lstctdkkt = new List<ChiTietDangKyKhoaTap>();
            foreach (var item1 in KhachHangDKKT)
            {
                var lstKhoaTap = mydata.ChiTietDangKyKhoaTaps.Where(w => w.idInvoice == item1.idInvoice).ToList();
                foreach (var item2 in lstKhoaTap)
                    lstctdkkt.Add(item2);
            }

            ViewData["listTime"] = mydata.ThoiGians.ToList();

            ViewData["lstKhoaTap"] = lstctdkkt;
            var lstThuKhoaTap = new List<List<string>>();
            if (lstctdkkt.Count > 0)
            {
                foreach (var item in lstctdkkt)
                {
                    var khoaTap_temp = mydata.KhoaTaps.Where(w => w.idCourse == item.idCourse && w.dueDay >= DateTime.Now).SingleOrDefault();
                    DateTime dt1 = (DateTime)khoaTap_temp.startDay;
                    DateTime dt2 = (DateTime)khoaTap_temp.dueDay;
                    List<string> thuKhoaTap = new List<string>();
                    while (dt2 >= dt1)
                    {
                        if (khoaTap_temp.ThuTrongTuan.dateInWeek.Contains(dt1.DayOfWeek.ToString()))
                        {
                            thuKhoaTap.Add(dt1.DayOfWeek.ToString());
                        }
                        dt1 = dt1.AddDays(1);
                    }
                    lstThuKhoaTap.Add(thuKhoaTap);
                }
            }
            ViewData["lstThuKhoaTap"] = lstThuKhoaTap;
            return View();
        }
    }
}