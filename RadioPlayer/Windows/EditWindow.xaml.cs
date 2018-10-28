using Microsoft.Win32;
using RadioPlayer.Controllers;
using RadioPlayer.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RadioPlayer.Windows
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        CRUDController controller;
        BassController bassController;
       public EditWindow()
        {
            InitializeComponent();
            controller = new CRUDController(this);
            bassController = new BassController();
            bassController.GetListRadioStations();
            DataContext = bassController;
        }

        void UpdateRadioList()
        {
            bassController.GetListRadioStations();
            RadioStation.ItemsSource = bassController.RadioEntries;
        }
        private void SelectIcon_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "Image files(*.png; *.jpg; *.bmp; *.bmp)|*.png; *.jpg; *.bmp; *.bmp|All files(*.*)|*.*"
            };
            if (openFile.ShowDialog() == true)
            {
                Icon.Source = new BitmapImage(new Uri(openFile.FileName, UriKind.RelativeOrAbsolute));
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            controller.Save();
            UpdateRadioList();
            //bassController.GetListRadioStations();
            //RadioStation.ItemsSource = bassController.RadioEntries;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            controller.Delete();
            UpdateRadioList();
            //bassController.GetListRadioStations();
            //RadioStation.ItemsSource = bassController.RadioEntries;
        }

        private void RadioStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Radio radio = (Radio)RadioStation.SelectedItem;

            Name.Text = radio.Name;
            URL.Text = radio.URL;
            if (radio.Icon!= string.Empty && radio.Icon != null)
            {
                Icon.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + radio.Icon, UriKind.RelativeOrAbsolute));
            }
        }
    }
}
