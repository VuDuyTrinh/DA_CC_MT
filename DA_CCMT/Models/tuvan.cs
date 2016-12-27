using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webfinal.Models
{
    public class tuvan
    {
        QLNHDataContext data = new QLNHDataContext();
        public int iMaCauHoi { get; set; }
        public string iCauHoi { get; set; }
        public int iMaKH { get; set; }
        public string itenKH { get; set; }
        public string iTraLoi { get; set; }
        public tuvan (int id)
        {
            iMaCauHoi = id;
            var a = data.CauHois.SingleOrDefault(n => n.MaCauHoi == id);
            iCauHoi = a.NoiDung;
            iMaKH = int.Parse(a.MaKH.ToString());
            var b = data.TraLois.SingleOrDefault(n => n.MaCauHoi == id);
            var c = data.KhachHangs.SingleOrDefault(n => n.MaKH == iMaKH);
            itenKH = c.TenKH;
            if(b==null)
            {
                iTraLoi = "chưa trả lời"; 
            }
            else
            {
                iTraLoi = b.TraLoi1;
            }
        }
    }
}