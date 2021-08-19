﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using Tinkoff.Trading.OpenApi.Models;

namespace tink_oblig.classes
{
    class Bound_sold : Bound, INotifyPropertyChanged
    {
        public Bound_sold(Portfolio.Position ps) : base(ps)
        {
            Base = ps;
        }


    }
}