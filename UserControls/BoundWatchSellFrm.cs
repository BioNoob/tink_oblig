using System.Windows.Forms;
using tink_oblig.classes;

namespace tink_oblig
{
    public partial class BoundWatchSellFrm : UserControl
    {
        Bound_sold Bound { get; set; }
        public BoundWatchSellFrm(Bound_sold bnd)
        {
            InitializeComponent();
            Bound = bnd;


            buy_avg_nkd_lbl.DataBindings.Add(new Binding("Text", Bound, "Buy_avg_nkd", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            buy_avg_market_price_lbl.DataBindings.Add(new Binding("Text", Bound, "Buy_avg_market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            buy_avg_lbl.DataBindings.Add(new Binding("Text", Bound, "Avg_buy", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            buy_sum_nkd_lbl.DataBindings.Add(new Binding("Text", Bound, "Buy_summ_nkd", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            buy_sum_market_price_lbl.DataBindings.Add(new Binding("Text", Bound, "Buy_summ_market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            buy_sum_lbl.DataBindings.Add(new Binding("Text", Bound, "Summ_buy", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            sell_avg_diff_lbl.DataBindings.Add(new Binding("Text", Bound, "Diff_sell_avg", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            sell_avg_lbl.DataBindings.Add(new Binding("Text", Bound, "Summ_sell_avg", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            sell_avg_nkd_lbl.DataBindings.Add(new Binding("Text", Bound, "Nkd_avg_by_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            sell_avg_market_price_lbl.DataBindings.Add(new Binding("Text", Bound, "Market_price_avg_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            sell_avg_diff_lbl.DataBindings.Add(new Binding("ForeColor", Bound, "Font_diff_clr", true, DataSourceUpdateMode.OnPropertyChanged));

            sell_sum_diff_lbl.DataBindings.Add(new Binding("Text", Bound, "Diff_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            sell_sum_lbl.DataBindings.Add(new Binding("Text", Bound, "Summ_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            sell_sum_nkd_lbl.DataBindings.Add(new Binding("Text", Bound, "Nkd_summ_by_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            sell_sum_market_price_lbl.DataBindings.Add(new Binding("Text", Bound, "Market_price_summ_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            sell_sum_diff_lbl.DataBindings.Add(new Binding("ForeColor", Bound, "Font_diff_clr", true, DataSourceUpdateMode.OnPropertyChanged));


            profit_sum_lbl.DataBindings.Add(new Binding("Text", Bound, "Profit_string", true, DataSourceUpdateMode.OnPropertyChanged));
            profit_prc_lbl.DataBindings.Add(new Binding("Text", Bound, "Profit_perc_string", true, DataSourceUpdateMode.OnPropertyChanged));
            profit_sum_lbl.DataBindings.Add(new Binding("ForeColor", Bound, "Font_profit_clr", true, DataSourceUpdateMode.OnPropertyChanged));
            profit_prc_lbl.DataBindings.Add(new Binding("ForeColor", Bound, "Font_profit_clr", true, DataSourceUpdateMode.OnPropertyChanged));

            buyback_sum_lb.DataBindings.Add(new Binding("Text", Bound, "Buy_Back_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            commision_lbl.DataBindings.Add(new Binding("Text", Bound, "Broker_comission_total", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            close_dt_lbl.DataBindings.Add(new Binding("Text", Bound, "Close_Date", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy")); 
            close_cnt_lbl.DataBindings.Add(new Binding("Text", Bound, "Close_cnt_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
        }

    }
}
