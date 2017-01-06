using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DA_CCMT
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //Cấu hình trang chủ cho web bằng controller và action trong Folder Controllers
                defaults: new { controller = "TrangChu", action = "Index", id = UrlParameter.Optional } 
            );
        }
    }
}
