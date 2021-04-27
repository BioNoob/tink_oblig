using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
