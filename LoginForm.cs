using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tink_oblig.classes;
using Tinkoff.Trading.OpenApi.Network;

namespace tink_oblig
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        public async Task<bool> do_login(string token = "t.ah7Yy_ylysKKaAGeK-8zo5CxwrWIyRoijeXSjakp9vJBphO1gKmakiNpyqouupEjmNWpMqckzGkywsKTnaRpqQ")
        {
            var connection = ConnectionFactory.GetConnection(token);
            var context = connection.Context;
            Program.CurrentContext = context;
            bool result = false;
            Program.InnerAccount.JobsDone += (t, mes) =>
            {
                if (!string.IsNullOrEmpty(mes))
                {
                    MessageBox.Show($"Error {mes}");
                }
                result = t;
            };
            await Program.InnerAccount.doLoad();
            return result;
        }

        private async void login_btn_Click(object sender, EventArgs e)
        {
            login_btn.Enabled = false;
            bool result;
            if (string.IsNullOrEmpty(keypare_cmb.SelectedText))
                result = await Task.Run(() => do_login()); //await do_login();
            else
                result = await Task.Run(() => do_login(keypare_cmb.SelectedText));
            if (result)
            {
                //тут припелить выбор аккаунта
                var o = Program.InnerAccount.Portfolios.Keys.Where(t => t.BrokerAccountType == Tinkoff.Trading.OpenApi.Models.BrokerAccountType.Tinkoff).Single();
                foreach (var item in Program.InnerAccount.Portfolios[o].BoundsList)
                {
                    await Accounts.LoadInfoBound(item);
                }
                await Accounts.LoadHistoryBound(Program.InnerAccount.Portfolios[o]);
                ViewForm wfrm = new ViewForm(Program.InnerAccount.Portfolios[o]);
                wfrm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Test");
                login_btn.Enabled = true;
            }
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
