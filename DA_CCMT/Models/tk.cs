using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_CCMT.Models
{
    public class tk
    {
        QLNHDataContext data = new QLNHDataContext();
        public string tentk { set; get; }
        public string hoten {set; get;}
        public List<CnAd> chucnang {set; get;}
        public tk(string id)
        {
            Admin a = data.Admins.SingleOrDefault(n => n.TenTK == id);
            tentk = id;
            hoten = a.HoTen;
            var c = from t in data.CTCNs where (t.TenTK == a.TenTK) select t;
            foreach(var i in c)
            {
                CnAd cn = new CnAd(i.MaCN);
                chucnang.Add(cn);
            }
        }
    }
}