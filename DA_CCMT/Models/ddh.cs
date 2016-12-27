using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webfinal.Models;

namespace webfinal.Models
{
    public class ddh
    {
        QLNHDataContext data = new QLNHDataContext();
        public int iMaDDH { set; get; }
        public string iThanhToan { set; get; }
        public string iNgayDat { set; get; }
        public string iNgayGiao { set; get; }
        public string iGiaoHang { set; get; }
        public KhachHang ikhachhang { set; get; }
        public ddh(int id)
        {
            DonDatHang dh = data.DonDatHangs.SingleOrDefault(n => n.MADDH == id);
            iMaDDH = dh.MADDH;
            iThanhToan = dh.DaThanhToan.ToString();
            iNgayDat = dh.NgayDat.ToString();
            iNgayGiao = dh.NgayGiao.ToString();
            iGiaoHang = dh.TinhTrangGiaoHang.ToString();
            ikhachhang = data.KhachHangs.SingleOrDefault(m => m.MaKH == dh.MaKH);
        }
    }
}