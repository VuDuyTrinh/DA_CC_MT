using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace DA_CCMT.Models
{
    //dùng để lấy thông tin các sản đã đặt của đơn đặt hàng
    public class dssp
    {
        QLNHDataContext data = new QLNHDataContext();
        //Khai báo các biến là các thuộc tính cần lấy của sản phẩm
        //Bao gồm các thuộc tính get và set
        public int iMaNH { set; get; }
        public string iTenNH { set; get; }
        public string iGia { set; get; }
        public string hinh { set; get; }
        public string sl { set; get; }
        //Phương thức lấy sản phẩm theo id
        public dssp(int id, int ids)
        {
            iMaNH = ids;
            //lấy chi tiết đặt hàng có MADDH = id và MÃ nước hoa = id NH 
            CTDDH dh = (from a in data.CTDDHs where (a.MADDH == id && a.MANH == ids) select a).Single();
            sl = dh.SoLuong.ToString();
            //lấy sản phẩm của đơn đặt hàng
            SanPham nh = data.SanPhams.SingleOrDefault(n => n.MaNH == ids);
            //gán lại giá trị cho các biến khai báo
            iTenNH = nh.TenNH;
            iGia = nh.Gia.ToString();
            hinh = nh.hinh;

        }
    }
}