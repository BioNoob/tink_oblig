using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
    public class Accounts
    {
        public Accounts()
        {
            Portfolios = new Dictionary<Account, Bounds>();
        }
        public Dictionary<Account, Bounds> Portfolios { get; set; }

        public delegate void JobInfo(bool ok, string mes = "");
        public event JobInfo JobsDone;

        public async Task doLoad()
        {
            try
            {
                var acc = await Program.CurrentContext.AccountsAsync();
                foreach (var item in acc)
                {
                    var prtfl = await Program.CurrentContext.PortfolioAsync(item.BrokerAccountId);
                    Bounds lbd = new Bounds(item);
                    var lpl = prtfl.Positions.Where(t => t.InstrumentType == InstrumentType.Bond).ToList();
                    foreach (var itm in lpl)
                    {
                        lbd.BoundsList.Add(new Bound(itm)); //лист сейчас. Для истории надо грузить через операции..
                    }
                    Portfolios.Add(item, lbd);
                }

                JobsDone?.Invoke(true);
            }
            catch (Exception ex)
            {
                JobsDone?.Invoke(false, ex.Message);
            }
        }
        public static async Task LoadInfoBound(Bound bo)//string ticker)
        {
            string xmlStr;
            using (var wc = new WebClient())
            {
                xmlStr = await wc.DownloadStringTaskAsync($"https://iss.moex.com/iss/securities/{bo.Base.Ticker}.xml");
            }
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);
            var xDoc = XDocument.Parse(xmlDoc.OuterXml);
            var sb = xDoc.Descendants("rows").Descendants("row");
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in sb)
            {
                if (item.Attributes().Where(t => t.Name == "name").Count() > 0)
                    dic.Add(item.Attributes("name").Select(x => x.Value).Single(), item.Attributes("value").Select(x => x.Value).Single());
            }
            decimal b = 0;
            int i = 0;
            DateTime dt = new DateTime();
            bo.Next_pay_dt = DateTime.TryParse(dic["COUPONDATE"], out dt) ? dt : dt;
            bo.End_pay_dt = DateTime.TryParse(dic["MATDATE"], out dt) ? dt : dt;
            bo.Nominal = decimal.TryParse(dic["FACEVALUE"], NumberStyles.Float, CultureInfo.InvariantCulture, out b) ? b : 0;
            bo.Pay_period = int.TryParse(dic["COUPONFREQUENCY"], NumberStyles.Float, CultureInfo.InvariantCulture, out i) ? 365/i : 0;
            bo.Cpn_Percent = decimal.TryParse(dic["COUPONPERCENT"], NumberStyles.Float, CultureInfo.InvariantCulture, out b) ? b : 0;
            bo.Cpn_val = decimal.TryParse(dic["COUPONVALUE"], NumberStyles.Float, CultureInfo.InvariantCulture, out b) ? b : 0;
            bo.LoadImagePath();
        }
        public static async Task LoadHistoryBound(Bounds bounds)//string ticker)
        {
            //По отношению к времени не верно. Нужно сканить от даты покупки, если перед ней кол-во = 0
            foreach (var bo in bounds.BoundsList)
            {
                bo.Payed_cpn_list =  await Program.CurrentContext.OperationsAsync(new DateTime(2015, 01, 01), DateTime.Now, bo.Base.Figi, bounds.Acc.BrokerAccountId);
            }
        }

        public static async Task LoadAllBndHistory(Bounds bounds)
        {
            var z = await Program.CurrentContext.OperationsAsync(new DateTime(2015, 01, 01), DateTime.Now, "", bounds.Acc.BrokerAccountId);
            var selectedz = z.Where(t => t.InstrumentType == InstrumentType.Bond).Select(t=>t.Figi).Distinct().ToList();
            var notfnd = selectedz.Except(bounds.BoundsList.Select(t=>t.Base.Figi)).ToList();
            //для этих получить инфу (
            foreach (var item in notfnd)
            {
                var t = await Program.CurrentContext.MarketSearchByFigiAsync(item); 
                Bound b = new Bound(new Portfolio.Position(t.Name,item,t.Ticker,t.Isin,t.Type,0,0,new MoneyAmount(t.Currency, 0),0,new MoneyAmount(t.Currency, 0),new MoneyAmount(t.Currency, 0)));
                b.Payed_cpn_list = z.Where(t => t.Figi == item).ToList();
                b.Simplify = true;
                bounds.BoundsList.Add(b);
            }

        }
    }
}
