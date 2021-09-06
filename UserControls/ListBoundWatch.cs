using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using tink_oblig.classes;
using static tink_oblig.classes.Accounts;

namespace tink_oblig
{
    public partial class ListBoundWatch : UserControl
    {
        Bound_Conclav _Bnb;
        SeeHistory See;
        public ListBoundWatch(Bound_Conclav bnb, SeeHistory see)
        {
            InitializeComponent();
            _Bnb = bnb;
            See = see;
            this.BackColor = Color.FromArgb(126, _Bnb.Bound.Img_color);
            foreach (Control item in this.Controls)
            {
                item.DoubleClick += ListBoundWatch_DoubleClick;
            }
        }

        private void ListBoundWatch_Load(object sender, EventArgs e)
        {
            name_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Bound.Base.Name"));
            ticker_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Bound.Base.Ticker"));

            cpn_perc_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Bound.Cpn_Percent", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            cpn_val_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Bound.Cpn_val", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
            next_pay_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Bound.Next_pay_dt", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "dd.MM.yyyy"));
            switch (See)
            {
                case SeeHistory.NoHistrory:
                    price_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb.Bound_now, "Bound.Market_price_total", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    count_lbl.DataBindings.Add(new Binding("Text", _Bnb.Bound_now, "Cnt_buy", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    break;
                case SeeHistory.History:
                    price_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb.Bound_sold, "Summ_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    count_lbl.DataBindings.Add(new Binding("Text", _Bnb.Bound_sold, "Cnt_sell", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    break;
                case SeeHistory.WithHistory:
                    count_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Cnt_sum", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "D"));
                    price_sum_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Money_sum", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
                    break;
            }

        }
        private void ListBoundWatch_DoubleClick(object sender, EventArgs e)
        {
            foreach (Form item in Application.OpenForms)
            {
                if (item.Visible)
                    if (item.Tag != null)
                        if (((TagWatcher)item.Tag).Name == _Bnb.Bound.Base.Name)
                        {
                            item.Focus();
                            return;
                        }

            }
            if (See == SeeHistory.WithHistory)
            {
                BoundWatchCntrlFrm bwf1 = new BoundWatchCntrlFrm(_Bnb, SeeHistory.NoHistrory);
                bwf1.StartPosition = FormStartPosition.WindowsDefaultLocation;
                //bwf1.Tag = Program.ShowedForms.Last();//$"{_Bnb.Bound.Base.Ticker}";
                //bwf1.Location = new Point(Program.mf.Location.X + Program.mf.Size.Width + 211 * Program.ShowedForms.Last().Index, Program.mf.Location.Y + 10 * Program.ShowedForms.Last().Index);
                bwf1.Show();
                BoundWatchCntrlFrm bwf = new BoundWatchCntrlFrm(_Bnb, SeeHistory.History);
                bwf.StartPosition = FormStartPosition.Manual;
                bwf.Location = new Point(bwf1.Location.X + bwf1.Width + 10, bwf1.Location.Y);
                //bwf.Tag = Program.ShowedForms.Last();//$"{_Bnb.Bound.Base.Ticker}";
                bwf.Show();
            }
            else
            {
                BoundWatchCntrlFrm bwf1 = new BoundWatchCntrlFrm(_Bnb, See);
                bwf1.StartPosition = FormStartPosition.WindowsDefaultLocation;
                //bwf1.Location = new Point(Program.mf.Location.X + Program.mf.Size.Width + 211 * Program.ShowedForms.Last().Index, Program.mf.Location.Y + 10 * Program.ShowedForms.Last().Index);
                //bwf1.Tag = Program.ShowedForms.Last();//$"{_Bnb.Bound.Base.Ticker}";
                bwf1.Show();
            }
        }
    }
}
