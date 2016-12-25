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
    }
}