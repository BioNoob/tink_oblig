using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

        public int Bnd_Cnt
        {
            get
            {
                return BoundsList.Sum(t => t.Base.Lots);
            }
        }
        public decimal Total_diff_price
        {
            get
            {
                return BoundsList.Sum(t => t.Price_now_total_market - t.Price_now_total_avg);
            }
        }
        public int Cpn_Cnt
        {
            get
            {
                return BoundsList.Sum(t => t.Coupon_Cnt_summ);
            }
        }
        public decimal Cpn_Taxes
        {
            get
            {
                return BoundsList.Sum(t => t.Coupon_Tax_summ);
            }
        }
        public decimal Sum_Profit
        {
            get
            {
                return BoundsList.Sum(t => t.Profit_summ);
            }
        }
        public decimal Sum_Profit_Perc //WRONG
        {
            get
            {
                //return (((Price_now_total_avg + Coupon_summ - Coupon_Tax_summ + (Price_now_total_market - Price_now_total_avg)) * 100) / Price_now_total_avg) - 100;
                return (((SumB_Coast + SumB_Coupons - Cpn_Taxes + (BoundsList.Sum(t => t.Price_now_total_market) - SumB_Coast)) * 100) / SumB_Coast) - 100;
            }
        }

        public string Profit_Summ_Perc_String
        {
            get
            {
                if (Sum_Profit_Perc > 0)
                    return string.Format("+{0:#0.0}", Math.Abs(Sum_Profit_Perc));
                else if (Sum_Profit_Perc < 0)
                    return string.Format("-{0:#0.0}", Math.Abs(Sum_Profit_Perc));
                else
                    return string.Format("{0:#0.0}", Math.Abs(Sum_Profit_Perc));
            }
        }
        public string Total_Diff_String
        {
            get
            {
                if (Total_diff_price > 0)
                    return string.Format("+{0:#0.0}", Math.Abs(Total_diff_price));
                else if (Total_diff_price < 0)
                    return string.Format("-{0:#0.0}", Math.Abs(Total_diff_price));
                else
                    return string.Format("{0:#0.0}", Math.Abs(Total_diff_price));
            }
        }
        public Color Font_Profit_Clr
        {
            get
            {
                return Sum_Profit >= 0 ? Color.DarkGreen : Color.DarkRed;
            }
        }
        public Color Font_Diff_Clr
        {
            get
            {
                return Total_diff_price >= 0 ? Color.DarkGreen : Color.DarkRed;
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
}
