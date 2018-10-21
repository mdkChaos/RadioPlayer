using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioPlayer.Models
{
    public class PlayList
    {
        public PlayList(string time, string artist, string song)
        {
            Time = time;
            Artist = artist;
            Song = song;
        }
        public string Time { get; set; }
        public string Artist { get; set; }
        public string Song { get; set; }
    }
}
