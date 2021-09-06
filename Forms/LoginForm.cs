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
                var indx_id = Settings.Default.LastID_indx;
                if (indx_id >= 0)
                    keypare_cmb.SelectedIndex = indx_id;
                else
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
            exit_btn.Select();
            bool result;
            if (string.IsNullOrEmpty(keypare_cmb.SelectedItem.ToString()))
            {
                MessageBox.Show("Введите API ключ!", "Ошибка");
                login_btn.Enabled = true;
                return;
            }
            else
            {
                var t = keypare_cmb.SelectedItem.ToString();
                result = await Task.Run(() => do_login(t));
            }
            if (result)
            {
                if (id_save_chk.Checked)
                {
                    if (Settings.Default.AccID == null)
                        Settings.Default.AccID = new System.Collections.Specialized.StringCollection();
                    if (!Settings.Default.AccID.Contains(keypare_cmb.SelectedItem.ToString()))
                        Settings.Default.AccID.Add(keypare_cmb.SelectedItem.ToString());
                    Settings.Default.LastID_indx = keypare_cmb.SelectedIndex;
                }
                else
                {
                    if (Settings.Default.AccID.Contains(keypare_cmb.SelectedItem.ToString()))
                    {
                        DialogResult ult = MessageBox.Show($"Удалить из сохранненых ключей\n{keypare_cmb.SelectedItem.ToString()} ?", "Удаление", MessageBoxButtons.YesNo);
                        if (ult == DialogResult.Yes)
                        {
                            Settings.Default.AccID.Remove(keypare_cmb.SelectedItem.ToString());
                            keypare_cmb.Items.RemoveAt(keypare_cmb.SelectedIndex);
                            Settings.Default.LastID_indx = keypare_cmb.SelectedIndex != 0 ? keypare_cmb.SelectedIndex - 1 : 0;
                        }

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
