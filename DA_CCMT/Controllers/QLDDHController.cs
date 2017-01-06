using DA_CCMT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DA_CCMT.Controllers
{
    public class QLDDHController : Controller
    {
        // GET: QLDDH
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
        {
            //kiem tra xem admin nếu có thực hiện kiểm tra quyền ở phần phân quyền
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int co = 0;
            String tencn = "";
            //lấy các chức năng của tài khoản admin đăng nhập
            List<CnAd> cn = Session["CN"] as List<CnAd>;
            foreach (CnAd i in cn)
            {
                //kiểm tra xem tai khoản có quyền quản lý đơn đặt hàng k?
                if (i.iAction == "DDH" && i.iController == "DDH")
                {
                    tencn = i.iTenCN;
                    //nếu có gán biên bằng =1
                    co = 1;
                }
            }
            //kiểm tra biên cờ
            if (co == 0)
            {
                return RedirectToAction("Loi", "Admin");
            }
            //nếu cờ =1 thì lấy danh sách các đơn đặt hàng
            var ds = from a in data.DonDatHangs select a;
            return View(ds);
        }
        //
        public ActionResult DsCheck()
        {
            //kiem tra xem admin nếu có thực hiện kiểm tra quyền ở phần phân quyền
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            int co = 0;
            String tencn = "";
            //lấy các chức năng của tài khoản admin đăng nhập
            List<CnAd> cn = Session["CN"] as List<CnAd>;
            foreach (CnAd i in cn)
            {
                //kiểm tra xem tai khoản có quyền quản lý đơn đặt hàng k?
                if (i.iAction == "DDH" && i.iController == "DDH")
                {
                    tencn = i.iTenCN;
                    //nếu có gán biên bằng =1
                    co = 1;
                }
            }
            //kiểm tra biên cờ
            if (co == 0)
            {
                return RedirectToAction("Loi", "Admin");
            }
            //lấy ra danh sách các các đơn đặt hàng chưa giao hoặc chưa thanh toán
            var ds = from a in data.DonDatHangs where (a.TinhTrangGiaoHang == false || a.DaThanhToan == false) select a;
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
    }
}