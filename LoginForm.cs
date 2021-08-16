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
        public async Task<bool> do_login(string token = "t.lQPDdG2GWl7DHrLTdOrmbh1M3bCnq5k_tflcuHC7mETEO8p4_jqXCjrzBceN_FSWlUM2JmBjvB5DaNI1j09v0g")
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
            await Program.InnerAccount.DoLoad_Portfail();
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
                //Program.InnerAccount.LoadObligInfoDone += (t, mes) =>
                //{
                //    if (!string.IsNullOrEmpty(mes))
                //    {
                //        MessageBox.Show($"Error {mes}");
                //    }
                //    Program.InnerAccount.SetSelectedPrtf(t);
                //    //ViewForm wfrm = new ViewForm(t, Accounts.SeeHistory.NoHistrory);
                //    //wfrm.Show();
                //    //this.Hide();
                //};
                //await Program.InnerAccount.DoLoad_ObligList();
                ManagerForm mf = new ManagerForm();
                mf.Show();
                this.Hide();

            }
            else
            {
                //MessageBox.Show("Test");
                login_btn.Enabled = true;
            }
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
