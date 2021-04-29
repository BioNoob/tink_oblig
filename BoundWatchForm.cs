using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using tink_oblig.classes;

namespace tink_oblig
{
    public partial class BoundWatchForm : Form
    {
        Bound _Bnb { get; set; }
        public BoundWatchForm(Bound bnb)
        {
            InitializeComponent();
            _Bnb = bnb;
        }

        private void BoundWatchForm_Load(object sender, EventArgs e)
        {
            name_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Name"));
            ticker_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Ticker"));
            count_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Lots", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
            nominal_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Nominal", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F1"));

            price_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Price_now_total_avg", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            pic_box_pb.DataBindings.Add(new Binding("Image", _Bnb, "Img_exct", true));

            avg_one_price_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Price_now_one_avg", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            avg_sum_price_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Price_now_total_avg", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            market_one_price_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Price_now_one_market", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            market_sum_price_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Price_now_total_market", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            prev_pay_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Prev_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
            next_pay_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Next_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
            end_pay_lbl.DataBindings.Add(new Binding("Text", _Bnb, "End_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));

            cpn_perc_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Cpn_Percent", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            cpn_val_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Cpn_val", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            last_cpn_val_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Last_Coupon_payed", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            cpn_sum_val_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Coupon_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            pay_period_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Pay_period", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
            nkd_one_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Nkd_one", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            nkd_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Nkd_sum", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            profit_perc_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Profit_summ_perc_string", true, DataSourceUpdateMode.OnPropertyChanged));
            profit_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Profit_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            profit_perc_lbl.DataBindings.Add(new Binding("ForeColor", _Bnb, "Font_profit_clr", true, DataSourceUpdateMode.OnPropertyChanged));
            profit_sum_lbl.DataBindings.Add(new Binding("ForeColor", _Bnb, "Font_profit_clr", true, DataSourceUpdateMode.OnPropertyChanged));
            //trade_event_lst;

            cpn_tax_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Coupon_Tax_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            buy_back_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Buy_Back_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(pic_box_pb.DisplayRectangle);
            pic_box_pb.Region = new Region(gp);
        }
    }
}
