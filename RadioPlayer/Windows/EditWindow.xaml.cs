﻿using Microsoft.Win32;
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
        Radio oldRadio;
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
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            controller.Delete();
            UpdateRadioList();
        }

        private void RadioStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldRadio = (Radio)RadioStation.SelectedItem;

            Name.Text = oldRadio.Name;
            URL.Text = oldRadio.URL;
            if (oldRadio.Icon != string.Empty && oldRadio.Icon != null)
            {
                Icon.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + oldRadio.Icon, UriKind.RelativeOrAbsolute));
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            controller.Update(oldRadio);
            UpdateRadioList();
        }
    }
}
