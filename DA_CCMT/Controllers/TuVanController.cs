using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webfinal.Models;
using System.IO;

namespace webfinal.Controllers
{
    public class TuVanController : Controller
    {
        //
        // GET: /TuVan/
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
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
                if (i.iAction == "TuVan" && i.iController == "TuVan")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            var ds = from a in data.CauHois select a;
            List<dsCauHoi> ls = new List<dsCauHoi>();
            foreach(var i in ds)
            {
                dsCauHoi d = new dsCauHoi(i.MaCauHoi);
                ls.Add(d);
            }
            return View(ls);
        }
        [HttpGet]
        public ActionResult xoa(int id)
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
                if (i.iAction == "TuVan" && i.iController == "TuVan")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            var ds = from a in data.CauHois where (a.MaCauHoi == id) select a;
            return View(ds.SingleOrDefault());
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            var ds = (from a in data.CauHois where (a.MaCauHoi == id) select a).SingleOrDefault();
            var tl = (from b in data.TraLois where (b.MaCauHoi == ds.MaCauHoi) select b).SingleOrDefault();
            try
            {
                if (tl != null)
                {
                    data.TraLois.DeleteOnSubmit(tl);
                    data.SubmitChanges();
                }
                data.CauHois.DeleteOnSubmit(ds);
                data.SubmitChanges();
                return RedirectToAction("Index", "TuVan");
            }
            catch(Exception ex)
            {
                ViewBag.ThongBao = "Lỗi";
                return View();
            }
            
        }
        public ActionResult TinNhan()
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
                if (i.iAction == "TuVan" && i.iController == "TuVan")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            var ds = from a in data.CauHois
                     where !(from b in data.TraLois select b.MaCauHoi).Contains(a.MaCauHoi)
                     select a;
            Session["cauhoi"] = ds.Count();
            return View(ds);
        }
        [HttpGet]
        public ActionResult TraLoiCH(int id)
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
                if (i.iAction == "TuVan" && i.iController == "TuVan")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            var ds = from a in data.CauHois where (a.MaCauHoi == id) select a;
            return View(ds.SingleOrDefault());
        }
        [HttpPost]
        public ActionResult TraLoiCH(int id,FormCollection collection)
        {
            string tl = collection["traloi"];
            var c = data.CauHois.SingleOrDefault(n => n.MaCauHoi == id);
            if(tl=="")
            {
                ViewBag.ThongBao = "hay nhập câu trả lời";
                return View(c);
            }
            TraLoi t = new TraLoi();
            try
            {
                t.TraLoi1 = tl;
                t.MaCauHoi = id;
                Admin a = (Admin)Session["Admin"];
                t.TenTK = a.TenTK;
                data.TraLois.InsertOnSubmit(t);
                data.SubmitChanges();
                return RedirectToAction("TinNhan", "TuVan");
            }
            catch (Exception ex)
            {
                ViewBag.ThongBao = ex;
                return View(c);
            }
            
        }
	}
}