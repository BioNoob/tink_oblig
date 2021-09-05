using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
    public class Bound_now : INotifyPropertyChanged, IComparable //Bound,
    {
        public Bound Bound { get; set; }
        public Bound_now(Bound bnd)
        {
            Bound = bnd;
            _buy_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.Buy && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _coupon_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.Coupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _coupon_tax_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.TaxCoupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _partrepayment_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.PartRepayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _brokercomission_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.BrokerCommission && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _repayment_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.Repayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
        }
        private List<Operation> _buy_list;
        private List<Operation> _coupon_list;
        private List<Operation> _coupon_tax_list;
        private List<Operation> _partrepayment_list;
        private List<Operation> _brokercomission_list;
        private List<Operation> _repayment_list; //погошения бумаги... хз пока как оброботать

        public event PropertyChangedEventHandler PropertyChanged;
        public int CompareTo(Bound_now other) { return Profit_perc.CompareTo(other.Profit_perc); }


        public int CompareTo(object obj)
        {
            var b = obj as Bound_now;
            return Profit_perc.CompareTo(b.Profit_perc);
        }

        public decimal Last_Coupon_payed
        {
            get
            {
                var buf = _coupon_list;
                if (buf.Count > 0)
                {
                    return buf.Last().Payment;
                }
                return 0;
            }
        }
        public decimal Coupon_summ
        {
            get
            {
                var buf = _coupon_list;
                if (buf.Count > 0)
                {
                    return buf.Sum(t => t.Payment);
                }
                return 0;
            }
        }
        public int Coupon_cnt
        {
            get
            {
                return _coupon_list.Count;  
            }
        }
        public decimal Coupon_Tax_summ
        {
            get
            {
                var buf = _coupon_tax_list;
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
                var buf = _partrepayment_list;
                if (buf.Count > 0)
                {
                    return Math.Abs(buf.Sum(t => t.Payment));
                }
                return 0;
            }
        }

        public decimal Broker_comission_total
        {
            get
            {
                return Math.Abs(_brokercomission_list.Sum(t => t.Payment));
            }
        }

        public decimal Profit
        {
            get
            {
                return Coupon_summ - Coupon_Tax_summ + Buy_Back_summ + Diff_Price
                    - Broker_comission_total + Bound.Nkd_for_now_sum;
            }
        }
        public decimal Profit_perc
        {
            get
            {
                return ((Profit * 100) / Summ_buy);
            }
        }

        public string Profit_perc_string
        {
            get
            {
                if (Profit_perc > 0)
                    return string.Format("+{0:#0.0}", Math.Abs(Profit_perc));
                else if (Profit_perc < 0)
                    return string.Format("-{0:#0.0}", Math.Abs(Profit_perc));
                else
                    return string.Format("{0:#0.0}", Math.Abs(Profit_perc));
            }
        }
        public string Profit_string
        {
            get
            {
                if (Profit > 0)
                    return string.Format("+{0:#0.00}", Math.Abs(Profit));
                else if (Profit < 0)
                    return string.Format("-{0:#0.00}", Math.Abs(Profit));
                else
                    return string.Format("{0:#0.00}", Math.Abs(Profit));
            }
        }
        public Color Font_profit_clr
        {
            get
            {
                return Profit >= 0 ? Color.DarkGreen : Color.DarkRed;
            }
        }


        public int Cnt_buy
        {
            get
            {
                return (int)_buy_list.Select(t=>t.QuantityExecuted).Sum();//t => t.Trades).Sum(t => t.Select(t => t.Quantity).Sum()); //не правильно. Или что если были проаджи. следовательно фильтр
            }
        }

        public decimal Buy_summ_market_price
        {
            get
            {
                return _buy_list.Sum(t => t.Price * (int)t.QuantityExecuted);
            }
        }
        public decimal Buy_avg_market_price
        {
            get
            {
                return Buy_summ_market_price / Cnt_buy;
            }
        }
        public decimal Buy_summ_nkd
        {
            get
            {
                return Summ_buy - Buy_summ_market_price;
            }
        }
        public decimal Buy_avg_nkd
        {
            get
            {
                return Avg_buy - Buy_avg_market_price;
            }
        }
        public decimal Summ_buy
        {
            get
            {
                return Math.Abs(_buy_list.Sum(t => t.Payment));
            }
        }
        public decimal Avg_buy
        {
            get
            {
                return Math.Abs(_buy_list.Sum(t => t.Payment)) / Cnt_buy;
            }
        }

       public decimal Diff_Price
        {
            get
            {
                return Bound.Market_price_total - Summ_buy;
            }
        }
        public decimal Diff_Price_avg
        {
            get
            {
                return Bound.Market_price - Avg_buy;
            }
        }
        public Color Font_diff_clr
        {
            get
            {
                return Diff_Price >= 0 ? Color.DarkGreen : Color.DarkRed;
            }
        }
    }


}
