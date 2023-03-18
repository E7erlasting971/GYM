using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN_WEB_GYM.Models;
namespace DOAN_WEB_GYM.Controllers
{
    public class TaiKhoanController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();
        // GET: TaiKhoan
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var dangnhap1 = collection["accountKH"];
            var matkhau1 = collection["passwordKH"];
            var user1 = db.TaiKhoanKHs.SingleOrDefault(n => n.accountKH == dangnhap1);
            if (String.IsNullOrEmpty(dangnhap1))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
                return this.DangNhap();
            }
            else if (String.IsNullOrEmpty(matkhau1))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
                return this.DangNhap();
            }
            else if (user1 == null)
            {
                ViewData["1"] = "Tài Khoản không tồn tại";
                return this.DangNhap();
            }
            else if (!String.Equals(ConvertMD5.MD5Hash(matkhau1), user1.passwordKH))
            {
                ViewData["2"] = "Sai mật khẩu";
                return this.DangKy();
            }
            else
            {
                Session["Taikhoan"] = user1;
                ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                return RedirectToAction("LichHoc", "KhoaTap");

            }

        }
        public ActionResult LogOut()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("DangNhap", "TaiKhoan");
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, TaiKhoanKH kh)
        {
            MyDataDataContext data = new MyDataDataContext();
            var accountKH = collection["accountKH"];
            var passwordKH = collection["passwordKH"];
            var fullName = collection["fullName"];
            var phoneNumber = collection["phoneNumber"];
            var gender = collection["gender"];
            var birtday = Convert.ToDateTime(collection["birtday"]);
            var gmail = collection["gmail"];


            if (String.IsNullOrEmpty(accountKH))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            if (String.IsNullOrEmpty(passwordKH))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            if (String.IsNullOrEmpty(fullName))
            {
                ViewData["Loi3"] = "Phải nhập họ tên khách hàng";
            }
            if (String.IsNullOrEmpty(phoneNumber))
            {
                ViewData["Loi4"] = "Phải nhập số điện thoại";
            }
            if (String.IsNullOrEmpty(gender))
            {
                ViewData["Loi5"] = "Giới tính không được để trống";
            }
            /*   if (String.IsNullOrEmpty(birtday))
               {
                   ViewData["Loi7"] = "Ngày sinh không được bỏ trống";
               }*/
            if (String.IsNullOrEmpty(gmail))
            {
                ViewData["Loi8"] = "Gmail không được bỏ trống";
            }


            else
            {
                kh.accountKH = accountKH.ToString();
                kh.passwordKH = ConvertMD5.MD5Hash(passwordKH.ToString());
                kh.fullName = fullName.ToString();
                kh.phoneNumber = phoneNumber;
              //  kh.gender = Convert.ToBoolean(gender);

                if (gender == "Nam")
                {
                    kh.gender = Convert.ToBoolean(true);
                }
                else if (gender == "Nữ")
                {
                    kh.gender = Convert.ToBoolean(false);
                }

                kh.birthday = birtday;
                kh.gmail = gmail;
                data.TaiKhoanKHs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }


    }
}