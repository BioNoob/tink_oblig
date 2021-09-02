using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tink_oblig.classes;
using tink_oblig.Properties;
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

                //Settings.Default.SelectedAcc = "";
                //Settings.Default.SelectedHistoryMode = 0;
                //Settings.Default.Save();
                ManagerForm mf = new ManagerForm();
                mf.StartPosition = FormStartPosition.CenterParent;
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
        ImgInfo img = new ImgInfo();
        
        private void linkLabel1_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", "https://www.tinkoff.ru/invest/settings/");
            if (img.Visible)
            {
                return;
            }
            img.Focus();
            img.Location = new Point(Location.X + this.Size.Width / 2, Location.Y);
            img.Show();
        }
    }
}
