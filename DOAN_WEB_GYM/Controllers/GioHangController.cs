using DemoVNPay.Others;
using DOAN_WEB_GYM.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DOAN_WEB_GYM.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext db = new MyDataDataContext();

        // GET: GioHang
        public List<GioHang> Laygiohang()
        {
            List<GioHang> lstGiohang = Session["Giohang"] as List<GioHang>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<GioHang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        // them vao gio hang
        public ActionResult ThemGiohang(int iMaKhoaTap, string strURL)
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("CLB", "CauLacBo");
            }
            TaiKhoanKH tkkh = (TaiKhoanKH)Session["Taikhoan"];
            List<ChiTietDangKyKhoaTap> lstCTDDKKT = new List<ChiTietDangKyKhoaTap>();
            List<GioHang> lstGiohang = Laygiohang();
            var khoaTapDB = db.KhoaTaps.ToList();
            var ktDaDK = db.DangKyKhoaTaps.Where(b => b.idGuest == tkkh.idGuest).ToList();
            foreach (var item1 in ktDaDK)
            {
                var ctdkkt = db.ChiTietDangKyKhoaTaps.Where(b => b.idInvoice == item1.idInvoice).ToList();
                foreach (var item2 in ctdkkt)
                    lstCTDDKKT.Add(item2);
            }
            KhoaTap kt = db.KhoaTaps.SingleOrDefault(n => n.idCourse == iMaKhoaTap);
            ThuTrongTuan thuKT = db.ThuTrongTuans.SingleOrDefault(n => n.idDOW == kt.idDOW);
            GioHang khoaTap = lstGiohang.Find(n => n.iMaKhoaTap == iMaKhoaTap);
            if (khoaTap == null)
            {
                if (lstCTDDKKT.Count > 0)
                {
                    foreach (var item1 in lstCTDDKKT)
                    {
                        if (item1.idCourse == iMaKhoaTap)
                        {
                            ViewData["Warning1"] = "Tồn tại khóa tập này";
                            return RedirectToAction("CLB", "CauLacBo");
                        }

                        var khoaTap_temp = db.KhoaTaps.Where(w => w.idCourse == item1.idCourse && w.dueDay >= DateTime.Now).ToList();
                        foreach (var item in khoaTap_temp)
                        {
                            ThuTrongTuan thuKT_temp = db.ThuTrongTuans.SingleOrDefault(n => n.idDOW == item.idDOW);
                            if (item.idDOW == kt.idDOW && item.idTime == kt.idTime && thuKT.dateInWeek.Contains(thuKT.dateInWeek))
                            {
                                ViewData["Warning2"] = "Trùng thời gian";
                                return RedirectToAction("CLB", "CauLacBo");
                            }
                        }
                    }
                }
                khoaTap = new GioHang(iMaKhoaTap);
                lstGiohang.Add(khoaTap);
                for (int i = 0; i < lstGiohang.Count(); i++)
                {
                    if ((lstGiohang[i].sNgayBatDau > khoaTap.sNgayBatDau && lstGiohang[i].sNgayKetThuc < khoaTap.sNgayKetThuc)
                        || (lstGiohang[i].sNgayBatDau < khoaTap.sNgayBatDau && lstGiohang[i].sNgayKetThuc > khoaTap.sNgayKetThuc)
                        && lstGiohang[i].idDOW == khoaTap.idDOW && lstGiohang[i].idTime == khoaTap.idTime)
                    {
                        lstGiohang.Remove(khoaTap);
                        return RedirectToAction("Index", "GYM");
                    }
                }
                return Redirect(strURL);
            }

            return Redirect(strURL);
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGiohang = Session["GioHang"] as List<GioHang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }

            return iTongTien;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "GYM");
            }
            ViewBag.Tongtien = TongTien();
            Session["TongTien"] = TongTien();
            return View(lstGiohang);
        }
        //xoa gio hang
        public ActionResult XoaGiohang(int iMaKT)
        {
            List<GioHang> lstGiohang = Laygiohang();
            GioHang khoaTap = lstGiohang.SingleOrDefault(n => n.iMaKhoaTap == iMaKT);
            if (khoaTap != null)
            {
                lstGiohang.RemoveAll(n => n.iMaKhoaTap == iMaKT);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("CLB", "CauLacBo");
            }
            return RedirectToAction("GioHang");
        }
        //xoa tat ca gio hang
        public ActionResult XoaTatCaGioHang()
        {
            //Lay gio hang tu Session
            List<GioHang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("CLB", "CauLacBo");
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            //kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "TaiKhoan");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("CLB", "CauLacBo");
            }
            List<GioHang> lstGiohang = Laygiohang();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult DangKy(FormCollection collection)
        {
            // them don dang ky
            DangKyKhoaTap ddk = new DangKyKhoaTap();
            TaiKhoanKH kh = (TaiKhoanKH)Session["Taikhoan"];
            List<GioHang> gh = Laygiohang();
            ddk.idGuest = kh.idGuest;
            ddk.idDate = DateTime.Now;
            ddk.status = false;
            db.DangKyKhoaTaps.InsertOnSubmit(ddk);
            db.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDangKyKhoaTap ctdk = new ChiTietDangKyKhoaTap();
                ctdk.idInvoice = ddk.idInvoice;
                ctdk.price = item.dGia;
                ctdk.idCourse = item.iMaKhoaTap;
                db.ChiTietDangKyKhoaTaps.InsertOnSubmit(ctdk);
            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandangky", "GioHang");
        }
        public ActionResult Xacnhandangky()
        {
            return View();
        }
        // xay dung trang gio hang
        public ActionResult Payment()
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.0.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.0.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", Convert.ToString((Convert.ToInt32(Session["TongTien"]) * 100))); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);
            Session["TongTien"] = null;
            return Redirect(paymentUrl);    
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Chúc mừng bạn đã thanh toán online thành công. hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Chúc mừng bạn đã thanh toán online thành công.";
                }
            }

            return View();
        }
    }
}
