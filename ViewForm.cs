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
    public partial class ViewForm : Form
    {
        public Bounds Selected_portfail { get; set; }
        public ViewForm(Bounds acc)
        {
            InitializeComponent();
            Selected_portfail = acc;
            foreach (var bd in Selected_portfail.BoundsList)
            {
                LoadBounds(bd);
            }
        }
        private void LoadBounds(Bound bd)
        {
            BoundListLayPannel.RowCount++;
            BoundListLayPannel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            ListBoundWatch newOne = new ListBoundWatch(bd);//("Test", "TST","lol", 10, 11, 12, 13, 1));
            newOne.BorderStyle = BorderStyle.None;
            newOne.Dock = DockStyle.Top;
            BoundListLayPannel.Controls.Add(newOne, 0, BoundListLayPannel.RowCount);
        }

        private void ViewForm_Load(object sender, EventArgs e)
        {
            //total_money_lbl.DataBindings.Add(new Binding("Text", Selected_portfail, "SumB_Coast", true, DataSourceUpdateMode.OnPropertyChanged, 0m, "F2"));
        }
    }
}
