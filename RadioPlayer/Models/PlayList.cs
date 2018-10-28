using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RadioPlayer.Models
{
    public class PlayList : INotifyPropertyChanged
    {
        string time;
        string artist;
        string song;
        string info = "Online Radio";
        public PlayList()
        {

        }
        public PlayList(string time, string artist, string song)
        {
            Time = time;
            Artist = artist;
            Song = song;
        }
        public string Time
        {
            get => time;
            set => time = value;
        }
        public string Artist
        {
            get => artist;
            set
            {
                artist = value;
                OnPropertyChanged("Artist");
            }
        }
        public string Song
        {
            get => song;
            set
            {
                song = value;
                OnPropertyChanged("Song");
            }
        }
        public string Info
        {
            get => info;
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
