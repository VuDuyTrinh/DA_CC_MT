using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DA_CCMT.Models;
namespace DA_CCMT.Models
{
    public class dsCauHoi
    {
        QLNHDataContext data = new QLNHDataContext();
        public string cauhoi { get; set; }
        public string cautraloi { get; set; }
        public string tenkh { get; set; }
        public int macauhoi { get; set; }
        public dsCauHoi(int id)
        {
            macauhoi = id;
            CauHoi c = data.CauHois.SingleOrDefault(n => n.MaCauHoi == id);
            cauhoi = c.NoiDung;
            KhachHang k = data.KhachHangs.SingleOrDefault(m => m.MaKH == c.MaKH);
            tenkh = k.TenKH;
            TraLoi t = data.TraLois.SingleOrDefault(i => i.MaCauHoi == id);
            if(t!=null)
            {
                cautraloi = t.TraLoi1;
            }
            else
            {
                cautraloi = "";
            }
        }
    }
}