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
            //name_active_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Name"));
            //ticker_lbl.DataBindings.Add(new Binding("Text", _Bnb, "Ticker"));
        }
    }
}
