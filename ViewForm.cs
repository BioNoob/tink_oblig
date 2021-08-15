using System;
using System.Collections.Generic;
using System.Windows.Forms;
using tink_oblig.classes;
using Tinkoff.Trading.OpenApi.Models;
using static tink_oblig.classes.Accounts;

namespace tink_oblig
{
    public partial class ViewForm : Form
    {
        public Bounds Selected_portfail { get; set; }
        SeeHistory SeeHistory { get; set; }
        public ViewForm(Bounds acc, SeeHistory history)
        {
            //Program.InnerAccount.LoadInfoDone += InnerAccount_LoadInfoDone;
            InitializeComponent();
            SeeHistory = history;
            Selected_portfail = acc;
            foreach (var bd in Selected_portfail.BoundsList)
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

        }
    }
}
