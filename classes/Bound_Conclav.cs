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

        public Bound Bound { get; set; }

        public Bound_Conclav(Bound bnb)
        {
            Bound = bnb;
            Bound_now = new Bound_now(Bound);
            Bound_sold = new Bound_sold(Bound);
        }
    }
}
