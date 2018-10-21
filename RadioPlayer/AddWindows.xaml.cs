using Microsoft.Win32;
using RadioPlayer.Controllers;
using RadioPlayer.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RadioPlayer
{
    /// <summary>
    /// Interaction logic for CRUDWindows.xaml
    /// </summary>
    public partial class AddWindows : Window
    {
        CRUDController controller;
        public AddWindows()
        {
            InitializeComponent();
            controller = new CRUDController();
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
            Radio radio = new Radio();
            if (Name.Text.Trim() != string.Empty)
            {
                radio.Name = Name.Text.Trim();
            }
            if (URL.Text.Trim() != string.Empty)
            {
                radio.URL = URL.Text.Trim();
            }
            if (Icon.Source != null)
            {
                string filePath = ".\\Images\\" + Path.GetFileName(Icon.Source.ToString());
                radio.Icon = filePath;
                if (!File.Exists(filePath))
                {
                    Directory.CreateDirectory(".\\Images\\");
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)Icon.Source));
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        encoder.Save(stream);
                    }
                }
            }
            if(controller.Add(radio))
            {
                MessageBox.Show("Add succeeded", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                Name.Text = string.Empty;
                URL.Text = string.Empty;
                Icon.Source = null;
            }
            else
            {
                MessageBox.Show("Add failed","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }
    }
}
