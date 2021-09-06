using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using tink_oblig.Properties;
using Tinkoff.Trading.OpenApi.Network;

namespace tink_oblig
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            id_save_chk.Checked = true;
            if (Settings.Default.AccID != null && Settings.Default.AccID.Count > 0)
            {
                foreach (var item in Settings.Default.AccID)
                {
                    keypare_cmb.Items.Add(item);
                }
                keypare_cmb.SelectedIndex = 0;
            }

        }
        public async Task<bool> do_login(string token)
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
            {
                MessageBox.Show("Введите API ключ!", "Ошибка");
                login_btn.Enabled = true;
                return;
            }
            else
            {
                var t = keypare_cmb.SelectedText;
                result = await Task.Run(() => do_login(t));
            }
            if (result)
            {
                if (id_save_chk.Checked)
                {
                    if (Settings.Default.AccID == null)
                        Settings.Default.AccID = new System.Collections.Specialized.StringCollection();
                    if (!Settings.Default.AccID.Contains(keypare_cmb.SelectedText))
                        Settings.Default.AccID.Add(keypare_cmb.SelectedText);
                }
                else
                {
                    if (Settings.Default.AccID.Contains(keypare_cmb.SelectedText))
                    {
                        DialogResult ult = MessageBox.Show($"Удалить из сохранненых ключей\n{keypare_cmb.SelectedText} ?", "Удаление", MessageBoxButtons.YesNo);
                        if (ult == DialogResult.Yes)
                            Settings.Default.AccID.Remove(keypare_cmb.SelectedText);
                    }
                }
                Settings.Default.Save();
                Settings.Default.Reload();
                Program.mf = new ManagerForm() { StartPosition = FormStartPosition.Manual };
                Program.mf.Location = new Point(this.Location.X, this.Location.Y - Program.mf.Height / 2);
                Program.mf.Show();
                this.Hide();
            }
            else
            {
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
            img.ShowDialog();
        }

        private void id_save_chk_Click(object sender, EventArgs e)
        {
            //id_save_chk.Checked = !id_save_chk.Checked;
        }
    }
}
