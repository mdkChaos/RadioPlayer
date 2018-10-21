using RadioPlayer.Controllers;
using System.Windows;
using System.Windows.Controls;

namespace RadioPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BassController bassController;
        public MainWindow()
        {
            InitializeComponent();
            bassController = new BassController(this);
            DataContext = bassController;
        }

        private void RadioStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bassController.URL = (string)RadioStation.SelectedValue;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (RadioStation.SelectedValue != null)
            {
                int volume = 100;
                bassController.Play(volume);
                Volume.Value = volume;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберете радиостанцию!", "Не выбрана радиостанция!");
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            bassController.Stop();
            Volume.Value = 0;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bassController.SetVolumeToStream(bassController.Stream, (int)Volume.Value);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            bassController.Stop();
            Close();
        }

        private void Added_Click(object sender, RoutedEventArgs e)
        {
            AddWindows add = new AddWindows();
            add.ShowDialog();
        }
    }
}
