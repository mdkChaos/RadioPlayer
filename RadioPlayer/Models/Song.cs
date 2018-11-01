using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RadioPlayer.Models
{
    public class Song// : INotifyPropertyChanged
    {
        //string time;
        //string artist;
        //string title;
        //string info = "Online Radio";

        public Song()
        {

        }
        public Song(string time, string artist, string title, string album, string comment, string genre, string year, string info)
        {
            Time = time;
            Artist = artist;
            Title = title;
            Album = album;
            Comment = comment;
            Genre = genre;
            Year = year;
            Info = info;
        }

        public string Time { get; set; }
        public string Artist { get; set; }
        public string Title { get; set; }
        public string Album { get; set; }
        public string Comment { get; set; }
        public string Genre { get; set; }
        public string Year { get; set; }
        public string Info { get; set; } = "Online Radio";

        //public string Time
        //{
        //    get => time;
        //    set => time = value;
        //}
        //public string Artist
        //{
        //    get => artist;
        //    set
        //    {
        //        artist = value;
        //        OnPropertyChanged("Artist");
        //    }
        //}
        //public string Title
        //{
        //    get => title;
        //    set
        //    {
        //        title = value;
        //        OnPropertyChanged("Song");
        //    }
        //}
        //public string Info
        //{
        //    get => info;
        //    set
        //    {
        //        info = value;
        //        OnPropertyChanged("Info");
        //    }
        //}

        //public event PropertyChangedEventHandler PropertyChanged;
        //public void OnPropertyChanged([CallerMemberName]string prop = "")
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        //}
    }
}
