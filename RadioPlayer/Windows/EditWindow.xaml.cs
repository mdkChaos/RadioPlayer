using Microsoft.Win32;
using RadioPlayer.Controllers;
using RadioPlayer.Models;
using System;
using System.IO;
using System.Windows;
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
        string path;

        public EditWindow()
        {
            InitializeComponent();
            controller = new CRUDController();
            bassController = new BassController();
            DataContext = bassController;
        }

        private void SelectIcon_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage image = controller.SetLogo();
            if (image != null)
            {
                Icon.Source = image;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Icon.Source != null)
            {
                path = "\\Images\\" + Path.GetFileName(Icon.Source.ToString());
            }
            else
            {
                path = string.Empty;
            }
            controller.Radio = new Radio(Name.Text, URL.Text, path);
            controller.Save();
            bassController.UpdateRadioList();
            RadioStation.ItemsSource = bassController.Radios;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Icon.Source != null)
            {
                path = "\\Images\\" + Path.GetFileName(Icon.Source.ToString());
            }
            else
            {
                path = string.Empty;
            }
            controller.Radio = new Radio(Name.Text, URL.Text, path);
            controller.Delete();
            Name.Text = string.Empty;
            URL.Text = string.Empty;
            Icon.Source = null;
            bassController.UpdateRadioList();
            RadioStation.ItemsSource = bassController.Radios;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (Icon.Source != null)
            {
                path = "\\Images\\" + Path.GetFileName(Icon.Source.ToString());
            }
            else
            {
                path = string.Empty;
            }
            controller.Radio = new Radio(Name.Text, URL.Text, path);
            controller.Update();
            bassController.UpdateRadioList();
            RadioStation.ItemsSource = bassController.Radios;
        }
    }
}
