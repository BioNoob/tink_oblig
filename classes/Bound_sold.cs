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
    class Bound_sold : Bound, INotifyPropertyChanged
    {
        public Bound_sold(Portfolio.Position ps) : base(ps)
        {
            Base = ps;
            _sell_list = Operations_for_sold.Where(t => t.OperationType == ExtendedOperationType.Sell && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _coupon_list = Operations_for_sold.Where(t => t.OperationType == ExtendedOperationType.Coupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _coupon_tax_list = Operations_for_sold.Where(t => t.OperationType == ExtendedOperationType.TaxCoupon && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _partrepayment_list = Operations_for_sold.Where(t => t.OperationType == ExtendedOperationType.PartRepayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _brokercomission_list = Operations_for_sold.Where(t => t.OperationType == ExtendedOperationType.BrokerCommission && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            _repayment_list = Operations_for_sold.Where(t => t.OperationType == ExtendedOperationType.Repayment && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
        }
        private List<Operation> _sell_list;
        private List<Operation> _coupon_list;
        private List<Operation> _coupon_tax_list;
        private List<Operation> _partrepayment_list;
        private List<Operation> _brokercomission_list;
        private List<Operation> _repayment_list;


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

        /// <summary>
        /// Дата закрытия (полсденей продажи)
        /// </summary>
        public DateTime CloseDate
        {
            get
            {
                return _sell_list.Last().Date;
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
        public decimal Summ_Sell
        {
            get
            {
                return _sell_list.Sum(t => t.Payment);
            }
        }


        private decimal _diff_sell;
        /// <summary>
        /// 
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
        public decimal Profit
        {
            get
            {
                var a = Diff_sell;
                var c = Coupon_summ - Coupon_Tax_summ;
                return a + c + Buy_Back_summ;

            }
        }
        public decimal Profit_prc
        {
            get
            {
                if (Profit == 0)
                    return 0;
                return ((Profit * 100) / Price_now_total_avg);
            }
        }
    }
}
