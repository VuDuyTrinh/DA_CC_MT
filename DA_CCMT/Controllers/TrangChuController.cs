using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;   // Sử dụng PagedList để phân trang
using PagedList.Mvc;
using System;
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
        [HttpPost]
        public ActionResult DangNhap(FormCollection conllection, String strURL)
        {
            var tk = conllection["inputEmail"];
            var mk = conllection["inputPassword"];
            KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tk && n.MatKhau == mk);
            if (kh != null)
            {
                Session["TaiKhoan"] = kh;
                return Redirect(strURL);
            }
            else
            {
                ViewData["loi"] = "tên tk hoặc mật khẩu sai";
                return Redirect(strURL);
            }
        }
        public ActionResult dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult dangky(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["HoTen"];
            var tk = collection["tk"];
            var mk = collection["mk"];
            var mk2 = collection["mk2"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            var diachi = collection["DC"];
            var sdt = collection["sdt"];
            var email = collection["email"];
            int co = 1;
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["loi1"] = " *";
                co = 0;

            }
            if (String.IsNullOrEmpty(tk))
            {
                ViewData["loi2"] = " *";
                co = 0;
            }
            if (String.IsNullOrEmpty(mk))
            {
                ViewData["loi3"] = " *";
                co = 0;
            }
            if (String.IsNullOrEmpty(mk2))
            {
                ViewData["loi4"] = " *";
                co = 0;
            }

            if (String.IsNullOrEmpty(sdt))
            {
                ViewData["loi7"] = " *";
                co = 0;
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["loi8"] = " *";
                co = 0;
            }
            if (mk != mk2)
            {
                ViewData["loi4"] = "( nhập lại mật khẩu sai)";
                co = 0;
            }
            if (co == 1)
            {
                kh.TenKH = hoten;
                kh.TaiKhoan = tk;
                kh.MatKhau = mk;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                kh.DiaChi = diachi;
                kh.DienThoai = sdt;
                kh.Email = email;
                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                Session["TaiKhoan"] = kh;
                return RedirectToAction("Index");
            }
            return this.dangky();
        }
        public ActionResult chitiet(int id)
        {
            var ct = (from t in data.SanPhams where (t.MaNH == id) select t);
            return View(ct.SingleOrDefault());
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
        public ActionResult SpHang(int? page, int id)
        {
            int SoSP = 9;
            int SoTrang = (page ?? 1);
            var sp = from a in data.SanPhams where (a.MaHang == id) select a;
            return View(sp.ToPagedList(SoTrang, SoSP));
        }
        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult ShowRoom()
        {
            return View();
        }
        public List<tuvan> laytuvan()
        {
            List<tuvan> tv = new List<tuvan>();
            var a = from b in data.CauHois orderby b.MaCauHoi descending select b;

            foreach (var item in a)
            {
                tuvan t = new tuvan(item.MaCauHoi);
                tv.Add(t);
            }
            return tv;
        }
        public ActionResult TuVan(int? pape)
        {
            List<tuvan> tv = laytuvan();
            int SoSP = 9;
            int SoTrang = (pape ?? 1);
            return View(tv.ToPagedList(SoTrang, SoSP));
        }
        [HttpPost]
        public ActionResult TuVan(int? pape, FormCollection colection)
        {
            List<tuvan> tv = laytuvan();
            int SoSP = 9;
            int SoTrang = (pape ?? 1);
            if (Session["TaiKhoan"] == null)
            {
                ViewBag.ThongBao = "Bạn phải đăng nhập để gửi thư";
                return View(tv.ToPagedList(SoTrang, SoSP));
            }
            else
            {
                string cauhoi = colection["cauhoi"];
                if (cauhoi == "")
                {
                    ViewBag.ThongBao = "Hãy nhập câu hỏi";
                    return View(tv.ToPagedList(SoTrang, SoSP));
                }
                else
                {
                    KhachHang kh = (KhachHang)Session["TaiKhoan"];
                    CauHoi c = new CauHoi();
                    c.MaKH = kh.MaKH;
                    c.NoiDung = cauhoi;
                    c.TraLoi = 0;
                    data.CauHois.InsertOnSubmit(c);
                    data.SubmitChanges();
                    tv = laytuvan();
                    return View(tv.ToPagedList(SoTrang, SoSP));
                }
            }
            return View(tv.ToPagedList(SoTrang, SoSP));
        }
    }
}