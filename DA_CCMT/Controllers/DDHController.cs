using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webfinal.Models;

namespace webfinal.Controllers
{
    public class DDHController : Controller
    {
        //
        // GET: /DDH/
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
        {
            if(Session["Admin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int co = 0;
            String tencn = "";
            List<CnAd> cn = Session["CN"] as List<CnAd>;
            foreach (CnAd i in cn)
            {
                if (i.iAction == "DDH" && i.iController == "DDH")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {
                return RedirectToAction("Loi", "Admin");
            }
            var ds = from a in data.DonDatHangs select a;
            return View(ds);

        }
        public ActionResult DsCheck()
        {
            if(Session["Admin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int co = 0;
            String tencn = "";
            List<CnAd> cn = Session["CN"] as List<CnAd>;
            foreach (CnAd i in cn)
            {
                if (i.iAction == "DDH" && i.iController == "DDH")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {
                return RedirectToAction("Loi", "Admin");
            }
            var ds = from a in data.DonDatHangs where (a.TinhTrangGiaoHang==false || a.DaThanhToan==false) select a;
            return View(ds);
        }
        public ActionResult ChiTietDDH(int id)
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
                if (i.iAction == "DDH" && i.iController == "DDH")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {
                return RedirectToAction("Loi", "Admin");
            }
            ddh dondathang = new ddh(id);
            return View(dondathang);
        }
        public ActionResult Check(int id)
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
                if (i.iAction == "DDH" && i.iController == "DDH")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {
                return RedirectToAction("Loi", "Admin");
            }
            ddh dondathang = new ddh(id);
            return View(dondathang);
        }
        public ActionResult DsSanPham(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var ds = from a in data.CTDDHs where (a.MADDH == id) select a;
            List<dssp> sp = new List<dssp>();
            foreach(var i in ds)
            {
                dssp a = new dssp(i.MADDH, i.MANH);
                sp.Add(a);
            }
            return PartialView(sp);
        }
        public ActionResult ThanhToan(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var dh = data.DonDatHangs.SingleOrDefault(n => n.MADDH == id);
            dh.DaThanhToan = true;
            UpdateModel(dh);
            data.SubmitChanges();
            return RedirectToAction("ChiTietDDH", "DDH", new { id = dh.MADDH });
        }
        public ActionResult GiaoHang(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var dh = data.DonDatHangs.SingleOrDefault(n => n.MADDH == id);
            dh.TinhTrangGiaoHang = true;
            UpdateModel(dh);
            data.SubmitChanges();
            return RedirectToAction("ChiTietDDH", "DDH", new { id = dh.MADDH });
        }
        public ActionResult Huy(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            var dh = data.DonDatHangs.SingleOrDefault(n => n.MADDH == id);
            var ct = from a in data.CTDDHs where (a.MADDH == id) select a;
            try
            {
                foreach(var i in ct)
                {
                    data.CTDDHs.DeleteOnSubmit(i);
                    data.SubmitChanges();
                }
                data.DonDatHangs.DeleteOnSubmit(dh);
                data.SubmitChanges();
                return RedirectToAction("DsCheck", "DDH");
            }
            catch (Exception ex)
            {
                return RedirectToAction("DsCheck", "DDH");
            }
        }
	}
}