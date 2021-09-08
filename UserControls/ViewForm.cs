using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using tink_oblig.classes;
using tink_oblig.Properties;
using static tink_oblig.classes.Accounts;

namespace tink_oblig
{
    public partial class ViewForm : UserControl
    {
        public Bounds Selected_portfail { get; set; }
        public int LastSelectedIndx { get { return sorting_mode_cmb.SelectedIndex; } }
        SeeHistory SeeHistory { get; set; }
        public ViewForm(Bounds acc, SeeHistory history)
        {
            //Program.InnerAccount.LoadInfoDone += InnerAccount_LoadInfoDone;
            InitializeComponent();
            sorting_mode_cmb.SelectedIndex = Settings.Default.SortMode;
             //ГРУЗИТЬ СЕЙВИТЬ!
            Switch_representation(acc, history);
        }
        private int Call_cnt = 0;
        public void Switch_representation(Bounds acc, SeeHistory history)
        {
            Call_cnt = 0;
            BoundListLayPannel.Controls.Clear();
            BoundListLayPannel.RowCount = 1;
            BoundListLayPannel.RowStyles.Clear();
            BoundListLayPannel.AutoScroll = false;
            BoundListLayPannel.AutoScroll = true;
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
            foreach (Control c in panel1.Controls)
            {
                if (c is TextBox)
                {
                    c.DataBindings.Clear();
                }
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
            sorting_mode_cmb_SelectedIndexChanged(null,new EventArgs());
        }

        private void LoadBounds(Bound_Conclav bd)
        {
            Call_cnt++;
            BoundListLayPannel.RowCount++;
            BoundListLayPannel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            ListBoundWatch newOne = new ListBoundWatch(bd, SeeHistory);
            newOne.Tag = bd;
            newOne.BorderStyle = BorderStyle.None;
            newOne.Dock = DockStyle.Top;
            BoundListLayPannel.Controls.Add(newOne, 0, BoundListLayPannel.RowCount);
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
                BoundWatchCntrlFrm bwf_better = new BoundWatchCntrlFrm(bc, SeeHistory);
                Program.ShowedForms.Add(new TagWatcher(bc.Bound.Base.Name, SeeHistory));
                //bwf_better.Tag = $"{bc.Bound.Base.Ticker}";
                bwf_better.Show();
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
                Program.ShowedForms.Add(new TagWatcher(bc.Bound.Base.Name, SeeHistory));
                //bwf_worst.Tag = Program.ShowedForms.Last();//$"{bc.Bound.Base.Ticker}";
                bwf_worst.Show();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void sorting_mode_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BoundListLayPannel.Controls.Count < 2)
                return;
            List<ListBoundWatch> lbwl = new List<ListBoundWatch>();
            foreach (var item in BoundListLayPannel.Controls)
            {
                lbwl.Add(item as ListBoundWatch);
            }
            switch (sorting_mode_cmb.SelectedIndex)
            {
                case 0:
                    switch (SeeHistory)
                    {
                        case SeeHistory.NoHistrory:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Bound_now.Profit_perc).ToList();
                            break;
                        case SeeHistory.History:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Bound_sold.Profit_perc).ToList();
                            break;
                        case SeeHistory.WithHistory:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Profit_perc).ToList();
                            break;
                    }

                    break;
                case 1:
                    switch (SeeHistory)
                    {
                        case SeeHistory.NoHistrory:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound_now.Profit_perc).ToList();
                            break;
                        case SeeHistory.History:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound_sold.Profit_perc).ToList();
                            break;
                        case SeeHistory.WithHistory:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Profit_perc).ToList();
                            break;
                    }
                    break;
                case 2:
                    switch (SeeHistory)
                    {
                        case SeeHistory.NoHistrory:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Bound_now.Open_Date).ToList();
                            break;
                        case SeeHistory.History:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Bound_sold.Open_Date).ToList();
                            break;
                        case SeeHistory.WithHistory:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Bound_now.Open_Date).ToList();
                            break;
                    }
                    break;
                case 3:
                    switch (SeeHistory)
                    {
                        case SeeHistory.NoHistrory:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound_now.Open_Date).ToList();
                            break;
                        case SeeHistory.History:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound_sold.Open_Date).ToList();
                            break;
                        case SeeHistory.WithHistory:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound_now.Open_Date).ToList();
                            break;
                    }
                    break;
                case 4:
                    switch (SeeHistory)
                    {
                        case SeeHistory.NoHistrory:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Bound_now.Cnt_buy).ToList();
                            break;
                        case SeeHistory.History:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Bound_sold.Cnt_buy).ToList();
                            break;
                        case SeeHistory.WithHistory:
                            lbwl = lbwl.OrderBy(t => t._Bnb.Cnt_sum).ToList();
                            break;
                    }
                    break;
                case 5:
                    switch (SeeHistory)
                    {
                        case SeeHistory.NoHistrory:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound_now.Cnt_buy).ToList();
                            break;
                        case SeeHistory.History:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound_sold.Cnt_buy).ToList();
                            break;
                        case SeeHistory.WithHistory:
                            lbwl = lbwl.OrderByDescending(t => t._Bnb.Cnt_sum).ToList();
                            break;
                    }
                    break;
                case 6:
                    lbwl = lbwl.OrderBy(t => t._Bnb.Bound.Cpn_Percent).ToList();
                    break;
                case 7:
                    lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound.Cpn_Percent).ToList();
                    break;
                case 8:
                    lbwl = lbwl.OrderBy(t => t._Bnb.Bound.Next_pay_dt).ToList();
                    break;
                case 9:
                    lbwl = lbwl.OrderByDescending(t => t._Bnb.Bound.Next_pay_dt).ToList();
                    break;
            }
            BoundListLayPannel.Controls.Clear();
            int a = 1;
            foreach (var item in lbwl)
            {
                BoundListLayPannel.Controls.Add(item, 0, a);
                a++;
            }
        }
        //private void change_history_btn_Click(object sender, EventArgs e)
        //{
        /*
        Доходность ▲ 0
Доходность ▼ 1
Дата покупки ▲ 2
Дата покупки ▼ 3
Количество ▲ 4
Количество ▼ 5
Процент купона ▲ 6
Процент купона ▼ 7

        */
        //    ViewForm vfm = new ViewForm(Program.InnerAccount.Selected_portfail_backup, SeeHistory.History);
        //    vfm.Show();
        //}
    }
}
