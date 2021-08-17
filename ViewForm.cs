using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using tink_oblig.classes;
using Tinkoff.Trading.OpenApi.Models;
using static tink_oblig.classes.Accounts;

namespace tink_oblig
{
    public partial class ViewForm : UserControl
    {
        public Bounds Selected_portfail { get; set; }
        SeeHistory SeeHistory { get; set; }

        public void Switch_representation(Bounds acc, SeeHistory history)
        {
            BoundListLayPannel.Controls.Clear();
            SeeHistory = history;
            Selected_portfail = acc.Copy();
            string buf_text = "";
            switch (SeeHistory)
            {
                case SeeHistory.NoHistrory:
                    Selected_portfail.BoundsList = Selected_portfail.BoundsList.Where(t => !t.Simplify).ToList();
                    buf_text = "Купите что-нибудь из облигаций";
                    break;
                case SeeHistory.History:
                    Selected_portfail.BoundsList = Selected_portfail.BoundsList.Where(t => t.Simplify).ToList();
                    buf_text = "Продайте что-нибудь из облигаций";
                    break;
                case SeeHistory.WithHistory:
                    buf_text = "Купите что-нибудь из облигаций";
                    break;
            }
            foreach (var bd in Selected_portfail.BoundsList.OrderByDescending(t=>t.Last_sell_dt))
            {
                switch (SeeHistory)
                {
                    case SeeHistory.NoHistrory:
                        if (!bd.Simplify)
                            LoadBounds(bd);
                        break;
                    case SeeHistory.History:
                        if (bd.Simplify)
                            LoadBounds(bd);
                        break;
                    case SeeHistory.WithHistory:
                        LoadBounds(bd);
                        break;
                }
            }
            if(Selected_portfail.BoundsList.Count < 1)
            {
                BoundListLayPannel.RowCount++;
                BoundListLayPannel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var newOne = new Label();
                newOne.Text = buf_text;
                newOne.Font = new System.Drawing.Font("Segoe UI",15F,System.Drawing.FontStyle.Bold);
                newOne.BorderStyle = BorderStyle.FixedSingle;
                newOne.Dock = DockStyle.Top;
                newOne.AutoSize = true;
                newOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                BoundListLayPannel.Controls.Add(newOne, 0, BoundListLayPannel.RowCount);
            }
        }
        public ViewForm(Bounds acc, SeeHistory history)
        {
            //Program.InnerAccount.LoadInfoDone += InnerAccount_LoadInfoDone;
            InitializeComponent();
            Switch_representation(acc, history);
        }

        private void LoadBounds(Bound bd)
        {
            BoundListLayPannel.RowCount++;
            BoundListLayPannel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            ListBoundWatch newOne = new ListBoundWatch(bd);//("Test", "TST","lol", 10, 11, 12, 13, 1));
            newOne.BorderStyle = BorderStyle.None;
            newOne.Dock = DockStyle.Top;
            BoundListLayPannel.Controls.Add(newOne, 0, BoundListLayPannel.RowCount);
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            total_money_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "SumB_Coast", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            summ_cpn_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "SumB_Coupons", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            total_profit_perc_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "Profit_Summ_Perc_String", true, DataSourceUpdateMode.OnPropertyChanged));
            total_profit_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "Sum_Profit", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            total_profit_perc_lbl.DataBindings.Add(new Binding("ForeColor", Selected_portfail, "Font_Profit_Clr", true, DataSourceUpdateMode.OnPropertyChanged));
            total_profit_lbl.DataBindings.Add(new Binding("ForeColor", Selected_portfail, "Font_Profit_Clr", true, DataSourceUpdateMode.OnPropertyChanged));

            total_cnt_cpn_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "Cpn_Cnt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
            total_cnt_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "Bnd_Cnt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));

            total_cpn_taxes_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "Cpn_Taxes", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            total_price_diff_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "Total_Diff_String", true, DataSourceUpdateMode.OnPropertyChanged));
            total_price_diff_lbl.DataBindings.Add(new Binding("ForeColor", Selected_portfail, "Font_Diff_Clr", true, DataSourceUpdateMode.OnPropertyChanged));

            buy_back_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "Buy_back", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
        }

        private void refresh_btn_Click(object sender, EventArgs e)
        {

        }

        private void better_profit_btn_Click(object sender, EventArgs e)
        {
            if (Selected_portfail.BoundsList.Count > 0)
            {
                var a = Selected_portfail.BoundsList.Max(t => t.Profit_summ);
                BoundWatchForm bwf = new BoundWatchForm(Selected_portfail.BoundsList.Where(t => t.Profit_summ == a).Single());
                bwf.Show();
            }

        }

        private void worst_ptofit_btn_Click(object sender, EventArgs e)
        {
            if (Selected_portfail.BoundsList.Count > 0)
            {
                var a = Selected_portfail.BoundsList.Min(t => t.Profit_summ);
                BoundWatchForm bwf = new BoundWatchForm(Selected_portfail.BoundsList.Where(t => t.Profit_summ == a).Single());
                bwf.Show(); //ПОЧЕМУ ТО ВИСНЕТ НАХРЕН
            }
        }
        //private void change_history_btn_Click(object sender, EventArgs e)
        //{
        //    ViewForm vfm = new ViewForm(Program.InnerAccount.Selected_portfail_backup, SeeHistory.History);
        //    vfm.Show();
        //}
    }
}
