using RadioPlayer.Models;
using RadioPlayer.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Xml;

namespace RadioPlayer.Controllers
{
    public class CRUDController
    {
        EditWindow window;
        public CRUDController()
        {

        }
        public CRUDController(EditWindow window)
        {
            this.window = window;
        }
        public bool Add(Radio radio)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(".\\Data\\RadioList.xml");
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            XmlElement radioElement = xmlDocument.CreateElement("Radio");
            XmlElement nameElement = xmlDocument.CreateElement("Name");
            XmlElement urlElement = xmlDocument.CreateElement("URL");
            XmlElement iconElement = xmlDocument.CreateElement("Icon");
            XmlText nameText = xmlDocument.CreateTextNode(radio.Name);
            XmlText urlText = xmlDocument.CreateTextNode(radio.URL);
            XmlText iconText = xmlDocument.CreateTextNode(radio.Icon);

            nameElement.AppendChild(nameText);
            urlElement.AppendChild(urlText);
            iconElement.AppendChild(iconText);
            radioElement.AppendChild(nameElement);
            radioElement.AppendChild(urlElement);
            radioElement.AppendChild(iconElement);
            xmlRoot.AppendChild(radioElement);
            xmlDocument.Save(".\\Data\\RadioList.xml");

            return true;
        }
        public void Delete()
        {

        }
        public void Update()
        {
            
        }
        public void Save()
        {
            Radio radio = new Radio();
            if (window.Name.Text.Trim() != string.Empty)
            {
                radio.Name = window.Name.Text.Trim();
            }
            if (window.URL.Text.Trim() != string.Empty)
            {
                radio.URL = window.URL.Text.Trim();
            }
            if (window.Icon.Source != null)
            {
                string filePath = "\\Images\\" + Path.GetFileName(window.Icon.Source.ToString());
                radio.Icon = filePath;
                if (!File.Exists(Environment.CurrentDirectory + filePath))
                {
                    Directory.CreateDirectory(".\\Images\\");
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)window.Icon.Source));
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        encoder.Save(stream);
                    }
                }
            }
            if (Add(radio))
            {
                MessageBox.Show("Add succeeded", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                window.Name.Text = string.Empty;
                window.URL.Text = string.Empty;
                window.Icon.Source = null;
            }
            else
            {
                MessageBox.Show("Add failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
