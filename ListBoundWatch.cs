using System;
using System.Drawing;
using System.Windows.Forms;
using tink_oblig.classes;
using System.Linq;

namespace tink_oblig
{
    public partial class ListBoundWatch : UserControl
    {
        Bound _Bnb;
        public ListBoundWatch(Bound bnb)
        {
            InitializeComponent();
            _Bnb = bnb;
            this.BackColor = Color.FromArgb(126, _Bnb.Img_color);
            foreach (Control item in this.Controls)
            {
                item.DoubleClick += ListBoundWatch_DoubleClick;
            }
        }

        private void ListBoundWatch_Load(object sender, EventArgs e)
        {
            name_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Name"));
            ticker_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Ticker"));
            count_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Lots", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
            cpn_perc_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Cpn_Percent", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            cpn_val_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Cpn_val", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            next_pay_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Next_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
            price_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Price_now_total_avg", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
        }

        private void ListBoundWatch_DoubleClick(object sender, EventArgs e)
        {
            foreach (Form item in Application.OpenForms)
            {
                if (item.Visible)
                    if (item.Tag != null)
                        if (!string.IsNullOrEmpty(item.Tag.ToString()))
                            if (item.Tag.ToString() == _Bnb.Base.Ticker)
                            {
                                item.Focus();
                                return;
                            }
                                
            }
            BoundWatchCntrlFrm bwf = new BoundWatchCntrlFrm(_Bnb);
            bwf.StartPosition = FormStartPosition.CenterParent;
            bwf.Tag = $"{_Bnb.Base.Ticker}";
            bwf.Show();
        }
    }
}
