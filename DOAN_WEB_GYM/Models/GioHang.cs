using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN_WEB_GYM.Models
{
    public class GioHang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int iMaKhoaTap { set; get; }
        public string sTenKhoaTap { set; get; }
        public DateTime sNgayBatDau { set; get; }
        public DateTime sNgayKetThuc { set; get; }
        public int dGia { set; get; }
        public string urlImg { set; get; }
        public int idCLB { set; get; }
        public int idTime { set; get; }
        public int idDOW { set; get; }
        public int idPT { set; get; }
        public Double dThanhtien
        {
            get
            {
                return dGia;
            }
        }
        // GET: GioHang
        public GioHang(int maKhoaTap)
        {
            iMaKhoaTap = maKhoaTap;
            KhoaTap khoaTap = data.KhoaTaps.Single(n => n.idCourse == iMaKhoaTap);
            sTenKhoaTap = khoaTap.nameCourse;
            sNgayBatDau = Convert.ToDateTime(khoaTap.startDay);
            sNgayKetThuc = Convert.ToDateTime(khoaTap.dueDay);
            dGia = int.Parse(khoaTap.price.ToString());
            urlImg = khoaTap.urlImg;
            idCLB = Convert.ToInt32(khoaTap.idCLB);
            idTime = Convert.ToInt32(khoaTap.idTime);
            idDOW = Convert.ToInt32(khoaTap.idDOW);
            if (khoaTap.idPT.ToString() == null)
                idPT = 0;
            else
                idPT = Convert.ToInt32(khoaTap.idPT);
        }
    }
}