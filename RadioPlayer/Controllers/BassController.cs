using RadioPlayer.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Threading;
using System.Xml;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;

namespace RadioPlayer.Controllers
{
    public class BassController
    {
        MainWindow mainWindow;
        ObservableCollection<PlayList> playLists;
        public BassController()
        {
            GetListRadioStations();
        }
        public BassController(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            GetListRadioStations();
        }
        private TAG_INFO tagInfo;
        private SYNCPROC mySync;

        public void GetListRadioStations()
        {
            ObservableCollection<Radio> radios = GetRadios();
            RadioEntries = new CollectionView(radios);
        }
        ObservableCollection<Radio> GetRadios()
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
                        radio.Icon = xmlNode.InnerText;
                    }
                }
                radios.Add(radio);
            }

            return radios;
        }

        public CollectionView RadioEntries { get; private set; }
        public CollectionView RadioListes { get; private set; }
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
            //string fileName = @"C:\Users\mdkch\Downloads\The Beach Boys - California Dreamin.mp3";
            playLists = new ObservableCollection<PlayList>();
            Stop();
            if (InitBass(HZ))
            {
                //Stream = Bass.BASS_StreamCreateFile(fileName, 0, 0, BASSFlag.BASS_DEFAULT);
                Stream = Bass.BASS_StreamCreateURL(URL, 0, BASSFlag.BASS_DEFAULT, null, IntPtr.Zero);

                tagInfo = new TAG_INFO(URL);

                if (BassTags.BASS_TAG_GetFromURL(Stream, tagInfo))
                {
                    UpdateTagDisplay();
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
                UpdateTagDisplay();
            }
        }
        private void UpdateTagDisplay()
        {
            mainWindow.PlayList.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                playLists.Add(new PlayList(DateTime.Now.ToString("HH:mm") + ": ", tagInfo.artist + " - ", tagInfo.title));
                RadioListes = new CollectionView(playLists);
                mainWindow.PlayList.ItemsSource = RadioListes;
            }));
            mainWindow.Artist.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                mainWindow.Artist.Text = tagInfo.artist;
            }));
            mainWindow.Title.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                mainWindow.Title.Text = tagInfo.title;
            }));
            mainWindow.Album.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                mainWindow.Album.Text = tagInfo.album;
            }));
            mainWindow.Comment.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                mainWindow.Comment.Text = tagInfo.comment;
            }));
            mainWindow.Genre.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                mainWindow.Genre.Text = tagInfo.genre;
            }));
            mainWindow.Year.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                mainWindow.Year.Text = tagInfo.year;
            }));
        }

        /// <summary>
        /// Остановка воспроизведения
        /// </summary>
        public void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
            if (playLists != null)
            {
                playLists.Clear();
            }
            mainWindow.PlayList.ItemsSource = null;
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