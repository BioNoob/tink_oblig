using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
    class Accounts
    {
        public Accounts()
        {
            Portfolios = new Dictionary<Account, List<Bound>>();
        }
        public Dictionary<Account, List<Bound>> Portfolios { get; set; }

        public delegate void JobInfo(bool ok, string mes = "");
        public event JobInfo JobsDone;

        public async Task doLoad()
        {
            try
            {
                var acc = await Program.CurrentContext.AccountsAsync();
                foreach (var item in acc)
                {
                    var  prtfl = await Program.CurrentContext.PortfolioAsync(item.BrokerAccountId);
                    List<Bound> lbd = new List<Bound>();
                    var lpl = prtfl.Positions.Where(t => t.InstrumentType == InstrumentType.Bond).ToList();
                    foreach (var itm in lpl)
                    {
                        lbd.Add(new Bound(itm));
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
    }
}
