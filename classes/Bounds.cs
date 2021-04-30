using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
    public class Bounds : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Bounds(Account acc)
        {
            BoundsList = new List<Bound>();
            Acc = acc;
        }
        public List<Bound> BoundsList { get; set; }
        public Account Acc { get; set; }

        public decimal SumB_Coast
        {
            get
            {
                return BoundsList.Sum(t => t.Price_now_total_avg);
            }
        }
        public decimal SumB_Coupons
        {
            get
            {
                return BoundsList.Sum(t => t.Coupon_summ);
            }
        }
        /*
        #region SUMMINFO
        public decimal SumB_Coast
        {
            get
            {
                return BoundsList.Sum(t => t.Price_now_one_market);
            }
        }
        public decimal SumB_Buy
        {
            get
            {
                return BoundsList.Sum(t => t.Base.AveragePositionPriceNoNkd.Value);
            }
        }
        public decimal SumB_Coupons_cnt
        {
            get
            {
                return BoundsList.Sum(t => t.Total_payed_cnt);
            }
        }
        public decimal SumB_CashBack
        {
            get
            {
                return BoundsList.Sum(t => t.Total_cash_back);
            }
        }
        public decimal SumB_Coupons_profit
        {
            get
            {
                return BoundsList.Sum(t => t.Total_payed);
            }
        }
        public decimal SumB_TOTAL
        {
            get
            {
                return SumB_Coast + SumB_Coupons_profit - SumB_Buy;
            }
        }
        #endregion
        */
    }
    public class Bound : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Tinkoff Position Base Class
        /// </summary>
        public Portfolio.Position Base { get; set; }
        public Bound(Portfolio.Position ps)
        {
            Base = ps;
            Nominal = 0m;
            Next_pay_dt = new DateTime();
            End_pay_dt = new DateTime();
            Pay_period = 0;
            Cpn_val = 0m;
            Cpn_Percent = 0m;
            Payed_cpn_list = new List<Operation>();
            //Total_payed_cnt = 0;
            //Total_cash_back = 0m;
        }
        //https://www.tinkoff.ru/api/trading/bonds/get?ticker=RU000A1005T9
        public string Img_path { get; private set; }
        public Image Img_exct { get; private set; }
        public Color Img_color { get; private set; }
        public Color Text_color { get; private set; }
        public void LoadImagePath()
        {
            xWebClient wx = new xWebClient();
            wx.DownloadStringCompleted += Wx_DownloadStringCompleted;
            wx.DownloadStringAsync(new Uri($"https://www.tinkoff.ru/api/trading/bonds/get?ticker={Base.Ticker}"));
        }

        private void Wx_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
                var data = (JObject)JsonConvert.DeserializeObject(e.Result);
                var b = data["payload"]["symbol"]["logoName"];
                Img_path = b != null ? $"https://static.tinkoff.ru/brands/traiding/{b.Value<string>().Replace(".png", "")}x160.png" : "";
                b = data["payload"]["symbol"]["color"];
                Img_color = b != null ? ColorTranslator.FromHtml(b.Value<string>()) : ColorTranslator.FromHtml("#FFFFFF");

                b = data["payload"]["symbol"]["textColor"];
                Text_color = b != null ? ColorTranslator.FromHtml(b.Value<string>()) : ColorTranslator.FromHtml("#FFFFFF");
                DrawImg();
        }

        private void DrawImg()
        {
            if (!string.IsNullOrEmpty(Img_path))
            {
                xWebClient wx = new xWebClient();
                wx.OpenReadCompleted += Wx_OpenReadCompleted;
                wx.OpenReadAsync(new Uri(Img_path));
            }
            else
                Img_exct = Program.DrawText(Base.Name.Substring(0, 1), new Font("Arial", 36), Text_color, Img_color);
        }

        private void Wx_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {
                Img_exct = Image.FromStream(e.Result);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("404"))
                    Img_path = "";
                    DrawImg();
            }

        }

        #region NoNKD
        public decimal Price_now_one_market
        {
            get
            {
                return Base.AveragePositionPriceNoNkd.Value + (Base.ExpectedYield.Value / Base.Lots);
            }
        }
        public decimal Price_now_total_market
        {
            get
            {
                return Price_now_one_market * Base.Lots;
            }
        }
        public decimal Price_now_one_avg
        {
            get
            {
                return Base.AveragePositionPriceNoNkd.Value;
            }
        }
        public decimal Price_now_total_avg
        {
            get
            {
                return Price_now_one_avg * Base.Lots;
            }
        }
        public decimal Nkd_one
        {
            get
            {
                return Base.AveragePositionPrice.Value - Base.AveragePositionPriceNoNkd.Value;
            }
        }
        public decimal Nkd_sum
        {
            get
            {
                return (Base.AveragePositionPrice.Value - Base.AveragePositionPriceNoNkd.Value) * Base.Lots;
            }
        }
        #endregion
        #region NoTinkoffInfo
        //искать в инфо о бумаге
        public decimal Nominal { get; set; }
        public DateTime Prev_pay_dt { get { return Next_pay_dt.AddDays(-Pay_period); } }
        public DateTime Next_pay_dt { get; set; }
        public DateTime End_pay_dt { get; set; }
        public int Pay_period { get; set; }
        public decimal Cpn_val { get; set; }
        public decimal Cpn_Percent { get; set; }

        private void ForDebug()
        {
            var a = Payed_cpn_list.Where(t=>t.Status == OperationStatus.Done).Select(t => t.Date).ToList();
            var b = Payed_cpn_list.Where(t => t.Status == OperationStatus.Done).Select(t => t.OperationType).ToList();
            var c = Payed_cpn_list.Where(t => t.Status == OperationStatus.Done).Select(t => t.Payment).ToList();
            var e = Payed_cpn_list.Where(t => t.Status == OperationStatus.Done).Select(t => t.Price).ToList();
            var g = Payed_cpn_list.Where(t => t.Status == OperationStatus.Done).Select(t => t.Trades).ToList();
            
            Debug.WriteLine($"");
        }

        //искать в истории
        public List<Operation> Payed_cpn_list { get; set; }

        public decimal Last_Coupon_payed
        {
            get
            {
                var buf = Payed_cpn_list.Where(t => t.OperationType == ExtendedOperationType.Coupon).ToList();
                if (buf.Count > 0)
                {
                    return buf.First().Payment;
                }
                return 0;
            }
        }

        //public decimal Last_Coupon_payed_perc
        //{
        //    get
        //    {
        //        var buf = Payed_cpn_list.Where(t => t.OperationType == ExtendedOperationType.Coupon).ToList();
        //        if (buf.Count > 0)
        //        {
        //            var o = buf.First().Payment/Base.Lots / Nominal * 100 * Math.Ceiling(365m / Pay_period); //меняется сумма при покупках надо это чекать
        //            return o;/// buf.Last(); //?
        //        }
        //        return 0;
        //    }
        //}
        public decimal Coupon_summ
        {
            get
            {
                var buf = Payed_cpn_list.Where(t => t.OperationType == ExtendedOperationType.Coupon).ToList();
                if (buf.Count > 0)
                {
                    return buf.Sum(t => t.Payment);
                }
                return 0;
            }
        }
        public decimal Coupon_Tax_summ
        {
            get
            {
                var buf = Payed_cpn_list.Where(t => t.OperationType == ExtendedOperationType.TaxCoupon).ToList();
                if (buf.Count > 0)
                {
                    return Math.Abs(buf.Sum(t => t.Payment));
                }
                return 0;
            }
        }
        public decimal Buy_Back_summ
        {
            get
            {
                ForDebug();
                var buf = Payed_cpn_list.Where(t => t.OperationType == ExtendedOperationType.PartRepayment).ToList();
                if (buf.Count > 0)
                {
                    return Math.Abs(buf.Sum(t => t.Payment));
                }
                return 0;
            }
        }
        //без нкд
        public decimal Profit_summ
        {
            get
            {
                return (Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (Price_now_total_market - Price_now_total_avg)) - Price_now_total_avg;
            }
        }
        public decimal Profit_summ_perc
        {
            get
            {
                var o = (((Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (Price_now_total_market - Price_now_total_avg)) * 100) / Price_now_total_avg) - 100;
                return o;
            }
        }
        public string Profit_summ_perc_string
        {
            get
            {
                if (Profit_summ_perc > 0)
                    return string.Format("+{0:#0.0}", Math.Abs(Profit_summ_perc));
                else if (Profit_summ_perc < 0)
                    return string.Format("-{0:#0.0}", Math.Abs(Profit_summ_perc));
                else
                    return string.Format("{0:#0.0}", Math.Abs(Profit_summ_perc));
            }
        }
        public Color Font_profit_clr
        {
            get
            {
                return Profit_summ >= 0 ? Color.DarkGreen : Color.DarkRed;
            }
        }

        public decimal Comission_total_summ
        {
            get
            {
                var buf = Payed_cpn_list.Where(t => t.OperationType == ExtendedOperationType.BrokerCommission).ToList();
                if (buf.Count > 0)
                {
                    return Math.Abs(buf.Sum(t => Math.Abs(t.Payment)));
                }
                return 0;
            }
        }

        public decimal ROI //периодику считать?
        {
            get
            {
                var o = (Price_now_one_market - Price_now_one_avg) * Base.Lots + Buy_Back_summ + Coupon_summ - Coupon_Tax_summ - Comission_total_summ;
                o = (o / (Price_now_total_avg) * 100);
                return o;
            }
        }
        //public decimal Total_payed { get { return Payed_cpn_list.Sum(); } }
        //public int Total_payed_cnt { get; set; }
        //public decimal Total_cash_back { get; set; }
        #endregion  
    }
}
