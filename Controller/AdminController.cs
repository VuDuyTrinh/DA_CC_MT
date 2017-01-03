using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webfinal.Models;

namespace webfinal.Controllers
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
        public ActionResult CaiDat()
        {
            if(Session["Admin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            Admin ad = (Admin)Session["Admin"];
            return View(ad);
        }
        [HttpPost]
        public ActionResult CaiDat(FormCollection collection)
        {
            Admin ad = (Admin)Session["Admin"];
            var mk = collection["MK"];
            var mk2 = collection["MK2"];
            var mkcu = collection["MKC"];
            if(mk==""||mk2==""||mkcu=="")
            {
                ViewBag.ThongBao = "hãy nhập đầy đủ thông tin !";
                return View(ad);
            }
            if(mk!=mk2)
            {
                ViewBag.ThongBao = "nhập lại mật khẩu sai"; 
                return View(ad);
            }
            if(mkcu!=ad.MatKhau)
                {
                    ViewBag.ThongBao = "nhập mật khẩu cũ sai";
                    return View(ad);
                }
            if(mk.Length<5)
            {
                ViewBag.ThongBao = "mật khẩu phải trên 5 kí tự";
                return View(ad);
            }
            var b = data.Admins.SingleOrDefault(n => n.TenTK == ad.TenTK);
            b.MatKhau = mk;
            UpdateModel(b);
            data.SubmitChanges();
            ViewBag.ThongBao = "Đổi mật khẩu thành công"; 
            return View(ad);
            
        }
        public ActionResult admin()
        {
            if(Session["Admin"]==null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        public ActionResult DSNuocHoa()
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
            var ds = from a in data.SanPhams select a;
            return View(ds);
        }
        public ActionResult Loi()
        {
            return View();
        }
	}
}