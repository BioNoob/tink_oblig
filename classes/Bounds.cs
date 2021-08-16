using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;
using static tink_oblig.classes.Accounts;

namespace tink_oblig.classes
{
    public class Bounds : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Bounds(Account_m acc)
        {
            _boundslist = new List<Bound>();
            Acc = acc;
        }
        public Bounds Copy ()
        {
            var b =  new Bounds(this.Acc) { BoundsList = new List<Bound>(this.BoundsList) };
            return b;
        }
        private List<Bound> _boundslist;
        public List<Bound> BoundsList
        {
            get
            {
                return _boundslist;
            }
            set
            {
                _boundslist = value; 
            }
        }
        public Account_m Acc { get; set; }
        public string Acc_Type
        {
            get
            {
                switch (Acc.BrokerAccountType)
                {
                    case BrokerAccountType.Tinkoff:
                        return "Брокерский";
                    case BrokerAccountType.TinkoffIis:
                        return "ИИС";
                    default:
                        return "Неопознаный мамонт";
                }
            }
        }
        public decimal SumB_Coast
        {
            get
            {
                if (BoundsList.Count != 0)
                    return BoundsList.Sum(t => t.Price_now_total_avg);
                else
                    return 0;
            }
        }
        public decimal SumB_Coupons
        {
            get
            {
                if (BoundsList.Count != 0)
                    return BoundsList.Sum(t => t.Coupon_summ);
                else
                    return 0;
            }
        }

        public int Bnd_Cnt
        {
            get
            {
                if (BoundsList.Count != 0)
                    return BoundsList.Sum(t => t.Base.Lots);
                else
                    return 0;
            }
        }
        public decimal Total_diff_price
        {
            get
            {
                if (BoundsList.Count != 0)
                    return BoundsList.Sum(t => t.Price_now_total_market - t.Price_now_total_avg);
                else
                    return 0;
            }
        }
        public int Cpn_Cnt
        {
            get
            {
                if (BoundsList.Count != 0)
                    return BoundsList.Sum(t => t.Coupon_Cnt_summ);
                else
                    return 0;
            }
        }
        public decimal Cpn_Taxes
        {
            get
            {
                if (BoundsList.Count != 0)
                    return BoundsList.Sum(t => t.Coupon_Tax_summ);
                else
                    return 0;
            }
        }
        public decimal Sum_Profit
        {
            get
            {
                if (BoundsList.Count != 0)
                    return BoundsList.Sum(t => t.Profit_summ);
                else
                    return 0;
            }
        }
        public decimal Sum_Profit_Perc //WRONG
        {
            get
            {
                if (BoundsList.Count != 0)
                    return (((SumB_Coast + SumB_Coupons - Cpn_Taxes + (BoundsList.Sum(t => t.Price_now_total_market) - SumB_Coast)) * 100) / SumB_Coast) - 100;
                else
                    return 0;
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
        public decimal Buy_back
        {
            get
            {
                return BoundsList.Sum(t => t.Buy_Back_summ);
            }
        }
    }
}
