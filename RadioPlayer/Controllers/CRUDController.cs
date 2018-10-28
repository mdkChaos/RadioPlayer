using RadioPlayer.Models;
using RadioPlayer.Windows;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml;

namespace RadioPlayer.Controllers
{
    public class CRUDController
    {
        EditWindow window;
        string xmlPath = "\\Data\\RadioList.xml";
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
            xmlDocument.Load(Environment.CurrentDirectory + xmlPath);
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
            xmlDocument.Save(Environment.CurrentDirectory + xmlPath);

            return true;
        }
        public void Delete()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Environment.CurrentDirectory + xmlPath);
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            XmlNode xmlNode = xmlRoot.SelectSingleNode(string.Format($"Radio[Name='{window.Name.Text.Trim()}' and URL='{window.URL.Text.Trim()}']"));
            XmlNode xmlRemoveNode = xmlNode.ParentNode;
            xmlRemoveNode.RemoveChild(xmlNode);
            xmlDocument.Save(Environment.CurrentDirectory + xmlPath);

            MessageBox.Show("Delete succeeded", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            window.Name.Text = string.Empty;
            window.URL.Text = string.Empty;
            window.Icon.Source = null;
        }
        public void Update(Radio oldRadio)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Environment.CurrentDirectory + xmlPath);
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            XmlNode xmlNode = xmlRoot.SelectSingleNode(string.Format($"Radio[Name='{oldRadio.Name}' and URL='{oldRadio.URL}']"));
            foreach (XmlNode xmlChildNode in xmlNode)
            {
                if (xmlChildNode.Name == "Name" && xmlChildNode.InnerText != window.Name.Text.ToString())
                {
                    xmlChildNode.InnerText = window.Name.Text.ToString();
                }
                if (xmlChildNode.Name == "URL" && xmlChildNode.InnerText != window.URL.Text.ToString())
                {
                    xmlChildNode.InnerText = window.URL.Text.ToString();
                }
                if (xmlChildNode.Name == "Icon" && xmlChildNode.InnerText != "\\Images\\" + Path.GetFileName(window.Icon.Source.ToString()))
                {
                    xmlChildNode.InnerText = "\\Images\\" + Path.GetFileName(window.Icon.Source.ToString());
                }
            }
            xmlDocument.Save(Environment.CurrentDirectory + xmlPath);

            MessageBox.Show("Update succeeded", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            window.Name.Text = string.Empty;
            window.URL.Text = string.Empty;
            window.Icon.Source = null;
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
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\Images\\");
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(new Uri(window.Icon.Source.ToString(), UriKind.RelativeOrAbsolute)));
                    using (FileStream stream = new FileStream(Environment.CurrentDirectory + filePath, FileMode.Create))
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
