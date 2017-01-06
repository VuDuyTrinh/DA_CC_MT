using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_CCMT.Models;

namespace DA_CCMT.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection conllection)
        {

            var tk = conllection["TK"];
            var mk = conllection["MK"];
            var ds = from a in data.CauHois
                     where !(from b in data.TraLois select b.MaCauHoi).Contains(a.MaCauHoi)
                     select a;
            Session["cauhoi"] = ds.Count();
            if (String.IsNullOrEmpty(tk) && String.IsNullOrEmpty(mk))
            {
                ViewData["loi"] = "hãy nhập đầy đủ thông tin để đăng nhập";
            }
            else
            {
                if (String.IsNullOrEmpty(tk))
                {
                    ViewData["loi"] = "tên tk không được đê trống";
                }
                else
                {
                    if (String.IsNullOrEmpty(mk))
                    {
                        ViewData["loi"] = "mk không được để trống";
                    }
                    else
                    {
                        Admin ad = data.Admins.SingleOrDefault(n => n.TenTK == tk && n.MatKhau == mk);
                        if (ad != null)
                        {
                            var cn = from a in data.CTCNs where (a.TenTK == ad.TenTK) select a;
                            List<CnAd> listcn = new List<CnAd>();
                            foreach (var i in cn)
                            {

                                CnAd c = new CnAd(i.MaCN);
                                listcn.Add(c);
                            }
                            Session["Admin"] = ad;
                            Session["CN"] = listcn;
                            return RedirectToAction("admin", "Admin");
                        }
                        else
                        {
                            ViewData["loi"] = "tên tk hoặc mật khẩu sai";
                        }
                    }
                }
            }
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["CN"] = null;
            Session["Admin"] = null;
            return RedirectToAction("Login", "Admin");
        }
        [HttpGet]
        
        public ActionResult Loi()
        {
            return View();
        }
	}
}