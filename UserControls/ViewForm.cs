using System;
using System.Linq;
using System.Windows.Forms;
using tink_oblig.classes;
using static tink_oblig.classes.Accounts;

namespace tink_oblig
{
    public partial class ViewForm : UserControl
    {
        public Bounds Selected_portfail { get; set; }
        SeeHistory SeeHistory { get; set; }
        public ViewForm(Bounds acc, SeeHistory history)
        {
            //Program.InnerAccount.LoadInfoDone += InnerAccount_LoadInfoDone;
            InitializeComponent();
            Switch_representation(acc, history);
        }
        private int Call_cnt = 0;
        public void Switch_representation(Bounds acc, SeeHistory history)
        {
            Call_cnt = 0;
            BoundListLayPannel.Controls.Clear();
            SeeHistory = history;
            Selected_portfail = acc.Copy();
            Selected_portfail.BoundsList = Selected_portfail.BoundsList.Where(t => t.Avalibale_mode == history | t.Avalibale_mode == SeeHistory.WithHistory).ToList();
            string buf_text = "";
            switch (SeeHistory)
            {
                case SeeHistory.NoHistrory:
                    buf_text = "Купите что-нибудь из облигаций";
                    break;
                case SeeHistory.History:
                    buf_text = "Продайте что-нибудь из облигаций";
                    break;
                case SeeHistory.WithHistory:
                    buf_text = "Продайте и купите что-нибудь";
                    break;
            }
            Selected_portfail.Mode = history;
            foreach (var bd in Selected_portfail.BoundsList)//.Bounds_Now)
            {
                switch (bd.Avalibale_mode)
                {
                    case SeeHistory.NoHistrory:
                        if (bd.Avalibale_mode == SeeHistory)
                            LoadBounds(bd);
                        break;
                    case SeeHistory.History:
                        if (bd.Avalibale_mode == SeeHistory)
                            LoadBounds(bd);
                        break;
                    case SeeHistory.WithHistory:
                        LoadBounds(bd);
                        break;
                }

            }
            //отдельным методом
            if (Call_cnt < 1)
            {
                BoundListLayPannel.RowCount++;
                BoundListLayPannel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                var newOne = new Label();
                newOne.Text = buf_text;
                newOne.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
                newOne.BorderStyle = BorderStyle.FixedSingle;
                newOne.Dock = DockStyle.Top;
                newOne.AutoSize = true;
                newOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                BoundListLayPannel.Controls.Add(newOne, 0, BoundListLayPannel.RowCount);
            }
            total_money_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "SumB_Cost", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
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

        private void LoadBounds(Bound_Conclav bd)
        {
            Call_cnt++;
            BoundListLayPannel.RowCount++;
            BoundListLayPannel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            ListBoundWatch newOne = new ListBoundWatch(bd, SeeHistory);
            newOne.BorderStyle = BorderStyle.None;
            newOne.Dock = DockStyle.Top;
            BoundListLayPannel.Controls.Add(newOne, 0, BoundListLayPannel.RowCount);
        }

        private void refresh_btn_Click(object sender, EventArgs e)
        {

        }

        private void better_profit_btn_Click(object sender, EventArgs e)
        {
            if (Selected_portfail.BoundsList.Count > 0)
            {
                Bound_Conclav bc;
                switch (SeeHistory)
                {
                    case SeeHistory.NoHistrory:
                        var a = Selected_portfail.Bounds_Now.Max();
                        bc = Selected_portfail.BoundsList.Where(t => t.Bound_now == a).Single();
                        break;
                    case SeeHistory.History:
                        var b = Selected_portfail.Bounds_Sold.Max();
                        bc = Selected_portfail.BoundsList.Where(t => t.Bound_sold == b).Single();
                        break;
                    case SeeHistory.WithHistory:
                    default:
                        var c = Selected_portfail.Bounds_Now.Max();
                        var d = Selected_portfail.Bounds_Sold.Max();
                        if (c.Profit_perc > d.Profit_perc)
                            bc = Selected_portfail.BoundsList.Where(t => t.Bound_now == c).Single();
                        else
                            bc = Selected_portfail.BoundsList.Where(t => t.Bound_sold == d).Single();
                        break;

                }
                BoundWatchCntrlFrm bwf_worst = new BoundWatchCntrlFrm(bc, SeeHistory);
                bwf_worst.Tag = $"{bc.Bound.Base.Ticker}";
                bwf_worst.Show();
            }

        }

        private void worst_ptofit_btn_Click(object sender, EventArgs e)
        {
            if (Selected_portfail.BoundsList.Count > 0)
            {
                Bound_Conclav bc;
                switch (SeeHistory)
                {
                    case SeeHistory.NoHistrory:
                        var a = Selected_portfail.Bounds_Now.Min();
                        bc = Selected_portfail.BoundsList.Where(t => t.Bound_now == a).Single();
                        break;
                    case SeeHistory.History:
                        var b = Selected_portfail.Bounds_Sold.Min();
                        bc = Selected_portfail.BoundsList.Where(t => t.Bound_sold == b).Single();
                        break;
                    case SeeHistory.WithHistory:
                    default:
                        var c = Selected_portfail.Bounds_Now.Min();
                        var d = Selected_portfail.Bounds_Sold.Min();
                        if (c.Profit_perc < d.Profit_perc)
                            bc = Selected_portfail.BoundsList.Where(t => t.Bound_now == c).Single();
                        else
                            bc = Selected_portfail.BoundsList.Where(t => t.Bound_sold == d).Single();
                        break;

                }
                BoundWatchCntrlFrm bwf_worst = new BoundWatchCntrlFrm(bc, SeeHistory);
                bwf_worst.Tag = $"{bc.Bound.Base.Ticker}";
                bwf_worst.Show();
            }
        }
        //private void change_history_btn_Click(object sender, EventArgs e)
        //{
        //    ViewForm vfm = new ViewForm(Program.InnerAccount.Selected_portfail_backup, SeeHistory.History);
        //    vfm.Show();
        //}
    }
}
