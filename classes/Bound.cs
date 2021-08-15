﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
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
            Simplify = false;
        }
        public bool Simplify { get; set; }
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
            Img_color = b != null ? ColorTranslator.FromHtml(b.Value<string>()) : ColorTranslator.FromHtml("#1E90FF");

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
                var b = Base.ExpectedYield.Value != 0 ? (Base.ExpectedYield.Value / Base.Lots) : 0;
                return Base.AveragePositionPriceNoNkd.Value + b;
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
                if (Simplify)
                {

                    var buf = Payed_cpn_list.Where(t => t.OperationType == ExtendedOperationType.Sell && t.Status == OperationStatus.Done).Sum(t => t.Payment);
                    var bb = (Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (buf - Price_now_total_avg)) - Price_now_total_avg;
                    return bb;
                }
                else
                    return (Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (Price_now_total_market - Price_now_total_avg)) - Price_now_total_avg;
            }
        }
        public decimal Profit_summ_perc
        {
            get
            {
                if (Simplify)
                {

                    var buf = Payed_cpn_list.Where(t => t.OperationType == ExtendedOperationType.Sell && t.Status == OperationStatus.Done).Sum(t => t.Payment);
                    var bb = (((Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (buf - Price_now_total_avg)) * 100) / Price_now_total_avg) - 100;
                    return bb;
                }
                else
                    return (((Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (Price_now_total_market - Price_now_total_avg)) * 100) / Price_now_total_avg) - 100;
            }
        }
        public int Coupon_Cnt_summ
        {
            get
            {
                return Payed_cpn_list.Count;
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
        public decimal Diff_price_one
        {
            get
            {
                return Price_now_one_market - Price_now_one_avg;
            }
        }
        public string Diff_price_one_string
        {
            get
            {
                if (Diff_price_one > 0)
                    return string.Format("+{0:#0.0}", Math.Abs(Diff_price_one));
                else if (Diff_price_one < 0)
                    return string.Format("-{0:#0.0}", Math.Abs(Diff_price_one));
                else
                    return string.Format("{0:#0.0}", Math.Abs(Diff_price_one));
            }
        }
        public decimal Diff_price
        {
            get
            {
                return Price_now_total_market - Price_now_total_avg;
            }
        }
        public string Diff_price_string
        {
            get
            {
                if (Diff_price > 0)
                    return string.Format("+{0:#0.0}", Math.Abs(Diff_price));
                else if (Diff_price < 0)
                    return string.Format("-{0:#0.0}", Math.Abs(Diff_price));
                else
                    return string.Format("{0:#0.0}", Math.Abs(Diff_price));
            }
        }
        public Color Font_diff_clr
        {
            get
            {
                return Diff_price >= 0 ? Color.DarkGreen : Color.DarkRed;
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
