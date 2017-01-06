using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webfinal.Models;

namespace webfinal.Controllers
{
    public class NuocHoaController : Controller
    {
        //
        // GET: /NuocHoa/
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public int KtRong(int co ,string s)
        {
            if(s=="")
            {
               return 0;
            }
            return co;
        }
        public ActionResult Them()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int co = 0;
            String tencn = "";
            List<CnAd> cn = Session["CN"] as List<CnAd>;
            foreach (CnAd i in cn)
            {
                if (i.iAction == "NuocHoa" && i.iController == "NuocHoa")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            ViewBag.MaLoai = new SelectList(data.LoaiNHs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaHang = new SelectList(data.HangNHs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Them(SanPham nh, HttpPostedFileBase fileUpload, FormCollection collection)
        {
            ViewBag.MaLoai = new SelectList(data.LoaiNHs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaHang = new SelectList(data.HangNHs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            var tennh = collection["TenNH"];
            var gia = collection["Gia"];
            var nongdo = collection["NongDo"];
            var nhomhuong = collection["NhomHuong"];
            var phongcach = collection["PhongCach"];
            var nguoisl = collection["NguoiSL"];
            var namsx = collection["NamSX"];
            var xuatxu = collection["XuatXu"];
            var mota = collection["MoTa"];
            var maloai = collection["MaLoai"];
            var mahang = collection["MaHang"];
            int co = 1;
            co=KtRong(co,tennh);
            co = KtRong(co, nongdo);
            co = KtRong(co, nhomhuong);
            co = KtRong(co, phongcach);
            co = KtRong(co, nguoisl);
            co = KtRong(co, namsx);
            co = KtRong(co, xuatxu);
            co = KtRong(co, mota);
            if (co == 0)
            {
                ViewBag.Loi = "hãy nhập đầy đủ thông tin nước hoa";
                return View();
            }
            else
            {
                if (fileUpload == null)
                {
                    ViewBag.Loi = "Vui Long chọn ảnh bìa";
                    return View();
                }
                else
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            var filename = Path.GetFileName(fileUpload.FileName);
                            var path = Path.Combine(Server.MapPath("~/hinhsp"), filename);
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.ThongBao = "hinh đã tồn tại";
                            }
                            else
                            {
                                fileUpload.SaveAs(path);
                            }
                            nh.hinh = filename;
                            nh.TenNH = tennh;
                            nh.Gia = int.Parse(gia.ToString());
                            nh.NongDo = nongdo;
                            nh.NhomHuong = nhomhuong;
                            nh.NguoiSL = nguoisl;
                            nh.PhongCach = phongcach;
                            nh.MoTa = mota;
                            nh.NamSX = int.Parse(namsx.ToString());
                            nh.MaHang = int.Parse(mahang.ToString());
                            nh.MaLoai = int.Parse(maloai.ToString());
                            nh.SoLuong = 0;
                            data.SanPhams.InsertOnSubmit(nh);
                            data.SubmitChanges();
                            ViewBag.Loi = "Thêm Thành Công";
                            return View();
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Loi = "Có lỗi xẩy ra khi thêm dữ liệu hãy kiểm tra lại . Dữ liệu có thể bị trùng hoặc sai định dạng";
                        return View();
                    }
                    return View();
                }
            }
        }
        [HttpGet]
        public ActionResult Sua(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int co = 0;
            String tencn = "";
            List<CnAd> cn = Session["CN"] as List<CnAd>;
            foreach (CnAd i in cn)
            {
                if (i.iAction == "NuocHoa" && i.iController == "NuocHoa")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            SanPham nh = data.SanPhams.SingleOrDefault(n => n.MaNH == id);
            if (nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoai = new SelectList(data.LoaiNHs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", nh.MaLoai);
            ViewBag.MaHang = new SelectList(data.HangNHs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang", nh.MaHang);
            return View(nh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(int id,HttpPostedFileBase fileUpload, FormCollection collection)
        {
            ViewBag.MaLoai = new SelectList(data.LoaiNHs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaHang = new SelectList(data.HangNHs.ToList().OrderBy(n => n.TenHang), "MaHang", "TenHang");
            var tennh = collection["TenNH"];
            var gia = collection["Gia"];
            var nongdo = collection["NongDo"];
            var nhomhuong = collection["NhomHuong"];
            var phongcach = collection["PhongCach"];
            var nguoisl = collection["NguoiSL"];
            var namsx = collection["NamSX"];
            var xuatxu = collection["XuatXu"];
            var mota = collection["MoTa"];
            var maloai = collection["MaLoai"];
            var mahang = collection["MaHang"];
            int co = 1;
            co = KtRong(co, tennh);
            co = KtRong(co, nongdo);
            co = KtRong(co, nhomhuong);
            co = KtRong(co, phongcach);
            co = KtRong(co, nguoisl);
            co = KtRong(co, namsx);
            co = KtRong(co, xuatxu);
            co = KtRong(co, mota);
            SanPham sp = data.SanPhams.SingleOrDefault(n => n.MaNH == id);
            if (co == 0)
            {
                ViewBag.Loi = "hãy nhập đầy đủ thông tin nước hoa";
                return View(sp);
            }
            else
            {
                var nh = data.SanPhams.First(m => m.MaNH == id);
                if (fileUpload == null)
                {
                    try
                    {
                            nh.TenNH = tennh;
                            nh.Gia = int.Parse(gia.ToString());
                            nh.NongDo = nongdo;
                            nh.NhomHuong = nhomhuong;
                            nh.NguoiSL = nguoisl;
                            nh.PhongCach = phongcach;
                            nh.MoTa = mota;
                            nh.NamSX = int.Parse(namsx.ToString());
                            nh.MaHang = int.Parse(mahang.ToString());
                            nh.MaLoai = int.Parse(maloai.ToString());
                            nh.SoLuong = 0;
                            TryUpdateModel(nh);
                            data.SubmitChanges();
                            ViewBag.Loi = "Thêm Thành Công";
                            return RedirectToAction("DSNuocHoa", "Admin");
                        
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Loi = "Có lỗi xẩy ra khi thêm dữ liệu hãy kiểm tra lại . Dữ liệu có thể bị trùng hoặc sai định dạng";
                        return View(sp);
                    }
                    
                }
                else
                {
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            var filename = Path.GetFileName(fileUpload.FileName);
                            var path = Path.Combine(Server.MapPath("~/hinhsp"), filename);
                            if (System.IO.File.Exists(path))
                            {
                                ViewBag.ThongBao = "hinh đã tồn tại";
                            }
                            else
                            {
                                fileUpload.SaveAs(path);
                            }
                            nh.hinh = filename;
                            nh.TenNH = tennh;
                            nh.Gia = int.Parse(gia.ToString());
                            nh.NongDo = nongdo;
                            nh.NhomHuong = nhomhuong;
                            nh.NguoiSL = nguoisl;
                            nh.PhongCach = phongcach;
                            nh.MoTa = mota;
                            nh.NamSX = int.Parse(namsx.ToString());
                            nh.MaHang = int.Parse(mahang.ToString());
                            nh.MaLoai = int.Parse(maloai.ToString());
                            nh.SoLuong = 0;
                            UpdateModel(nh);
                            data.SubmitChanges();
                            ViewBag.Loi = "Thêm Thành Công";
                            return RedirectToAction("DSNuocHoa","Admin");
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Loi = "Có lỗi xẩy ra khi thêm dữ liệu hãy kiểm tra lại . Dữ liệu có thể bị trùng hoặc sai định dạng";
                        return View();
                    }
                    return View(sp);
                }
            }
        }
        public ActionResult Xoa(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int co = 0;
            String tencn = "";
            List<CnAd> cn = Session["CN"] as List<CnAd>;
            foreach (CnAd i in cn)
            {
                if (i.iAction == "NuocHoa" && i.iController == "NuocHoa")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            SanPham nh = data.SanPhams.SingleOrDefault(n => n.MaNH == id);
            if (nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nh);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int id)
        {
            SanPham nh = data.SanPhams.SingleOrDefault(n => n.MaNH == id);
            if (nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                data.SanPhams.DeleteOnSubmit(nh);
                data.SubmitChanges();
            }
            catch
            {
                ViewBag.ThongBao = "Lỗi";
                return View();
            }
            return RedirectToAction("DSNuocHoa", "Admin");
        }
        public ActionResult ChiTiet(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int co = 0;
            String tencn = "";
            List<CnAd> cn = Session["CN"] as List<CnAd>;
            foreach (CnAd i in cn)
            {
                if (i.iAction == "NuocHoa" && i.iController == "NuocHoa")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            SanPham nh = data.SanPhams.SingleOrDefault(n => n.MaNH == id);
            if (nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            HangNH hang = data.HangNHs.SingleOrDefault(n => n.MaHang == nh.MaHang);
            LoaiNH loai = data.LoaiNHs.SingleOrDefault(n => n.MaLoai == nh.MaLoai);
            ViewBag.hang = hang.TenHang;
            ViewBag.loai = loai.TenLoai;
            return View(nh);
        }
	}
}