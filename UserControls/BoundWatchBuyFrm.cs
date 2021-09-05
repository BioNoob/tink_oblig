using System.Windows.Forms;
using tink_oblig.classes;

namespace tink_oblig
{
    public partial class BoundWatchBuyFrm : UserControl
    {
        Bound_now Bound { get; set; }
        public BoundWatchBuyFrm(Bound_now bnd)
        {
            InitializeComponent();
            Bound = bnd;

            buy_avg_nkd_lbl.DataBindings.Add(new Binding("Text", Bound, "Buy_avg_nkd", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            buy_avg_market_price_lbl.DataBindings.Add(new Binding("Text", Bound, "Buy_avg_market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            buy_avg_lbl.DataBindings.Add(new Binding("Text", Bound, "Avg_buy", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            buy_sum_nkd_lbl.DataBindings.Add(new Binding("Text", Bound, "Buy_summ_nkd", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            buy_sum_market_price_lbl.DataBindings.Add(new Binding("Text", Bound, "Buy_summ_market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            buy_sum_lbl.DataBindings.Add(new Binding("Text", Bound, "Summ_buy", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));


            sum_in_lbl.DataBindings.Add(new Binding("Text", Bound, "Summ_buy", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            diff_price_total_lbl.DataBindings.Add(new Binding("Text", Bound, "Diff_Price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            diff_price_total_lbl.DataBindings.Add(new Binding("ForeColor", Bound, "Font_diff_clr", true, DataSourceUpdateMode.OnPropertyChanged));
            market_price_now_lbl.DataBindings.Add(new Binding("Text", Bound, "Bound.Market_price_total", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            nkd_now_lbl.DataBindings.Add(new Binding("Text", Bound, "Bound.Nkd_for_now_sum", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            avg_in_lbl.DataBindings.Add(new Binding("Text", Bound, "Avg_buy", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            market_price_avg_now_lbl.DataBindings.Add(new Binding("Text", Bound, "Bound.Market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            nkd_now_avg_lbl.DataBindings.Add(new Binding("Text", Bound, "Bound.Nkd_for_now", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            diff_price_avg_lbl.DataBindings.Add(new Binding("Text", Bound, "Diff_Price_avg", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            diff_price_avg_lbl.DataBindings.Add(new Binding("ForeColor", Bound, "Font_diff_clr", true, DataSourceUpdateMode.OnPropertyChanged));


            profit_sum_lbl.DataBindings.Add(new Binding("Text", Bound, "Profit_string", true, DataSourceUpdateMode.OnPropertyChanged));
            profit_prc_lbl.DataBindings.Add(new Binding("Text", Bound, "Profit_perc_string", true, DataSourceUpdateMode.OnPropertyChanged));
            profit_sum_lbl.DataBindings.Add(new Binding("ForeColor", Bound, "Font_profit_clr", true, DataSourceUpdateMode.OnPropertyChanged));
            profit_prc_lbl.DataBindings.Add(new Binding("ForeColor", Bound, "Font_profit_clr", true, DataSourceUpdateMode.OnPropertyChanged));
            buyback_sum_lb.DataBindings.Add(new Binding("Text", Bound, "Buy_Back_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            commision_lbl.DataBindings.Add(new Binding("Text", Bound, "Broker_comission_total", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));


        }
    }
}
