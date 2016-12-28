using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;   // Sử dụng PagedList để phân trang
using PagedList.Mvc;
using DA_CCMT.Models;

namespace DA_CCMT.Controllers
{
    public class TrangChuController : Controller
    {
        QLNHDataContext data = new QLNHDataContext(); // Biến data quản lý CSDL
        private List<SanPham> LaySP()
        {
            return data.SanPhams.ToList(); // hàm trả về list table SanPham
        }

        public ActionResult Index(int? page ) // Khi nhấn vào trang chủ hàm này sẽ kích hoạt
        {                                      //tham số page để chỉ ra số trang trong list sản phẩm ở trang chủ
            int SoSP = 9; // SoSP chỉ ra số sản phẩm sẽ hiển thị trên web, nếu quá hơn số sp sẽ tự phân thành trang kế tiếp             
            int SoTrang = (page ?? 1); //Chỉ ra số trang 

            var nh = LaySP(); //Lấy list sản phẩm có trong CSDL
            return View(nh.ToPagedList(SoTrang, SoSP)); //Trả về Model chứa table SanPham, chứa hàm phân trang có trong PagedList
        }


        public ActionResult Hang() 
        {
            var hang = (from a in data.HangNHs select a).Take(12); //Lấy 12 sản phẩm
            return PartialView(hang);
        }


        public ActionResult SpHot(int i) // i là số sản phẩm sẽ bỏ qua để lấy các sản phẩm khác nhau trong carousel slide
        {
            var sp = (from a in data.SanPhams select a).Skip(i).Take(4); //Lấy 4 sản phẩm để hiển thị trong carousel slide
            return PartialView(sp); //Đây là PartialView của LayoutUser
        }


        public ActionResult ChoNam(int? pape)//trang sản phẩm cho nam
        {
            var t = from a in data.SanPhams where (a.MaLoai == 1) select a;//truy vấn lấy dữ liệu sản phẩm
            int SoSP = 9;//số sản phẩm trong một trang là 9
            int SoTrang = (pape ?? 1);//số trang
            return View(t.ToPagedList(SoTrang, SoSP));//trả về danh sách sản phẩm và số trang

        }
        public ActionResult ChoNu(int? pape)//trang sản phẩm cho nữ
        {
            var t = from a in data.SanPhams where (a.MaLoai == 2) select a;//truy vấn
            int SoSP = 9;//số sản phẩm trong một trang
            int SoTrang = (pape ?? 1);
            return View(t.ToPagedList(SoTrang, SoSP));//trả về danh sách sản phẩm và số trang
        }
    }
}