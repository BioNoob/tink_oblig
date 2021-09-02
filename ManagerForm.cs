
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using tink_oblig.classes;
using tink_oblig.Properties;
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
            InitializeComponent();
            LoadAccounts();
            foreach (SeeHistory item in Enum.GetValues(typeof(SeeHistory)))
            {
                switch (item)
                {
                    case SeeHistory.NoHistrory:
                        history_cmb.Items.Add("Открытые позиции");
                        break;
                    case SeeHistory.History:
                        history_cmb.Items.Add("Закрытые позиции");
                        break;
                    case SeeHistory.WithHistory:
                        history_cmb.Items.Add("Все позиции");
                        break;
                    default:
                        break;
                }
            }
            Program.InnerAccount.LoadObligInfoDone += InnerAccount_LoadObligInfoDone;
            LoadSettings();
            history_cmb.Enabled = false;
        }
        public void LoadSettings()
        {
            account_switcher_cmb.SelectionChangeCommitted -= account_switcher_cmb_SelectionChangeCommitted;
            history_cmb.SelectionChangeCommitted -= history_cmb_SelectionChangeCommitted;
            Account_m acm = null;

            if (!string.IsNullOrEmpty(Settings.Default.SelectedAcc))
            {
                Account acma = JsonConvert.DeserializeObject<Account>(Settings.Default.SelectedAcc);
                acm = new Account_m(acma.BrokerAccountType, acma.BrokerAccountId);
            }
            if (acm != null)
            {
                foreach (var item in account_switcher_cmb.Items)
                {
                    if (((Account_m)item).BrokerAccountId == acm.BrokerAccountId)
                    {
                        account_switcher_cmb.SelectedItem = item;
                        break;
                    }
                }
            }
            else
            {
                account_switcher_cmb.SelectedIndex = 0;
            }
            Mode = (SeeHistory)Settings.Default.SelectedHistoryMode;
            if ((int)Mode == 0)
            {
                history_cmb.SelectedIndex = 0; Mode = SeeHistory.NoHistrory;
            }
            else
                history_cmb.SelectedIndex = (int)Mode - 1;

            account_switcher_cmb.SelectionChangeCommitted += account_switcher_cmb_SelectionChangeCommitted;
            history_cmb.SelectionChangeCommitted += history_cmb_SelectionChangeCommitted;

            account_switcher_cmb_SelectionChangeCommitted(account_switcher_cmb, null);
        }
        private void InnerAccount_LoadObligInfoDone(Bounds bnd, string mes = "")
        {
            if (string.IsNullOrEmpty(mes))
                SwitchAcc();
            else
            {
                MessageBox.Show($"Ошибка загрузки информации:\n{mes}");
                Program.LoginForm.Show();
                this.Close();
                return;
            }

            history_cmb.Enabled = true;
            account_switcher_cmb.Enabled = true;
            refresh_btn.Enabled = true;
            pictureBox1.Visible = false;
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
            view_panel.Controls.Clear();
            _vf = new ViewForm(Program.InnerAccount.Portfolios[Acc], Mode);
            _vf.BorderStyle = BorderStyle.None;
            _vf.Dock = DockStyle.Fill;
            view_panel.Controls.Add(_vf);
        }

        private async void account_switcher_cmb_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            Acc = ((ComboBox)sender).SelectedItem as Account_m;
            history_cmb.Enabled = false;
            account_switcher_cmb.Enabled = false;
            refresh_btn.Enabled = false;
            pictureBox1.Visible = true;
            await Program.InnerAccount.DoLoad_ObligList(Acc);

        }
        private void history_cmb_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            Mode = (SeeHistory)history_cmb.SelectedIndex + 1;
            SwitchAcc();
        }

        private async void refresh_btn_Click(object sender, System.EventArgs e)
        {
            //Обновлять все или текущий? DialogResult
            await Program.InnerAccount.DoLoad_ObligList(Acc, true);
        }

        private void ManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.SelectedAcc = Acc.GetJson();
            Settings.Default.SelectedHistoryMode = (int)Mode;
            Settings.Default.Save();
            Settings.Default.Reload();
            Application.Exit();
        }

        private void ManagerForm_Shown(object sender, EventArgs e)
        {

        }
    }
}
