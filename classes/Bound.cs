using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
        }

        public Bound Copy()
        {
            var b = new Bound(this.Base) { Operations_list = new List<Operation>(this.Operations_list) };
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
        #endregion

        #region OperationsList
        //искать в истории
        public List<Operation> Operations_list { get; set; }

        //protected List<Operation> Operations_for_now { get; set; }
        //protected List<Operation> Operations_for_sold { get; set; }

        //sell buy coupon taxcoupon partrepayment brokercomission
        //private List<Operation> _sell_list;
        //private List<Operation> _buy_list;
        //private List<Operation> _coupon_list;
        //private List<Operation> _coupon_tax_list;
        //private List<Operation> _partrepayment_list;
        //private List<Operation> _brokercomission_list;
        //private List<Operation> _repayment_list;
        /*private void Operations_listChanged()
        {
            //_sell_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.Sell && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            //_buy_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.Buy && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            //_coupon_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.Coupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            //_coupon_tax_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.TaxCoupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            //_partrepayment_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.PartRepayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            //_brokercomission_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.BrokerCommission && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            //_repayment_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.Repayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();

            //if (_sell_list.Count() != 0)
            //{
            //    var bbuy = new List<Operation>();
            //    foreach (var item in _buy_list)
            //    {
            //        for (int i = 0; i < item.Quantity; i++)
            //        {
            //            bbuy.Add(new Operation(item.Id, OperationStatus.Done, new List<Trade>(), new MoneyAmount(Currency.Rub, item.Commission.Value / item.Trades.Sum(t => t.Quantity)), Currency.Rub,
            //                item.Payment / item.Trades.Sum(t => t.Quantity), item.Price, 1, null, item.Figi, item.InstrumentType, false, item.Date, item.OperationType));
            //        }
            //    }
            //    Operations_for_sold = new List<Operation>(bbuy);
            //    foreach (var item in _sell_list)
            //    {
            //        int sold_cnt = item.Trades.Sum(t => t.Quantity);
            //        //Diff_sell += item.Payment - Math.Abs(bbuy.Take(sold_cnt).Sum(t => t.Payment));
            //        bbuy.RemoveRange(0, sold_cnt);
            //    }

            //    var cpn = _coupon_list;
            //    var cpn_tax = _coupon_tax_list;
            //    //var selected = cpn.Where().ToList();
            //    //берем диапазаон от первой покупки до последней продажи на кол-во продаж (из всех покупок)
            //    Operations_for_now = Operations_list.Where(t => t.Date > _sell_list.Last().Date).ToList();
            //    Operations_for_sold = Operations_list.Where(t => t.Date <= _sell_list.Last().Date.AddDays(14) && t.Date >= Operations_for_sold.First().Date).ToList();//.Take(Cnt_sell).ToList();

                //    //считаем купоны за этот период
                //    Coupon_sell = Math.Abs(cpn.Where(t => t.Date <= _sell_list.Last().Date.AddDays(14) && t.Date >= s.First().Date).Sum(t => t.Payment));// 14 дней для дохода купонов после продажи
                //    //считаем комисиию за этот период
                //    Coupon_sell_tax = Math.Abs(cpn_tax.Where(t => t.Date <= _sell_list.Last().Date.AddDays(14) && t.Date >= s.First().Date).Sum(t => t.Payment));


                //    список всех покупок, всех продаж.
                //    берем диапазаон от первой покупки до последней продажи на кол-во продаж (из всех покупок)
                //    чтобы осталось то что было куплено но не продано
                //    считаем купоны за этот период
                //    вычитаем это из листа операций. составляем список операций для актуальных бумаг!! 
                //    тоесть кол-во бумаг по факту (base.lots) остается Cnt_sell = кол-во проданных
                //    а остальное вырезается 
                //    (изменить для закрытых позиций в LoadAllBndHistory кол-во на 0) использовать другой счетчик 
                //    

                //}
            //}
        }
    */
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



        //public bool Simplify { get; set; }

        //#region NoTinkoffInfo
        ////искать в инфо о бумаге



        //public DateTime? Last_sell_dt
        //{
        //    get
        //    {
        //        var b = _sell_list;
        //        if (b.Count() < 1)
        //            return null;
        //        return b.OrderBy(t => t.Date).Select(t => t.Date).Last();
        //    }
        //}



        /*
        //public decimal Last_Coupon_payed
        //{
        //    get
        //    {
        //        var buf = _coupon_list;
        //        if (buf.Count > 0)
        //        {
        //            return buf.Last().Payment;
        //        }
        //        return 0;
        //    }
        //}
        //public decimal Coupon_summ
        //{
        //    get
        //    {
        //        var buf = _coupon_list;
        //        if (buf.Count > 0)
        //        {
        //            return buf.Sum(t => t.Payment);
        //        }
        //        return 0;
        //    }
        //}
        //public decimal Coupon_Tax_summ
        //{
        //    get
        //    {
        //        var buf = _coupon_tax_list;
        //        if (buf.Count > 0)
        //        {
        //            return Math.Abs(buf.Sum(t => t.Payment));
        //        }
        //        return 0;
        //    }
        //}
        //public decimal Buy_Back_summ
        //{
        //    get
        //    {
        //        var buf = _partrepayment_list;
        //        if (buf.Count > 0)
        //        {
        //            return Math.Abs(buf.Sum(t => t.Payment));
        //        }
        //        return 0;
        //    }
        //}
        */

        ////без нкд
        //public decimal Coupon_sell { get; set; }
        //public decimal Coupon_sell_tax { get; set; }
        //public decimal Summ_Sell
        //{
        //    get
        //    {
        //        return _sell_list.Sum(t => t.Payment);
        //    }
        //}

        //private decimal _diff_sell;

        //public decimal Diff_sell
        //{//это разница по всем сделкам на все кол-во
        //    get
        //    {

        //        return _diff_sell;
        //    }
        //    set
        //    {
        //        _diff_sell = value;
        //    }
        //}
        //public int Cnt_sell
        //{
        //    get
        //    {
        //        var b = _sell_list;
        //        return b.Select(t => t.Trades).Sum(t => t.Select(t => t.Quantity).Sum());
        //    }
        //}
        //public decimal Profit_sell
        //{
        //    get
        //    {
        //        //сумма купонов за период владения - их налог + разница в продаже
        //        var a = Diff_sell;
        //        var c = Coupon_sell - Coupon_sell_tax;
        //        return a + c + Buy_Back_summ;

        //    }
        //}
        //public decimal Profit_sell_prc
        //{
        //    get
        //    {
        //        if (Profit_sell == 0)
        //            return 0;
        //        return ((Profit_sell * 100) / Price_now_total_avg);
        //    }
        //}
        //public string Profit_sell_perc_string
        //{
        //    get
        //    {
        //        if (Profit_sell_prc > 0)
        //            return string.Format("+{0:#0.0}", Math.Abs(Profit_sell_prc));
        //        else if (Profit_sell_prc < 0)
        //            return string.Format("-{0:#0.0}", Math.Abs(Profit_sell_prc));
        //        else
        //            return string.Format("{0:#0.0}", Math.Abs(Profit_sell_prc));
        //    }
        //}
        //public string Profit_sell_string
        //{
        //    get
        //    {
        //        if (Profit_sell > 0)
        //            return string.Format("+{0:#0.00}", Math.Abs(Profit_sell));
        //        else if (Profit_sell < 0)
        //            return string.Format("-{0:#0.00}", Math.Abs(Profit_sell));
        //        else
        //            return string.Format("{0:#0.00}", Math.Abs(Profit_sell));
        //    }
        //}
        //public Color Font_sell_profit_clr
        //{
        //    get
        //    {
        //        return Profit_sell >= 0 ? Color.DarkGreen : Color.DarkRed;
        //    }
        //}


        //public decimal Profit_summ
        //{
        //    get
        //    {
        //        if (Simplify)
        //        {
        //            var buf = _sell_list.Sum(t => t.Payment);
        //            var bb = Coupon_summ - Coupon_Tax_summ + buf - Price_now_total_avg - Nkd_sum + Buy_Back_summ;
        //            return bb;
        //        }
        //        else
        //        {
        //            return Coupon_summ - Coupon_Tax_summ + Price_now_total_market + Diff_sell - Price_now_total_avg - Nkd_sum + Buy_Back_summ;
        //        }

        //    }
        //}
        //public decimal Profit_summ_perc
        //{
        //    get
        //    {
        //        //if (Simplify)
        //        //{

        //        //var buf = _sell_list.Sum(t => t.Payment);
        //        var bb = ((Profit_summ * 100) / Price_now_total_avg);// - 100;//(((Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (buf - Price_now_total_avg)) * 100) / Price_now_total_avg) - 100;
        //        return bb;
        //        //}
        //        //else
        //        //{
        //        //return (((Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (Price_now_total_market - Price_now_total_avg) + Diff_sell) * 100) / Price_now_total_avg) - 100;
        //        //}

        //    }
        //}
        //public int Coupon_Cnt_summ
        //{
        //    get
        //    {
        //        return _coupon_list.Count;
        //    }
        //}
        //public string Profit_summ_perc_string
        //{
        //    get
        //    {
        //        if (Profit_summ_perc > 0)
        //            return string.Format("+{0:#0.0}", Math.Abs(Profit_summ_perc));
        //        else if (Profit_summ_perc < 0)
        //            return string.Format("-{0:#0.0}", Math.Abs(Profit_summ_perc));
        //        else
        //            return string.Format("{0:#0.0}", Math.Abs(Profit_summ_perc));
        //    }
        //}
        //public string Profit_summ_string
        //{
        //    get
        //    {
        //        if (Profit_summ > 0)
        //            return string.Format("+{0:#0.00}", Math.Abs(Profit_summ));
        //        else if (Profit_summ < 0)
        //            return string.Format("-{0:#0.00}", Math.Abs(Profit_summ));
        //        else
        //            return string.Format("{0:#0.00}", Math.Abs(Profit_summ));
        //    }
        //}
        //public decimal Diff_price_one
        //{
        //    get
        //    {
        //        return Price_now_one_market - Price_now_one_avg;
        //    }
        //}
        //public string Diff_price_one_string
        //{
        //    get
        //    {
        //        if (Diff_price_one > 0)
        //            return string.Format("+{0:#0.0}", Math.Abs(Diff_price_one));
        //        else if (Diff_price_one < 0)
        //            return string.Format("-{0:#0.0}", Math.Abs(Diff_price_one));
        //        else
        //            return string.Format("{0:#0.0}", Math.Abs(Diff_price_one));
        //    }
        //}
        //public decimal Diff_price
        //{
        //    get
        //    {
        //        return Price_now_total_market - Price_now_total_avg;
        //    }
        //}
        //public string Diff_price_string
        //{
        //    get
        //    {
        //        if (Diff_price > 0)
        //            return string.Format("+{0:#0.0}", Math.Abs(Diff_price));
        //        else if (Diff_price < 0)
        //            return string.Format("-{0:#0.0}", Math.Abs(Diff_price));
        //        else
        //            return string.Format("{0:#0.0}", Math.Abs(Diff_price));
        //    }
        //}
        //public Color Font_diff_clr
        //{
        //    get
        //    {
        //        return Diff_price >= 0 ? Color.DarkGreen : Color.DarkRed;
        //    }
        //}
        //public Color Font_profit_clr
        //{
        //    get
        //    {
        //        return Profit_summ >= 0 ? Color.DarkGreen : Color.DarkRed;
        //    }
        //}
        //public decimal Comission_total_summ
        //{
        //    get
        //    {
        //        var buf = _brokercomission_list;
        //        if (buf.Count > 0)
        //        {
        //            return Math.Abs(buf.Sum(t => Math.Abs(t.Payment)));
        //        }
        //        return 0;
        //    }
        //}

        //public decimal ROI //периодику считать?
        //{
        //    get
        //    {
        //        var o = (Price_now_one_market - Price_now_one_avg) * Base.Lots + Buy_Back_summ + Coupon_summ - Coupon_Tax_summ - Comission_total_summ;
        //        o = (o / (Price_now_total_avg) * 100);
        //        return o;
        //    }
        //}
        //public decimal Total_payed { get { return Payed_cpn_list.Sum(); } }
        //public int Total_payed_cnt { get; set; }
        //public decimal Total_cash_back { get; set; }

        //#endregion  
    }
}
