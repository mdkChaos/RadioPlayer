using RadioPlayer.Controllers;
using RadioPlayer.Models;
using RadioPlayer.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RadioPlayer
{
    public partial class MainWindow : Window
    {
        BassController bassController;
        WindowState prevState;
        public MainWindow()
        {
            InitializeComponent();
            bassController = new BassController(this);
            DataContext = bassController;
        }

        private void RadioStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadioIcon.ImageSource = null;
            if (RadioStation.SelectedItem is Radio radio)
            {
                if (radio.Icon != string.Empty && radio.Icon != null)
                {
                    RadioIcon.ImageSource = new BitmapImage(new Uri(radio.Icon, UriKind.RelativeOrAbsolute));
                }
                bassController.URL = RadioStation.SelectedValue.ToString();
            }
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
        private void Volume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bassController.SetVolumeToStream(bassController.Stream, (int)Volume.Value);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            bassController.Stop();
            Close();
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditWindow edit = new EditWindow();
            edit.ShowDialog();
            var temp = RadioStation.SelectedValue;
            bassController.UpdateRadioList();
            RadioStation.ItemsSource = bassController.Radios;
            RadioStation.SelectedValue = temp;
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();
            else
                prevState = WindowState;
        }

        private void TaskbarIcon_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            Show();
            WindowState = prevState;
            Focusable = true;
        }
    }
}
