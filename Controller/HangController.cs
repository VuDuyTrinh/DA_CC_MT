using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webfinal.Models;

namespace webfinal.Controllers
{
    public class HangController : Controller
    {
        //
        // GET: /Hang/
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult DsHang()
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
                if (i.iAction == "Hang" && i.iController == "Hang")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            var ds = from a in data.HangNHs select a;
            return View(ds);
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
                if (i.iAction == "Hang" && i.iController == "Hang")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Them(HangNH nh, HttpPostedFileBase fileUpload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    data.HangNHs.InsertOnSubmit(nh);
                    data.SubmitChanges();
                    ViewBag.Loi = "Thêm Thành Công";
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Loi = "Có lỗi xẩy ra khi thêm dữ liệu hãy kiểm tra lại . Dữ liệu có thể bị trùng hoặc sai định dạnh";
                return View();
            }
            return View();
        }
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
                if (i.iAction == "Hang" && i.iController == "Hang")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            var hang = data.HangNHs.SingleOrDefault(n => n.MaHang == id);
            return View(hang);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(int id,FormCollection collection)
        {
            var hang = data.HangNHs.SingleOrDefault(n => n.MaHang == id);
            var ten = collection["TenHang"];
            if(ten=="")
            {
                ViewBag.Loi = "Tên hãng không được để trống";
                return View(hang);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    hang.TenHang = ten;
                    UpdateModel(hang);
                    data.SubmitChanges();
                    ViewBag.Loi = "Sửa thành công";
                    return RedirectToAction("DsHang", "Hang");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Loi = "Có lỗi xẩy ra khi thêm dữ liệu hãy kiểm tra lại . Dữ liệu có thể bị trùng hoặc sai định dạnh";
                return View(hang);
            }
            return View(hang);
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
                if (i.iAction == "Hang" && i.iController == "Hang")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            HangNH nh = data.HangNHs.SingleOrDefault(n => n.MaHang == id);
            if (nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nh);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoaHang(int id)
        {
            HangNH nh = data.HangNHs.SingleOrDefault(n => n.MaHang == id);
            if (nh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                data.HangNHs.DeleteOnSubmit(nh);
                data.SubmitChanges();
            }
            catch
            {
                ViewBag.ThongBao = "Lỗi";
                return View();
            }
            return RedirectToAction("DsHang", "Hang");
        }
	}
}