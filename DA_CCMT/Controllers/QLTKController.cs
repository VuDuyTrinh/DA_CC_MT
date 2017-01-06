using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_CCMT.Models;

namespace DA_CCMT.Controllers
{
    public class QLTKController : Controller
    {
        //
        // GET: /QLTK/
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            //int co = 0;
            //String tencn = "";
            //List<CnAd> cn = Session["CN"] as List<CnAd>;
            //foreach (CnAd i in cn)
            //{
            //    if (i.iAction == "QLTK" && i.iController == "QLTK")
            //    {
            //        tencn = i.iTenCN;
            //        co = 1;
            //    }
            //}
            //if (co == 0)
            //{

            //    return RedirectToAction("Loi", "Admin");
            //}
            var ds = from a in data.Admins select a;
            return View(ds);
        }
        public ActionResult Chitiet(string id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            //int co = 0;
            //String tencn = "";
            //List<CnAd> cn = Session["CN"] as List<CnAd>;
            //foreach (CnAd i in cn)
            //{
            //    if (i.iAction == "QLTK" && i.iController == "QLTK")
            //    {
            //        tencn = i.iTenCN;
            //        co = 1;
            //    }
            //}
            //if (co == 0)
            //{

            //    return RedirectToAction("Loi", "Admin");
            //}
            Admin a = data.Admins.SingleOrDefault(n => n.TenTK == id);
            ViewBag.TenTK = a.TenTK;
            ViewBag.HoTen = a.HoTen;
            List<CnAd> chucnang = new List<CnAd>();
            var c = from t in data.CTCNs where (t.TenTK == a.TenTK) select t;
            foreach (var i in c)
            {
                CnAd cn1 = new CnAd(i.MaCN);
                chucnang.Add(cn1);
            }
            return View(chucnang);
        }
        [HttpGet]
        public ActionResult taotk()
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            //int co = 0;
            //String tencn = "";
            //List<CnAd> cn = Session["CN"] as List<CnAd>;
            //foreach (CnAd i in cn)
            //{
            //    if (i.iAction == "QLTK" && i.iController == "QLTK")
            //    {
            //        tencn = i.iTenCN;
            //        co = 1;
            //    }
            //}
            //if (co == 0)
            //{

            //    return RedirectToAction("Loi", "Admin");
            //}
            var a = from b in data.ChucNangs select b;
            return View(a);
        }
        [HttpPost]
        public ActionResult taotk(FormCollection collection)
        {
             var a = from b in data.ChucNangs select b;
            var tentk = collection["TenTK"];
            var hoten = collection["HoTen"];
            var mk = collection["MK"];
            var mk2 = collection["MK2"];
            if(tentk==""||hoten==""||mk==""||mk2=="")
            {
                ViewBag.ThongBao = "Hãy nhập đầy đủ thông tin";
                return View(a);
            }
            if(mk!=mk2)
            {
                ViewBag.ThongBao = "Nhập lại mật khẩu sai";
                return View(a);
            }
            if (mk.Length<5)
            {
                ViewBag.ThongBao = "mật khẩu phải lớn hơn 5 ký tự";
                return View(a);
            }
            var tam = data.Admins.SingleOrDefault(n => n.TenTK == tentk);
            if(tam!=null)
            {
                ViewBag.ThongBao = "Tên tài khoản đã tồn tại";
                return View(a);
            }
            Admin ad = new Admin();
            ad.HoTen = hoten;
            ad.MatKhau = mk;
            ad.TenTK = tentk;
            data.Admins.InsertOnSubmit(ad);
            data.SubmitChanges();
            
            foreach(var i in a)
            {
                if(collection[i.TenCN.ToString()]==i.TenCN.ToString())
                {
                    CTCN ct = new CTCN();
                    ct.MaCN = i.MaCN;
                    ct.TenTK = tentk;
                    data.CTCNs.InsertOnSubmit(ct);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("Index");
        }
	}
}