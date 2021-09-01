using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
    public class Bound_Conclav : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Bound_now Bound_now { get; set; }
        public Bound_sold Bound_sold { get; set; }

        public Bound Bound { get; set; }

        public Bound_Conclav(Bound bnb)
        {
            Bound = bnb;
            Bound_now = new Bound_now(Bound);
            Bound_sold = new Bound_sold(Bound);
            List<Operation> buf_s = new List<Operation>();
            List<Operation> buf_n = new List<Operation>();
            Splitter_sold_out(Bound.Operations_list, out buf_s, out buf_n);
            Bound_now.Bound.Operations_list = buf_n;
            Bound_sold.Bound.Operations_list = buf_s;
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
            //operations = operations.Where(t => t.OperationType != ExtendedOperationType.BrokerCommission && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            var buy_list = operations.Where(t => t.OperationType == ExtendedOperationType.Buy && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            var sell_list = operations.Where(t => t.OperationType == ExtendedOperationType.Sell && t.Status == OperationStatus.Done).OrderBy(t => t.Date).ToList();
            List<Operation> buf_list = new List<Operation>();

            //создаем список покупочных операций поштучно
            foreach (var item in buy_list)
            //foreach (var item in operations)
            {
                for (int i = 0; i < item.QuantityExecuted; i++)
                {
                    buf_list.Add(new Operation(item.Id, item.Status, new List<Trade>(), new MoneyAmount(Currency.Rub, item.Commission.Value / item.Trades.Sum(t => t.Quantity)), Currency.Rub,
                        item.Payment / item.Trades.Sum(t => t.Quantity), item.Price, 1, null, item.Figi, item.InstrumentType, false, item.Date, item.OperationType));
                }
            }
            //не хватает алгоритма учета действий при покупке
            //брокер комишен.. и тд
            //мож как то попробовать выцепить прост дипазаон по другому
            foreach (var item in sell_list)
            {
                int sold_cnt = item.Trades.Sum(t => t.Quantity);
                var a = buf_list.Take(sold_cnt).Select(t => t.Date).ToList(); //взяли операции с первой купленной (после последней продажи, или первой покупки)
                //дата следующей покупки
                DateTime ?dt = null;
                if (sold_cnt < buf_list.Count)
                    dt = buf_list.Take(sold_cnt + 1).Select(t => t.Date).Last();
                
                if(dt != null)
                {
                    var q = operations.Where(t => t.Date >= a.First()).ToList();
                    q = q.Where(t => t.Date < dt).ToList();
                    sold_list.AddRange(q);
                    buf_list.RemoveRange(0, sold_cnt); // удаляем записанные
                }
                else
                {
                    sold_list.AddRange(operations);
                    return;
                }
            }
            now_list = operations.Except(sold_list).ToList();//РАБОТАЕТ ЛИ?
        }
    }
}
