using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace tink_oblig.classes
{
    public class Bound_Conclav : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Bound_now Bound_now { get; set; }
        public Bound_sold Bound_sold { get; set; }

        public Bound_Conclav(Bound_sold bns,Bound_now bnn)
        {
            Bound_now = bnn;
            Bound_sold = bns;
        }
    }
}
