using Newtonsoft.Json;
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
    public class Account_m : Account
    {
        public Account_m(BrokerAccountType brk, string id) : base(brk, id)
        {
        }
        public string TypeAcc
        {
            get
            {
                switch (BrokerAccountType)
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
        public Account GetBase()
        {
            return new Account(BrokerAccountType,BrokerAccountId);
        }
        public string GetJson()
        {
            return JsonConvert.SerializeObject(GetBase());
            //return JsonSerializer.Serialize(this);//JsonConvert.SerializeObject(this);
        }
        public override string ToString()
        {
            return $"{TypeAcc}  ({BrokerAccountId})";
        }
    }
    public class Accounts
    {
        public Accounts()
        {
            Portfolios = new Dictionary<Account_m, Bounds>();
        }
        public enum SeeHistory 
        {
            NoHistrory = 1,
            History,
            WithHistory
        }
        public Dictionary<Account_m, Bounds> Portfolios { get; set; }
        private Bounds _selected_portf_bckp;
        public Bounds Selected_portfail_backup { get { return _selected_portf_bckp; } }
        public void SetSelectedPrtf(Bounds b)
        {
            _selected_portf_bckp = b;
        }

        public delegate void JobInfo(bool ok, string mes = "");
        public delegate void JobBounds(Bounds bnd, string mes = "");
        public event JobInfo JobsDone;
        public event JobBounds LoadObligInfoDone;
        /// <summary>
        /// Грузим список бумаг по всем аккаунтам
        /// </summary>
        /// <returns></returns>
        public async Task DoLoad_Portfail()
        {
            try
            {
                var acc = await Program.CurrentContext.AccountsAsync();
                foreach (var item in acc)
                {
                    var price_now = await Program.CurrentContext.MarketOrderbookAsync("", 1);
                    var prtfl = await Program.CurrentContext.PortfolioAsync(item.BrokerAccountId);
                    Bounds lbd = new Bounds(new Account_m(item.BrokerAccountType, item.BrokerAccountId));
                    var lpl = prtfl.Positions.Where(t => t.InstrumentType == InstrumentType.Bond).ToList();
                    foreach (var itm in lpl)
                    {
                        lbd.BoundsList.Add(new Bound(itm)); //лист сейчас. Для истории надо грузить через операции..
                    }
                    Portfolios.Add(lbd.Acc, lbd);
                }

                JobsDone?.Invoke(true);
            }
            catch (Exception ex)
            {
                JobsDone?.Invoke(false, ex.Message);
            }
        }
        /// <summary>
        /// Грузим список опреаций для расчетов по бумаге для выбранного аккаунта
        /// </summary>
        /// <param name="acc">аккаунт</param>
        /// <returns></returns>
        public async Task DoLoad_ObligList(Account_m acc, bool fl_refresh = false) //грузим инфу по портфелью
        {
            var bounds = Program.InnerAccount.Portfolios[acc];
            var load_signal = bounds.BoundsList.Select(t => t.Operations_list).Select(t => t.Count).Sum(); //если операции загружены то > 0
            if (load_signal > 0)
            {
                if (!fl_refresh)
                {
                    LoadObligInfoDone?.Invoke(Program.InnerAccount.Portfolios[acc]);
                    return;
                }
            }
            foreach (var item in bounds.BoundsList)
            {
                item.Operations_list = await Program.CurrentContext.OperationsAsync(new DateTime(2015, 01, 01), DateTime.Now, item.Base.Figi, bounds.Acc.BrokerAccountId);
            }
            await LoadAllBndHistory(bounds);
            foreach (var item in bounds.BoundsList)
            {
                await LoadInfoBound(item);
            }
            LoadObligInfoDone?.Invoke(Program.InnerAccount.Portfolios[acc]);
        }
        /// <summary>
        /// Копаем инфу с мосбиржи для бумажки
        /// </summary>
        /// <param name="bo">бумажка</param>
        /// <returns></returns>
        public static async Task LoadInfoBound(Bound bo)//string ticker) грузим с мос биржи
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
            bo.Pay_period = int.TryParse(dic["COUPONFREQUENCY"], NumberStyles.Float, CultureInfo.InvariantCulture, out i) ? 365 / i : 0;
            bo.Cpn_Percent = decimal.TryParse(dic["COUPONPERCENT"], NumberStyles.Float, CultureInfo.InvariantCulture, out b) ? b : 0;
            bo.Cpn_val = decimal.TryParse(dic["COUPONVALUE"], NumberStyles.Float, CultureInfo.InvariantCulture, out b) ? b : 0;
            bo.LoadImagePath();
        }
        /// <summary>
        /// Грузим исторические сведенья для облиг в портфеле
        /// </summary>
        /// <param name="bounds">список облигаций</param>
        /// <returns></returns>
        public static async Task LoadAllBndHistory(Bounds bounds) //грузим для тех кого нет в живых
        {
            var z = await Program.CurrentContext.OperationsAsync(new DateTime(2015, 01, 01), DateTime.Now, "", bounds.Acc.BrokerAccountId);
            var selectedz = z.Where(t => t.InstrumentType == InstrumentType.Bond).Select(t => t.Figi).Distinct().ToList();
            var notfnd = selectedz.Except(bounds.BoundsList.Select(t => t.Base.Figi)).ToList();
            //для этих получить инфу (
            foreach (var item in notfnd)
            {
                var t = await Program.CurrentContext.MarketSearchByFigiAsync(item);
                var data = z.Where(t => t.Figi == item && t.OperationType == ExtendedOperationType.Buy && t.Status == OperationStatus.Done).ToList();
                //смотреть в трейдс а не в общем(
                var trd = data.Select(a => a.Trades).ToList();
                var cnt = trd.Sum(a => a.Sum(c => c.Quantity));

                var avg = data.Sum(a => Math.Abs(a.Payment)) / cnt;
                var buf_nkd = data.Sum(a => Math.Abs(a.Payment) - (a.Price * a.Trades.Sum(c => c.Quantity))); //- (a.Price * a.Quantity));
                var avg_no_nkd = avg - buf_nkd / cnt;
                var price_now = await Program.CurrentContext.MarketOrderbookAsync(item, 1);
                var expt_yeld = price_now.ClosePrice - avg_no_nkd; //грузим свечки берем последнюю цену
                Bound b = new Bound(new Portfolio.Position(t.Name, item, t.Ticker, t.Isin, t.Type, cnt, 0, new MoneyAmount(t.Currency, expt_yeld), cnt, new MoneyAmount(t.Currency, avg), new MoneyAmount(t.Currency, avg_no_nkd)));

                b.Operations_list = z.Where(t => t.Figi == item).ToList();
                b.Simplify = true;
                bounds.BoundsList.Add(b);
            }
        }
    }
}
