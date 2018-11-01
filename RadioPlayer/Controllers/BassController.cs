using RadioPlayer.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Threading;
using System.Xml;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;

namespace RadioPlayer.Controllers
{
    public class BassController : INotifyPropertyChanged
    {
        MainWindow mainWindow;

        public ObservableCollection<Radio> Radios { get; private set; }

        public ObservableCollection<Song> PlayLists { get; private set; } = new ObservableCollection<Song>();

        public Song SongInfo
        {
            get
            {
                return songInfo;
            }
            private set
            {
                songInfo = value;
                OnPropertyChanged("GetInfo");
            }
        }

        public string URL { get; set; }
        /// <summary>
        /// Частота дискретизации
        /// </summary>
        private int HZ { get; set; } = 44100;
        /// <summary>
        /// Состояние инициализации
        /// </summary>
        public bool InitDefaultDevice { get; set; }
        /// <summary>
        /// Канал
        /// </summary>
        public int Stream { get; set; }
        /// <summary>
        /// Громкость
        /// </summary>
        public int Volume { get; set; } = 100;

        public BassController()
        {
            UpdateRadioList();
        }
        public BassController(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            UpdateRadioList();
            SongInfo = new Song();
        }

        private TAG_INFO tagInfo;
        private SYNCPROC mySync;
        Song songInfo;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void UpdateRadioList()
        {
            Radios = GetRadios();
        }

        private ObservableCollection<Radio> GetRadios()
        {
            ObservableCollection<Radio> radios = new ObservableCollection<Radio>();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Environment.CurrentDirectory + "\\Data\\RadioList.xml");
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            foreach (XmlElement xmlElement in xmlRoot)
            {
                Radio radio = new Radio();
                foreach (XmlNode xmlNode in xmlElement.ChildNodes)
                {
                    if (xmlNode.Name == "Name")
                    {
                        radio.Name = xmlNode.InnerText;
                    }
                    if (xmlNode.Name == "URL")
                    {
                        radio.URL = xmlNode.InnerText;
                    }
                    if (xmlNode.Name == "Icon")
                    {
                        radio.Icon = Environment.CurrentDirectory + xmlNode.InnerText;
                    }
                }
                radios.Add(radio);
            }

            return radios;
        }

        /// <summary>
        /// Инициализация Bass.dll
        /// </summary>
        /// <param name="hz">Частота дискретизации</param>
        /// <returns>Состояние инициализации</returns>
        private bool InitBass(int hz)
        {
            if (!InitDefaultDevice)
            {
                //BassNet.Registration("mdkchaos1@gmail.com", "2X229119152222");
                InitDefaultDevice = Bass.BASS_Init(-1, hz, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            }
            return InitDefaultDevice;
        }

        public void Play(int volume)
        {
            Stop();
            if (InitBass(HZ))
            {
                Stream = Bass.BASS_StreamCreateURL(URL, 0, BASSFlag.BASS_DEFAULT, null, IntPtr.Zero);

                tagInfo = new TAG_INFO(URL);

                if (BassTags.BASS_TAG_GetFromURL(Stream, tagInfo))
                {
                    UpdatePlayList();
                }

                mySync = new SYNCPROC(MetaSync);
                Bass.BASS_ChannelSetSync(Stream, BASSSync.BASS_SYNC_META, 0, mySync, IntPtr.Zero);

                Bass.BASS_ChannelPlay(Stream, false);
                //Установка громкости
                Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, volume / 100f);
            }
        }

        private void MetaSync(int handle, int channel, int data, IntPtr user)
        {
            // BASS_SYNC_META is triggered on meta changes of SHOUTcast streams
            if (tagInfo.UpdateFromMETA(Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_META), false, true))
            {
                UpdatePlayList();
            }
        }

        private void UpdatePlayList()
        {
            SongInfo = new Song(
                DateTime.Now.ToString("HH:mm"),
                tagInfo.artist,
                tagInfo.title,
                tagInfo.album,
                tagInfo.comment,
                tagInfo.genre,
                tagInfo.year,
                tagInfo.artist + " - " + tagInfo.title);
            mainWindow.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                PlayLists.Add(SongInfo);
            }));
        }

        /// <summary>
        /// Остановка воспроизведения
        /// </summary>
        public void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
            if (PlayLists != null)
            {
                PlayLists.Clear();
            }
        }

        /// <summary>
        /// Установка громкости
        /// </summary>
        /// <param name="stream">Канал</param>
        /// <param name="volume">Громкость</param>
        public void SetVolumeToStream(int stream, int volume)
        {
            Volume = volume;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100f);
        }
    }
}