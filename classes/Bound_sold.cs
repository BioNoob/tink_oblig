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
    public class Bound_sold : INotifyPropertyChanged
    {
        Bound Bound { get; set; }
        public Bound_sold(Bound bnb)
        {
            Bound = bnb;

            _buy_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.Buy && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _sell_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.Sell && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _coupon_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.Coupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _coupon_tax_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.TaxCoupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _partrepayment_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.PartRepayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _brokercomission_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.BrokerCommission && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _repayment_list = Bound.Operations_list.Where(t => t.OperationType == ExtendedOperationType.Repayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();


            List<Operation> buf_list = new List<Operation>();
            foreach (var item in _buy_list)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    buf_list.Add(new Operation(item.Id, OperationStatus.Done, new List<Trade>(), new MoneyAmount(Currency.Rub, item.Commission.Value / item.Trades.Sum(t => t.Quantity)), Currency.Rub,
                        item.Payment / item.Trades.Sum(t => t.Quantity), item.Price, 1, null, item.Figi, item.InstrumentType, false, item.Date, item.OperationType));
                }
            }
            buf_list = buf_list.OrderBy(t => t.Date).ToList();
            foreach (var item in _sell_list)
            {
                int sold_cnt = item.Trades.Sum(t => t.Quantity);
                Diff_sell += item.Payment - Math.Abs(buf_list.Take(sold_cnt).Sum(t => t.Payment));
                buf_list.RemoveRange(0, sold_cnt); // удаляем записанные
            }

        }

        private List<Operation> _buy_list;
        private List<Operation> _sell_list;
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
        /// <summary>
        /// Дата закрытия (полсденей продажи)
        /// </summary>
        public DateTime Close_Date
        {
            get
            {
                return _sell_list.Last().Date;
            }
        }
        /// <summary>
        /// Кол-во проданых при закрытии
        /// </summary>
        public int Close_cnt_sell
        {
            get 
            {
                return _sell_list.Last().Quantity;
            }
        }
        /// <summary>
        /// Кол-во проданного штук
        /// </summary>
        public int Cnt_sell
        {
            get
            {
                return _sell_list.Select(t => t.Trades).Sum(t => t.Select(t => t.Quantity).Sum());
            }
        }

        /// <summary>
        /// Сумма полученных платежей за все продажи
        /// </summary>
        public decimal Summ_sell
        {
            get
            {
                return _sell_list.Sum(t => t.Payment);
            }
        }
        /// <summary>
        /// Средняя полученных платежей за все продажи
        /// </summary>
        public decimal Summ_sell_avg
        {
            get
            {
                return Summ_sell / Cnt_sell;
            }
        }


        public decimal Market_price_summ_sell
        {
            get
            {
                return _sell_list.Select(t => t.Payment).Sum();
            }
        }
        public decimal Market_price_avg_sell
        {
            get
            {
                return _sell_list.Select(t => t.Payment).Sum() / Cnt_sell;
            }
        }

        public decimal Nkd_summ_by_sell
        {
            get
            {
                var a = Market_price_summ_sell -
                    (_sell_list.Select(t => t.Price).Sum() * Cnt_sell);
                return a;
            }
        }
        public decimal Nkd_avg_by_sell
        {
            get
            {
                return Nkd_summ_by_sell / Cnt_sell;
            }
        }

        private decimal _diff_sell;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Суммарная разница по закрытым позициям
        /// </summary>
        public decimal Diff_sell
        {//это разница по всем сделкам на все кол-во
            get
            {

                return _diff_sell;
            }
            set
            {
                _diff_sell = value;
            }
        }
        /// <summary>
        /// Средняя разница по закрытым позициям
        /// </summary>
        public decimal Diff_sell_avg
        {
            get
            {
                return Diff_sell / Cnt_sell;
            }
        }

        public decimal Profit
        {
            get
            {
                var a = Diff_sell;
                var c = Coupon_summ - Coupon_Tax_summ;
                return a + c + Buy_Back_summ - Broker_comission_total;

            }
        }
        public decimal Profit_perc
        {
            get
            {
                if (Profit == 0)
                    return 0;
                return ((Profit * 100) / Bound.Avg_buy_paid_total);
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
    }
}
