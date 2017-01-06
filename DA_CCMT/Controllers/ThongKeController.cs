using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA_CCMT.Models;

namespace DA_CCMT.Controllers
{
    public class ThongKeController : Controller
    {
        //
        // GET: /ThongKe/
        QLNHDataContext data = new QLNHDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult taikhoan()
        {
            int i = data.Admins.Count();
            ViewBag.SL = i;
            return View(i);
        }
    }
}