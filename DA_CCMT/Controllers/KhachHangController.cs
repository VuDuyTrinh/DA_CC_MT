using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_CCMT.Models;
namespace DA_CCMT.Controllers
{
    public class KhachHangController : Controller
    {
        //
        // GET: /KhachHang/
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
                if (i.iAction == "KhachHang" && i.iController == "KhachHang")
                {
                    tencn = i.iTenCN;
                    co = 1;
                }
            }
            if (co == 0)
            {

                return RedirectToAction("Loi", "Admin");
            }
            var ds = from a in data.KhachHangs select a;
            return View(ds);
        }
	}
}