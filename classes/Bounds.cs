using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
    public class Bounds : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Bounds()
        {
            BoundsList = new List<Bound>();
        }
        public List<Bound> BoundsList { get; set; }
        #region SUMMINFO
        public decimal SumB_Coast
        {
            get
            {
                return BoundsList.Sum(t => t.Price_now_total);
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
            Payed_cpn_list = new List<decimal>();
            Total_payed_cnt = 0;
            Total_cash_back = 0m;
        }
        //https://www.tinkoff.ru/api/trading/bonds/get?ticker=RU000A1005T9
        public string Img_path { get; private set; }
        public Image Img_exct { get; private set; }
        public Color Img_color { get; private set; }
        public Color Text_color { get; private set; }
        public async void LoadImagePath()
        {
            WebClient wx = new WebClient();
            string json = await wx.DownloadStringTaskAsync($"https://www.tinkoff.ru/api/trading/bonds/get?ticker={Base.Ticker}");
            var data = (JObject)JsonConvert.DeserializeObject(json);
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
                using (WebClient webClient = new WebClient())
                {
                    try
                    {
                        using (Stream stream = webClient.OpenRead(Img_path))
                        {
                            Img_exct = Image.FromStream(stream);
                        }
                    }
                    catch (Exception)
                    {

                        Img_path = "";
                        DrawImg();
                    }

                }
            else
                Img_exct = Program.DrawText(Base.Name.Substring(0, 1), new Font("Arial", 64), Text_color, Img_color);
        }

        #region NoNKD
        public decimal Price_now_one
        {
            get
            {
                return Base.AveragePositionPrice.Value + (Base.ExpectedYield.Value / Base.Lots);
            }
        }
        public decimal Price_now_total
        {
            get
            {
                return Price_now_one * Base.Lots;
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
        public List<decimal> Payed_cpn_list { get; set; }
        public decimal Total_payed { get { return Payed_cpn_list.Sum(); } }
        public int Total_payed_cnt { get; set; }
        public decimal Total_cash_back { get; set; }
        #endregion  
    }
}
