using DA_CCMT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DA_CCMT.Controllers
{
    public class QLDDHController : Controller
    {
        // GET: QLDDH
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
        {
            //nếu cờ =1 thì lấy danh sách các đơn đặt hàng
            var ds = from a in data.DonDatHangs select a;
            return View(ds);
        }
    }
}