using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webfinal.Models;

namespace webfinal.Controllers
{
    public class GioHangController : Controller
    {
        //
        // GET: /GioHang/
        QLNHDataContext data = new QLNHDataContext();
        public List<giohang> laygiohang()
        {
            List<giohang> lstGioHang = Session["Giohang"] as List<giohang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<giohang>();
                Session["Giohang"] = lstGioHang;
            }
            Session["SoLuong"] = tongsl();
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int iMANH, String strURL, int sl)
        {
            List<giohang> lstGiohang = laygiohang();
            giohang sanpham = lstGiohang.Find(n => n.iMANH == iMANH);
            if (sanpham == null)
            {
                sanpham = new giohang(iMANH, sl);
                lstGiohang.Add(sanpham);
                Session["SoLuong"] = tongsl();
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoLuong = sanpham.iSoLuong + sl;
                Session["SoLuong"] = tongsl();
                return Redirect(strURL);
            }
        }
        private int tongsl()
        {
            int sl = 0;
            List<giohang> lstGioHang = Session["Giohang"] as List<giohang>;
            if (lstGioHang != null)
            {
                sl = lstGioHang.Sum(n => n.iSoLuong);
            }
            return sl;
        }
        private double tongtien()
        {
            double tong = 0;
            List<giohang> lstGioHang = Session["Giohang"] as List<giohang>;
            if (lstGioHang != null)
            {
                tong = lstGioHang.Sum(n => n.iThanhTie);

            }
            return tong;
        }
        public ActionResult Onepay()
        {

            double t = tongtien();

            string amount = (t * 100).ToString();
            string url = RedirectOnepay("nuochoa T&N", amount, "192.186.0.1");
            return Redirect(url);
        }
        public string RedirectOnepay(string codeInvoice, string amount, string ip)
        {
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnePayProperty.Url_ONEPAY_TEST);
            conn.SetSecureSecret(OnePayProperty.HASH_CODE);

            // Gan cac thong so de truyen sang cong thanh toan onepay
            conn.AddDigitalOrderField("AgainLink", OnePayProperty.AGAIN_LINK);
            conn.AddDigitalOrderField("Title", "Tich hop onepay vao web asp.net mvc 5");
            conn.AddDigitalOrderField("vpc_Locale", OnePayProperty.PAYGATE_LANGUAGE);
            conn.AddDigitalOrderField("vpc_Version", OnePayProperty.VERSION);
            conn.AddDigitalOrderField("vpc_Command", OnePayProperty.COMMAND);
            conn.AddDigitalOrderField("vpc_Merchant", OnePayProperty.MERCHANT_ID);
            conn.AddDigitalOrderField("vpc_AccessCode", OnePayProperty.ACCESS_CODE);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", "nuochoa T&N");
            conn.AddDigitalOrderField("vpc_OrderInfo", codeInvoice);
            conn.AddDigitalOrderField("vpc_Amount", amount);
            conn.AddDigitalOrderField("vpc_ReturnURL", Url.Action("OnepayResponse", "TrangChu", null, Request.Url.Scheme, null));

            // Thong tin them ve khach hang. De trong neu khong co thong tin
            conn.AddDigitalOrderField("vpc_SHIP_Street01", "");
            conn.AddDigitalOrderField("vpc_SHIP_Provice", "");
            conn.AddDigitalOrderField("vpc_SHIP_City", "");
            conn.AddDigitalOrderField("vpc_SHIP_Country", "");
            conn.AddDigitalOrderField("vpc_Customer_Phone", "");
            conn.AddDigitalOrderField("vpc_Customer_Email", "");
            conn.AddDigitalOrderField("vpc_Customer_Id", "");
            conn.AddDigitalOrderField("vpc_TicketNo", ip);

