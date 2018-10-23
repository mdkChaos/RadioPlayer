using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RadioPlayer.Models
{
    public class Radio
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
