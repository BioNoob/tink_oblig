using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;
using PropertyChanged;
using static tink_oblig.classes.Accounts;
using System.ComponentModel;

namespace tink_oblig.classes
{
    public class Bounds : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Bounds(Account_m acc)
        {
            _boundslist = new List<Bound_Conclav>();
            Acc = acc;
        }
        public Bounds Copy ()
        {
            var b =  new Bounds(this.Acc) { BoundsList = new List<Bound_Conclav>(this.BoundsList) };
            return b;
        }
        private List<Bound_Conclav> _boundslist;
        public List<Bound_Conclav> BoundsList
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
        public List<Bound_now> Bounds_Now
        {
            get
            {
                return BoundsList.Select(t => t.Bound_now).ToList();
            }
        }
        public List<Bound_sold> Bounds_Sold
        {
            get
            {
                return BoundsList.Select(t => t.Bound_sold).ToList();
            }
        }

        public SeeHistory Mode { get; set; }

        public decimal SumB_Cost
        {
            get
            {
                switch (Mode)
                {
                    case SeeHistory.NoHistrory:
                        return Bounds_Now.Sum(t => t.Summ_buy);
                    case SeeHistory.History:
                        return Bounds_Sold.Sum(t => t.Summ_sell);
                    case SeeHistory.WithHistory:
                        return Bounds_Sold.Sum(t => t.Summ_sell) + Bounds_Now.Sum(t => t.Summ_buy);
                    default:
                        return 0;
                }
            }
        }
        public decimal SumB_Coupons
        {
            get
            {
                switch (Mode)
                {
                    case SeeHistory.NoHistrory:
                        return Bounds_Now.Sum(t => t.Coupon_summ);
                    case SeeHistory.History:
                        return Bounds_Sold.Sum(t => t.Coupon_summ);
                    case SeeHistory.WithHistory:
                        return Bounds_Sold.Sum(t => t.Coupon_summ) + Bounds_Now.Sum(t => t.Coupon_summ);
                    default:
                        return 0;
                }
            }
        }

        public int Bnd_Cnt
        {
            get
            {
                switch (Mode)
                {
                    case SeeHistory.NoHistrory:
                        return Bounds_Now.Sum(t => t.Bound.Base.Lots);
                    case SeeHistory.History:
                        return Bounds_Sold.Sum(t => t.Cnt_sell);
                    case SeeHistory.WithHistory:
                        return Bounds_Sold.Sum(t => t.Cnt_sell) + Bounds_Now.Sum(t => t.Bound.Base.Lots);
                    default:
                        return 0;
                }
            }
        }
        public decimal Total_diff_price
        {
            get
            {
                switch (Mode)
                {
                    case SeeHistory.NoHistrory:
                        return Bounds_Now.Sum(t => t.Diff_Price);
                    case SeeHistory.History:
                        return Bounds_Sold.Sum(t => t.Diff_sell);
                    case SeeHistory.WithHistory:
                        return Bounds_Sold.Sum(t => t.Diff_sell) + Bounds_Now.Sum(t => t.Diff_Price);
                    default:
                        return 0;
                }
            }
        }
        public int Cpn_Cnt
        {
            get
            {
                switch (Mode)
                {
                    case SeeHistory.NoHistrory:
                        return Bounds_Now.Sum(t => t.Coupon_cnt);
                    case SeeHistory.History:
                        return Bounds_Sold.Sum(t => t.Coupon_cnt);
                    case SeeHistory.WithHistory:
                        return Bounds_Sold.Sum(t => t.Coupon_cnt) + Bounds_Now.Sum(t => t.Coupon_cnt);
                    default:
                        return 0;
                }
            }
        }
        public decimal Cpn_Taxes
        {
            get
            {
                switch (Mode)
                {
                    case SeeHistory.NoHistrory:
                        return Bounds_Now.Sum(t => t.Coupon_Tax_summ);
                    case SeeHistory.History:
                        return Bounds_Sold.Sum(t => t.Coupon_Tax_summ);
                    case SeeHistory.WithHistory:
                        return Bounds_Sold.Sum(t => t.Coupon_Tax_summ) + Bounds_Now.Sum(t => t.Coupon_Tax_summ);
                    default:
                        return 0;
                }
            }
        }
        public decimal Sum_Profit
        {
            get
            {
                switch (Mode)
                {
                    case SeeHistory.NoHistrory:
                        return Bounds_Now.Sum(t => t.Profit);
                    case SeeHistory.History:
                        return Bounds_Sold.Sum(t => t.Profit);
                    case SeeHistory.WithHistory:
                        return Bounds_Sold.Sum(t => t.Profit) + Bounds_Now.Sum(t => t.Profit);
                    default:
                        return 0;
                }
            }
        }
        public decimal Sum_Profit_Perc //WRONG
        {
            get
            {
                return (Sum_Profit * 100 / SumB_Cost);
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
                switch (Mode)
                {
                    case SeeHistory.NoHistrory:
                        return Bounds_Now.Sum(t => t.Buy_Back_summ);
                    case SeeHistory.History:
                        return Bounds_Sold.Sum(t => t.Buy_Back_summ);
                    case SeeHistory.WithHistory:
                        return Bounds_Sold.Sum(t => t.Buy_Back_summ) + Bounds_Now.Sum(t => t.Buy_Back_summ);
                    default:
                        return 0;
                }
            }
        }
    }
}
