using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
    public class Bound_now : Bound, INotifyPropertyChanged
    {
        public Bound_now(Portfolio.Position ps) : base(ps)
        {
            Base = ps;
            _buy_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.Buy && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _coupon_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.Coupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _coupon_tax_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.TaxCoupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _partrepayment_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.PartRepayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _brokercomission_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.BrokerCommission && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _repayment_list = Operations_list.Where(t => t.OperationType == ExtendedOperationType.Repayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
        }
        private List<Operation> _buy_list;
        private List<Operation> _coupon_list;
        private List<Operation> _coupon_tax_list;
        private List<Operation> _partrepayment_list;
        private List<Operation> _brokercomission_list;
        private List<Operation> _repayment_list; //погошения бумаги... хз пока как оброботать


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
                return _brokercomission_list.Sum(t => t.Payment);
            }
        }

        public decimal Profit
        {
            get
            {
                return Coupon_summ - Coupon_Tax_summ + Buy_Back_summ + Diff_price_sum_nkd
                    - Broker_comission_total;
            }
        }
        public decimal Profit_perc
        {
            get
            {
                return ((Profit * 100) / Avg_buy_paid_total);
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
                return _buy_list.Select(t => t.Trades).Sum(t => t.Select(t => t.Quantity).Sum());
            }
        }

        public decimal Buy_summ_market_price
        {
            get
            {
                return _buy_list.Sum(t => t.Price);
            }
        }
        public decimal Buy_avg_market_price
        {
            get
            {
                return _buy_list.Sum(t => t.Price) / Cnt_buy;
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

    }


}
