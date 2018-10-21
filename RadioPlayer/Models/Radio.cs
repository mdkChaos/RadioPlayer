﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioPlayer.Models
{
    class Radio
    {
        public Radio()
        {

        }
        public Radio(string name, string url, string icon = null)
        {
            Name = name;
            URL = url;
            Icon = icon;
        }
        public string Name { get; set; }
        public string URL { get; set; }
        public string Icon { get; set; }
    }
}