            string url = conn.Create3PartyQueryString();
            return url;
        }
        public ActionResult OnepayResponse()
        {
            string hashvalidateResult = "";

            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest(OnePayProperty.Url_ONEPAY_TEST);
            conn.SetSecureSecret(OnePayProperty.HASH_CODE);

            // Xu ly tham so tra ve va du lieu ma hoa
            hashvalidateResult = conn.Process3PartyResponse(Request.QueryString);

            // Lay tham so tra ve tu cong thanh toan
            string vpc_TxnResponseCode = conn.GetResultField("vpc_TxnResponseCode");
            string amount = conn.GetResultField("vpc_Amount");
            string localed = conn.GetResultField("vpc_Locale");
            string command = conn.GetResultField("vpc_Command");
            string version = conn.GetResultField("vpc_Version");
            string cardType = conn.GetResultField("vpc_Card");
            string orderInfo = conn.GetResultField("vpc_OrderInfo");
            string merchantID = conn.GetResultField("vpc_Merchant");
            string authorizeID = conn.GetResultField("vpc_AuthorizeId");
            string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef");
            string transactionNo = conn.GetResultField("vpc_TransactionNo");
            string acqResponseCode = conn.GetResultField("vpc_AcqResponseCode");
            string txnResponseCode = vpc_TxnResponseCode;
            string message = conn.GetResultField("vpc_Message");

            // Kiem tra 2 tham so tra ve quan trong nhat
            if (hashvalidateResult == "CORRECTED" && txnResponseCode.Trim() == "0")
            {
                return View("PaySuccess");
            }
            else if (hashvalidateResult == "INVALIDATED" && txnResponseCode.Trim() == "0")
            {
                return View("PayPending");
            }
            else
            {
                return View("PayUnSuccess");
            }
        }
        public ActionResult Giohang()
        {
            List<giohang> lstGioHang = laygiohang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            ViewBag.Tongsoluong = tongsl();
            ViewBag.Tongtien = tongtien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult CapNhap(int id,FormCollection collection)
        {
            List<giohang> lstGioHang = laygiohang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "TrangChu");
            }
            ViewBag.Tongsoluong = tongsl();
            ViewBag.Tongtien = tongtien();
            int ls = int.Parse(collection["soluong"].ToString());
            if(ls==0)
            {
                int i = 0;
                while(i< lstGioHang.Count())
                {
                    if(lstGioHang[i].iMANH==id)
                    {
                        lstGioHang.Remove(lstGioHang[i]);
                        return RedirectToAction("Giohang", "GioHang");
                    }
                    i++;
                }
                return RedirectToAction("Giohang","GioHang");
            }
            else
            {
                int i = 0;
                while (i < lstGioHang.Count())
                {
                    if (lstGioHang[i].iMANH == id)
                    {
                        lstGioHang[i].iSoLuong = ls;
                        return RedirectToAction("Giohang", "GioHang");
                    }
                    i++;
                }
                return RedirectToAction("Giohang", "GioHang");
            }
        }
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null)
            {
                ViewBag.Loi = "hãy đang nhập để đặt hàng";
                return RedirectToAction("Giohang", "GioHang");
            }
            else
            {
                List<giohang> lstGioHang = laygiohang();
                ViewBag.Tongsoluong = tongsl();
                ViewBag.Tongtien = tongtien();
                return View(lstGioHang);
            }
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<giohang> lstGioHang = laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            if(ngaygiao=="")
            {
                
                ViewBag.Tongsoluong = tongsl();
                ViewBag.Tongtien = tongtien();
                ViewBag.Loi = "Nhập ngày giao hàng";
                return View(lstGioHang);
            }
            if (DateTime.Parse(ngaygiao) <= DateTime.Now)
            {
                ViewBag.Tongsoluong = tongsl();
                ViewBag.Tongtien = tongtien();
                ViewBag.Loi = "Ngày giao phải lớn hơn ngày hiện tại và không quá 20 ngày";
                return View(lstGioHang);
            }
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (var item in lstGioHang)
            {
                CTDDH ct = new CTDDH();
                ct.MADDH = ddh.MADDH;
                ct.MANH = item.iMANH;
                ct.SoLuong = item.iSoLuong;
                ct.DonGia = (decimal)item.iGia;
                data.CTDDHs.InsertOnSubmit(ct);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("XacNhan", "GioHang");
        }
        public ActionResult XacNhan()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
	}
}