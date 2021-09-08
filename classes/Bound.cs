using Newtonsoft.Json;
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
            Operations_list = new List<Operation>();
            Offerts = new List<Offert>();
            Amortizations = new List<Amortization>();
        }

        public Bound Copy()
        {
            var b = new Bound(this.Base)
            {
                Operations_list = new List<Operation>(this.Operations_list),
                Next_pay_dt = this.Next_pay_dt,
                End_pay_dt = this.End_pay_dt,
                Pay_period = this.Pay_period,
                Cpn_Percent = this.Cpn_Percent,
                Cpn_val = this.Cpn_val,
                Nominal = this.Nominal,
                Offerts = this.Offerts,
                Amortizations = this.Amortizations
            };
            return b;
        }

        /// <summary>
        /// Флаг для пометки того что позиция по бумаге была закрыта и ее нет в списке портфеля 
        /// </summary>
        //https://www.tinkoff.ru/api/trading/bonds/get?ticker=RU000A1005T9


        #region IMGTHING
        public string Img_path { get; private set; }
        public Image Img_exct { get; private set; }
        public Color Img_color { get; private set; }
        public Color Text_color { get; private set; }
        ///https://www.tinkoff.ru/api/trading/symbols/brands?brandId=%D0%A4%D0%BB%D0%B0%D0%B9%20%D0%9F%D0%BB%D1%8D%D0%BD%D0%B8%D0%BD%D0%B3
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
        #endregion

        #region CouponInfo
        //даты купонов
        public DateTime Prev_pay_dt { get { return Next_pay_dt.AddDays(-Pay_period); } }
        public DateTime Next_pay_dt { get; set; }
        public DateTime End_pay_dt { get; set; }
        public int Pay_period { get; set; }
        public decimal Cpn_val { get; set; }
        public decimal Cpn_Percent { get; set; }

        public List<Offert> Offerts { get; set; }
        public List<Amortization> Amortizations { get; set; }

        public bool HaveOffert => Offerts.Count > 0;
        public bool HaveAmort => Amortizations.Count > 0;
        public Offert ClouserOffert { get { if (HaveOffert) return Offerts.First(); else return null; } }
        public Amortization ClouserAmort { get { if (HaveAmort) return Amortizations.First(); else return null; } }

        public decimal? OffertPerc { get { if (HaveOffert) return (ClouserOffert.facevalue / Nominal) * 100; else return null; } }

        #endregion

        #region OperationsList
        //искать в истории
        public List<Operation> Operations_list { get; set; }

        #endregion



        public decimal Nominal { get; set; }
        /// <summary>
        /// уплочено денег в среднем
        /// </summary>
        public decimal Avg_buy_paid_one
        {
            get
            {
                return Base.AveragePositionPrice.Value;
            }
        }
        /// <summary>
        /// фактическая стоимость покупки в среднем
        /// </summary>
        public decimal Avg_buy_price_one
        {
            get
            {
                return Base.AveragePositionPriceNoNkd.Value;
            }
        }
        /// <summary>
        /// Фактическая цена по рынку
        /// </summary>
        public decimal Market_price
        {
            get
            {
                return Avg_buy_price_one + Base.ExpectedYield.Value / Base.Lots;
            }
        }
        /// <summary>
        /// Фактическая стоимость одной в портфеле исходя из средней
        /// </summary>
        public decimal Avg_portfail_sum_one
        {
            get
            {
                var b = Base.ExpectedYield.Value != 0 ? (Base.ExpectedYield.Value / Base.Lots) : 0;
                return Avg_buy_price_one + b + Nkd_one;
            }
        }
        /// <summary>
        /// Накопленный купонный доход на одну акцию при покупке
        /// </summary>
        public decimal Nkd_one
        {
            get
            {
                return Base.AveragePositionPrice.Value - Base.AveragePositionPriceNoNkd.Value;
            }
        }
        /// <summary>
        /// Сумма нкд по покупке
        /// </summary>
        public decimal Nkd_sum
        {
            get
            {
                return Nkd_one * Base.Lots;
            }
        }
        /// <summary>
        /// Нкд рыночный
        /// </summary>
        public decimal Nkd_for_now
        {
            get
            {
                var days = (int)Math.Floor((DateTime.Now - Prev_pay_dt).TotalDays);
                return (Cpn_val / Pay_period) * days;
            }
        }
        public decimal Nkd_for_now_sum
        {
            get
            {
                var days = (int)Math.Floor((DateTime.Now - Prev_pay_dt).TotalDays);
                return ((Cpn_val / Pay_period) * days) * Base.Lots;
            }
        }
        /// <summary>
        /// Сумма уплоченных денег в среднем
        /// </summary>
        public decimal Avg_buy_paid_total
        {
            get
            {
                return Avg_buy_paid_one * Base.Balance; //ps Лоты это баланс деленый на кол-во продаваемых за раз штук
            }
        }
        /// <summary>
        /// Суммарная фактическая стоимость покупки
        /// </summary>
        public decimal Avg_buy_price_total
        {
            get
            {
                return Avg_buy_price_one * Base.Balance;
            }
        }
        /// <summary>
        /// Суммарная фактическая цена по рынку
        /// </summary>
        public decimal Market_price_total
        {
            get
            {
                return Market_price * Base.Balance;
            }
        }
        /// <summary>
        /// Суммарная фактическая стоимость облиги в порфеле
        /// </summary>
        public decimal Avg_portfail_sum_total
        {
            get
            {
                return Avg_portfail_sum_one * Base.Balance;
            }
        }

        public decimal Diff_price_sum_no_nkd
        {
            get
            {
                return Market_price_total - Avg_buy_paid_total;
            }
        }

        public decimal Diff_price_sum_nkd
        {
            get
            {
                return Market_price_total + Nkd_for_now - Avg_buy_paid_total;
            }
        }
    }

    public class Amortization
    {
        /// <summary>
        /// дата амортизации
        /// </summary>
        public DateTime amortdate { get; set; }
        /// <summary>
        /// Выплата на одну бумагу
        /// </summary>
        public decimal value { get; set; }
        /// <summary>
        /// Процент погашения
        /// </summary>
        public decimal valueprc { get; set; }
        public Amortization() { }
    }
    public class Offert
    {
        /// <summary>
        /// дата офферты
        /// </summary>
        public DateTime offerdate { get; set; }
        /// <summary>
        /// Выплата на одну бумагу
        /// </summary>
        public decimal facevalue { get; set; }
        ///// <summary>
        ///// Процент погашения
        ///// </summary>
        //public decimal valueprc { get; set; }
        public Offert() { }
    }
}
