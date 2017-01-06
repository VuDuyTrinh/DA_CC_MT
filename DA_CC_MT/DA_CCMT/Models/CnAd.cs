using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DA_CCMT.Models
{
    public class CnAd
    {
        QLNHDataContext data = new QLNHDataContext();
        public int iMaCN { get; set; }
        public String iTenCN { get; set; }
        public String iAction { get; set; }
        public String iController { get; set; }
        public CnAd(int id)
        {
            ChucNang cn = data.ChucNangs.SingleOrDefault(n => n.MaCN == id);
            iMaCN = id;
            iTenCN = cn.TenCN;
            iAction = cn.Action;
            iController = cn.Controller;
        }
    }
}