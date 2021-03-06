using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using tink_oblig.classes;
using static tink_oblig.classes.Accounts;

namespace tink_oblig
{
    public partial class BoundWatchCntrlFrm : Form
    {
        Bound_Conclav Bound_Cnc { get; set; }
        Bound_sold Bound_s { get { return Bound_Cnc.Bound_sold; } }
        Bound_now Bound_n { get { return Bound_Cnc.Bound_now; } }
        Bound Bound { get { return Bound_Cnc.Bound; } }
        public SeeHistory Mode { get; set; }
        public BoundWatchCntrlFrm(Bound_Conclav bnc, SeeHistory mod = SeeHistory.WithHistory)
        {
            InitializeComponent();
            Bound_Cnc = bnc;
            Mode = mod;
            Program.ShowedForms.Add(new TagWatcher(Bound_Cnc.Bound.Base.Name, Mode));
            Tag = Program.ShowedForms.Last();
            name_lbl.DataBindings.Add(new Binding("Text", Bound, "Base.Name", true, DataSourceUpdateMode.OnPropertyChanged));
            ticker_lbl.DataBindings.Add(new Binding("Text", Bound, "Base.Ticker", true, DataSourceUpdateMode.OnPropertyChanged));
            pic_box_pb.DataBindings.Add(new Binding("Image", Bound, "Img_exct", true));
            nominal_lbl.DataBindings.Add(new Binding("Text", Bound, "Nominal", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F1"));
            pay_period_lbl.DataBindings.Add(new Binding("Text", Bound, "Pay_period", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));

            prev_pay_lbl.DataBindings.Add(new Binding("Text", Bound, "Prev_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
            next_pay_lbl.DataBindings.Add(new Binding("Text", Bound, "Next_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
            end_pay_lbl.DataBindings.Add(new Binding("Text", Bound, "End_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));

            coupon_prc_lbl.DataBindings.Add(new Binding("Text", Bound, "Cpn_Percent", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            coupon_val_lbl.DataBindings.Add(new Binding("Text", Bound, "Cpn_val", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));


            offert_date_lbl.DataBindings.Add(new Binding("Enabled", Bound, "HaveOffert", true, DataSourceUpdateMode.OnPropertyChanged));
            offert_pay_lbl.DataBindings.Add(new Binding("Enabled", Bound, "HaveOffert", true, DataSourceUpdateMode.OnPropertyChanged));
            offert_pay_prc_lbl.DataBindings.Add(new Binding("Enabled", Bound, "HaveOffert", true, DataSourceUpdateMode.OnPropertyChanged));
            offert_pay_sum_lbl.DataBindings.Add(new Binding("Enabled", Bound, "HaveOffert", true, DataSourceUpdateMode.OnPropertyChanged));
            amort_date_lbl.DataBindings.Add(new Binding("Enabled", Bound, "HaveAmort", true, DataSourceUpdateMode.OnPropertyChanged));
            amort_pay_lbl.DataBindings.Add(new Binding("Enabled", Bound, "HaveAmort", true, DataSourceUpdateMode.OnPropertyChanged));
            amort_pay_prc_lbl.DataBindings.Add(new Binding("Enabled", Bound, "HaveAmort", true, DataSourceUpdateMode.OnPropertyChanged));
            amort_pay_sum_lbl.DataBindings.Add(new Binding("Enabled", Bound, "HaveAmort", true, DataSourceUpdateMode.OnPropertyChanged));
            if (Bound.HaveOffert)
            {
                offert_date_lbl.DataBindings.Add(new Binding("Text", Bound.ClouserOffert, "offerdate", true, DataSourceUpdateMode.OnPropertyChanged, "", "dd.MM.yyyy"));
                offert_pay_lbl.DataBindings.Add(new Binding("Text", Bound.ClouserOffert, "facevalue", true, DataSourceUpdateMode.OnPropertyChanged, "", "F2"));
                offert_pay_prc_lbl.DataBindings.Add(new Binding("Text", Bound, "OffertPerc", true, DataSourceUpdateMode.OnPropertyChanged, "", "F1"));
            }
            if(Bound.HaveAmort)
            {
                amort_date_lbl.DataBindings.Add(new Binding("Text", Bound.ClouserAmort, "amortdate", true, DataSourceUpdateMode.OnPropertyChanged, "", "dd.MM.yyyy"));
                amort_pay_lbl.DataBindings.Add(new Binding("Text", Bound.ClouserAmort, "value", true, DataSourceUpdateMode.OnPropertyChanged, "", "F2"));
                amort_pay_prc_lbl.DataBindings.Add(new Binding("Text", Bound.ClouserAmort, "valueprc", true, DataSourceUpdateMode.OnPropertyChanged, "", "F1"));
            }


            SwitchType(mod);

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(pic_box_pb.DisplayRectangle);
            pic_box_pb.Region = new Region(gp);


        }
        public void SwitchType(SeeHistory mod)
        {
            Mode = mod;
            switch (Mode)
            {
                case SeeHistory.NoHistrory:
                    last_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_n, "Last_Coupon_payed", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    total_cnt_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_n, "Coupon_cnt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    sum_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_n, "Coupon_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    coupon_tax_lbl.DataBindings.Add(new Binding("Text", Bound_n, "Coupon_Tax_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    count_lbl.DataBindings.Add(new Binding("Text", Bound_n, "Cnt_buy", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    price_sum_lbl.DataBindings.Add(new Binding("Text", Bound_n, "Buy_summ_market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    first_buy_dt_lbl.DataBindings.Add(new Binding("Text", Bound_n, "Open_Date", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
                    amort_pay_sum_lbl.DataBindings.Add(new Binding("Text", Bound_n, "AmortSum", true, DataSourceUpdateMode.OnPropertyChanged, "", "F2"));
                    offert_pay_sum_lbl.DataBindings.Add(new Binding("Text", Bound_n, "OffertSum", true, DataSourceUpdateMode.OnPropertyChanged, "", "F2"));
                    show_panel.Controls.Clear();
                    show_panel.Controls.Add(new BoundWatchBuyFrm(Bound_n) { Dock = DockStyle.Fill });
                    //this.Text = Bound_Cnc.Bound.Base.Name;
                    break;
                case SeeHistory.History:
                    last_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Last_Coupon_payed", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    total_cnt_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Coupon_cnt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    sum_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Coupon_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    coupon_tax_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Coupon_Tax_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    count_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Cnt_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    price_sum_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Buy_summ_market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    first_buy_dt_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Open_Date", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
                    amort_pay_sum_lbl.DataBindings.Add(new Binding("Text", Bound_s, "AmortSum", true, DataSourceUpdateMode.OnPropertyChanged, "", "F2"));
                    offert_pay_sum_lbl.DataBindings.Add(new Binding("Text", Bound_s, "OffertSum", true, DataSourceUpdateMode.OnPropertyChanged, "", "F2"));
                    show_panel.Controls.Clear();
                    show_panel.Controls.Add(new BoundWatchSellFrm(Bound_s) { Dock = DockStyle.Fill });
                    //this.Text = Bound_Cnc.Bound.Base.Name;
                    break;
            }
            this.Text = ((TagWatcher)Tag).ToString();
        }

        private void BoundWatchCntrlFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.RemoveFromShowed((TagWatcher)this.Tag);
        }
    }
}
