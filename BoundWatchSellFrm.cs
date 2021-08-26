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
    public partial class BoundWatchSellFrm : UserControl
    {
        Bound_sold Bound { get; set; }
        public BoundWatchSellFrm(Bound_sold bnd)
        {
            InitializeComponent();
            Bound = bnd;
        }

    }
}
