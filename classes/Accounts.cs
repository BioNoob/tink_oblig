using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

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
                    Bounds lbd = new Bounds();
                    var lpl = prtfl.Positions.Where(t => t.InstrumentType == InstrumentType.Bond).ToList();
                    foreach (var itm in lpl)
                    {
                        lbd.BoundsList.Add(new Bound(itm));
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
            bo.Pay_period = int.TryParse(dic["COUPONFREQUENCY"], NumberStyles.Float, CultureInfo.InvariantCulture, out i) ? i : 0;
            bo.Cpn_Percent = decimal.TryParse(dic["COUPONPERCENT"], NumberStyles.Float, CultureInfo.InvariantCulture, out b) ? b : 0;
            bo.Cpn_val = decimal.TryParse(dic["COUPONVALUE"], NumberStyles.Float, CultureInfo.InvariantCulture, out b) ? b : 0;
            bo.LoadImagePath();
        }
    }
}
