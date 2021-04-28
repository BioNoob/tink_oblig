using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using tink_oblig.classes;

namespace tink_oblig
{
    public partial class ListBoundWatch : UserControl
    {
        Bound _Bnb;
        public ListBoundWatch(Bound bnb)
        {
            InitializeComponent();
            _Bnb = bnb;
        }

        private void ListBoundWatch_Load(object sender, EventArgs e)
        {
            name_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Name"));
            ticker_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Ticker"));
            count_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Base.Lots", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
            cpn_perc_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Cpn_Percent", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            cpn_val_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Cpn_val",true,DataSourceUpdateMode.OnPropertyChanged,0m,"F2"));
            next_pay_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Next_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
            price_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Price_now_total_avg", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            //pictureBox1.DataBindings.Add(new Binding("Image", _Bnb, "Img_exct", true));
        }

        private void ListBoundWatch_DoubleClick(object sender, EventArgs e)
        {
            BoundWatchForm bwf = new BoundWatchForm(_Bnb);
            bwf.Show();
        }
    }
}
