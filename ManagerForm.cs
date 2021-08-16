using System.Data;
using System.Linq;
using System.Windows.Forms;
using tink_oblig.classes;
using Tinkoff.Trading.OpenApi.Models;
using static tink_oblig.classes.Accounts;

namespace tink_oblig
{
    public partial class ManagerForm : Form
    {
        public SeeHistory Mode { get; set; }
        public Account_m Acc { get; set; }
        private ViewForm _vf;
        public ManagerForm()
        {
            //Program.InnerAccount.Portfolios;
            InitializeComponent();

            LoadAccounts();

            Mode = SeeHistory.NoHistrory; // грузить с настроек
            Acc = Program.InnerAccount.Portfolios.Keys.Where(t => t.BrokerAccountType == BrokerAccountType.Tinkoff).Single(); // грузить с настроек

            _vf = new ViewForm(Program.InnerAccount.Portfolios[Acc], Mode);
            _vf.BorderStyle = BorderStyle.None;
            _vf.Dock = DockStyle.Fill;
            view_panel.Controls.Add(_vf);
        }
        private void LoadAccounts()
        {
            foreach (var item in Program.InnerAccount.Portfolios.Keys)
            {
                account_switcher_cmb.Items.Add(item);
            }
        }
        private void SwitchAcc()
        {
            _vf.Switch_representation(Program.InnerAccount.Portfolios[Acc], Mode);
        }

        private void account_switcher_cmb_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            Acc = sender as Account_m;
            SwitchAcc();
        }
    }
}
