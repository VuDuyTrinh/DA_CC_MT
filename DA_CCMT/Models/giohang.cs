using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_CCMT.Models
{
    public class giohang
    {
        QLNHDataContext data = new QLNHDataContext();
        public int iMANH { set; get; }
        public String iHinh { set; get; }
        public String iTenNH { set; get; }
        public int iGia { set; get; }
        public int iSoLuong { set; get; }
        public Double iThanhTie { get { return iSoLuong * iGia; } }
        public giohang(int MANH, int sl)
        {
            iMANH = MANH;
            SanPham nh = data.SanPhams.Single(n => n.MaNH == iMANH);
            iHinh = nh.hinh;
            iTenNH = nh.TenNH;
            iGia = int.Parse(nh.Gia.ToString());
            iSoLuong = sl;
        }
    }
}