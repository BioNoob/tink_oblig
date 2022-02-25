
using Newtonsoft.Json;
using System;
using System.Linq;
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
        //public Accounts Acc { get; set; }
        private Account_m Acc_key { get; set; }
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
                        history_cmb.Items.Add($"Открытые позиции");
                        break;
                    case SeeHistory.History:
                        history_cmb.Items.Add($"Закрытые позиции");
                        break;
                    case SeeHistory.WithHistory:
                        history_cmb.Items.Add($"Совместные позиции");
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
            Account_m Load_m = null;

            if (!string.IsNullOrEmpty(Settings.Default.SelectedAcc))
            {
                Account Load = JsonConvert.DeserializeObject<Account>(Settings.Default.SelectedAcc);
                Load_m = new Account_m(Load.BrokerAccountType, Load.BrokerAccountId);
            }
            if (Load_m != null)
            {
                foreach (var item in account_switcher_cmb.Items)
                {
                    if (((Account_m)item).BrokerAccountId == Load_m.BrokerAccountId)
                    {
                        account_switcher_cmb.SelectedItem = item;
                        break;
                    }
                }
            }
            if (account_switcher_cmb.SelectedIndex == -1)
                account_switcher_cmb.SelectedIndex = 0;
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
            {
                SwitchAcc(bnd);
            }
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
            int si = history_cmb.SelectedIndex;
            history_cmb.Items.Clear();
            foreach (SeeHistory item in Enum.GetValues(typeof(SeeHistory)))
            {
                switch (item)
                {
                    case SeeHistory.NoHistrory:
                        history_cmb.Items.Add($"Открытые позиции {bnd.Bounds_Now.Count}");
                        break;
                    case SeeHistory.History:
                        history_cmb.Items.Add($"Закрытые позиции {bnd.Bounds_Sold.Count}");
                        break;
                    case SeeHistory.WithHistory:
                        history_cmb.Items.Add($"Совместные позиции {bnd.BoundsList.Where(t => t.Avalibale_mode == SeeHistory.WithHistory).Count()}");
                        break;
                    default:
                        break;
                }
            }
            history_cmb.SelectedIndex = si;
            if (history_cmb.SelectedIndex < 0)
            {
                history_cmb.SelectedIndex = 0;
                history_cmb_SelectionChangeCommitted(history_cmb, null);
            }

        }

        private void LoadAccounts()
        {
            foreach (var item in Program.InnerAccount.Portfolios.Keys)
            {
                account_switcher_cmb.Items.Add(item);
            }
        }
        private void SwitchAcc(Bounds bnd)
        {
            foreach (var item in bnd.BoundsList)
            {
                item.SetMode();
            }
            //view_panel.Controls.Clear();
            if (_vf == null)
            {
                _vf = new ViewForm(bnd, Mode);
                _vf.BorderStyle = BorderStyle.None;
                _vf.Dock = DockStyle.Fill;
                view_panel.Controls.Add(_vf);
            }
            else
                _vf.Switch_representation(bnd, Mode);

        }

        private async void account_switcher_cmb_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            Acc_key = ((ComboBox)sender).SelectedItem as Account_m;
            history_cmb.Enabled = false;
            account_switcher_cmb.Enabled = false;
            refresh_btn.Enabled = false;
            pictureBox1.Visible = true;
            await Program.InnerAccount.DoLoad_ObligList(Acc_key);

        }
        private void history_cmb_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            Mode = (SeeHistory)history_cmb.SelectedIndex + 1;
            SwitchAcc(Program.InnerAccount.Portfolios[Acc_key]);
        }

        private async void refresh_btn_Click(object sender, System.EventArgs e)
        {
            //Обновлять все или текущий? DialogResult
            history_cmb.Enabled = false;
            account_switcher_cmb.Enabled = false;
            refresh_btn.Enabled = false;
            pictureBox1.Visible = true;
            await Program.InnerAccount.DoLoad_ObligList(Acc_key, true);
        }

        private void ManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.SelectedAcc = Acc_key.GetJson();
            Settings.Default.SelectedHistoryMode = (int)Mode;
            Settings.Default.SortMode = _vf.LastSelectedIndx;
            Settings.Default.Save();
            Settings.Default.Reload();
            Application.Exit();
        }

        private void ManagerForm_Shown(object sender, EventArgs e)
        {

        }
    }
}
