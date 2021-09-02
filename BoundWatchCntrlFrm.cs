using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
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
                    count_lbl.DataBindings.Add(new Binding("Text", Bound, "Base.Lots", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    price_sum_lbl.DataBindings.Add(new Binding("Text", Bound_n, "Buy_summ_market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    show_panel.Controls.Clear();
                    show_panel.Controls.Add(new BoundWatchBuyFrm(Bound_n) { Dock = DockStyle.Fill });
                    this.Text = Bound_Cnc.Bound.Base.Name;
                    break;
                case SeeHistory.History:
                    last_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Last_Coupon_payed", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    total_cnt_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Coupon_cnt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    sum_coupon_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Coupon_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    coupon_tax_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Coupon_Tax_summ", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    count_lbl.DataBindings.Add(new Binding("Text", Bound, "Base.Lots", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    price_sum_lbl.DataBindings.Add(new Binding("Text", Bound_s, "Buy_summ_market_price", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    show_panel.Controls.Clear();
                    show_panel.Controls.Add(new BoundWatchSellFrm(Bound_s) { Dock = DockStyle.Fill });
                    this.Text = Bound_Cnc.Bound.Base.Name;
                    break;
            }

        }
    }
}
