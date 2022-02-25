using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;
using static tink_oblig.classes.Accounts;

namespace tink_oblig.classes
{
    public class Bound_Conclav : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Bound_now Bound_now { get; set; }
        public Bound_sold Bound_sold { get; set; }

        public Bound Bound { get; set; }

        public SeeHistory Avalibale_mode { get; set; }

        public Bound_Conclav(Bound bnb)
        {
            Bound = bnb;
            SetMode();
        }
        public decimal Money_sum
        {
            get
            {
                if (Avalibale_mode == SeeHistory.WithHistory)
                    return Bound_now.Summ_buy + Bound_sold.Summ_sell;
                else
                    return 0;
            }
        }
        public int Cnt_sum
        {
            get
            {
                if (Avalibale_mode == SeeHistory.WithHistory)
                    return Bound_now.Cnt_buy + Bound_sold.Cnt_sell;
                else
                    return 0;
            }
        }
        public decimal Profit
        {
            get
            {
                if (Avalibale_mode == SeeHistory.WithHistory)
                    return Bound_now.Profit + Bound_sold.Profit;
                else
                    return 0;
            }
        }
        public decimal Profit_perc
        {
            get
            {
                if (Avalibale_mode == SeeHistory.WithHistory)
                {
                    if (Profit == 0)
                        return 0;
                    return ((Profit * 100) / Money_sum);
                }
                else
                    return 0;

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


        public void SetMode()
        {
            if (Bound.Operations_list.Count < 1)
                return;
            List<Operation> buf_s = new List<Operation>();
            List<Operation> buf_n = new List<Operation>();
            Splitter_sold_out(Bound.Operations_list, out buf_s, out buf_n);
            int counter = 0;
            if (buf_s.Count > 0)
            {
                Bound copy_s = Bound.Copy();
                counter += (int)SeeHistory.History;
                copy_s.Operations_list = buf_s;
                Bound_sold = new Bound_sold(copy_s);
            }

            if (buf_n.Count > 0)
            {
                Bound copy_n = Bound.Copy();
                counter += (int)SeeHistory.NoHistrory;
                copy_n.Operations_list = buf_n;
                Bound_now = new Bound_now(copy_n);
            }
            Avalibale_mode = (SeeHistory)counter;
        }
        public static void Splitter_sold_out(List<Operation> operations, out List<Operation> sold_list, out List<Operation> now_list)
        {
            /*
             * Определить периоды когда позиция закрыта (или частично закрыта)
             * все покупки в хронологическом порядке по одной штуке
             * определяем участки закрытия берем даты где было продано
             * по этим датам свичим между листами
             */
            sold_list = new List<Operation>();
            now_list = new List<Operation>();
            var copyoper = new List<Operation>(operations);
            //operations = operations.Where(t => t.OperationType != ExtendedOperationType.BrokerCommission && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            var buy_list = operations.Where(t => t.OperationType == ExtendedOperationType.Buy && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            var sell_list = operations.Where(t => t.OperationType == ExtendedOperationType.Sell && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            copyoper = operations.Except(buy_list).ToList();
            copyoper = copyoper.Except(sell_list).ToList();
            copyoper = copyoper.Except(copyoper.Where(t => t.Status != OperationStatus.Done)).ToList();
            copyoper = copyoper.OrderBy(t => t.Date).ToList();
            List<Operation> buf_list = new List<Operation>();

            //создаем список покупочных операций поштучно
            foreach (var item in buy_list)
            //foreach (var item in operations)
            {
                for (int i = 0; i < item.QuantityExecuted; i++)
                {
                    buf_list.Add(new Operation(item.Id, item.Status, new List<Trade>(), new MoneyAmount(Currency.Rub, item.Commission.Value / item.Trades.Sum(t => t.Quantity)), Currency.Rub,
                        item.Payment / item.Trades.Sum(t => t.Quantity), item.Price, 1, 1, item.Figi, item.InstrumentType, false, item.Date, item.OperationType));
                }

            }
            foreach (var item in sell_list)
            {
                //в любом случае должны быть взяты столько сколько купленно + продано
                //не хватает обработки случаев когда комиссия раньше покупки
                int sold_cnt = item.Trades.Sum(t => t.Quantity);
                var a = buf_list.Take(sold_cnt).Select(t => t.Date).ToList(); //взяли операции с первой купленной (после последней продажи, или первой покупки)

                //дата следующей покупки
                DateTime? dt = null;
                if (sold_cnt < buf_list.Count)
                    dt = buf_list.Take(sold_cnt + 1).Select(t => t.Date).Last();

                List<Operation> q = new List<Operation>();

                if (dt != null && dt != a.Last())
                {
                    q = copyoper.Where(t => t.Date >= a.First()).ToList();
                    q = q.Where(t => t.Date < dt).ToList();
                }
                else
                {
                    q = copyoper.Where(t => t.Date >= a.First()).ToList();

                    if (sold_cnt != buf_list.Count)
                        q = q.Where(t => t.Date <= item.Date).ToList();

                    //sold_list.AddRange(operations.Except(sold_list)); //не верна или верна?
                    //return;
                }
                copyoper = copyoper.Except(q).ToList(); //режем записанные в кушке
                sold_list.AddRange(q); //промежуточные события между покупкой продажей
                sold_list.AddRange(buf_list.Take(sold_cnt).ToList()); //покупки
                sold_list.Add(item); //продажа
                sold_list = sold_list.OrderBy(t => t.Date).ToList();
                buf_list.RemoveRange(0, sold_cnt); // удаляем записанные
            }
            now_list.AddRange(buf_list);//= operations.Except(sold_list).ToList();
            now_list.AddRange(copyoper.Except(sold_list));
            now_list = now_list.OrderBy(t => t.Date).ToList();
        }
    }
}
