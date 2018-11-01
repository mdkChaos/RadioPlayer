using Microsoft.Win32;
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
        //EditWindow window;
        string xmlPath = "\\Data\\RadioList.xml";
        public Radio Radio { get; set; }

        public CRUDController()
        {

        }

        public bool Add()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(Environment.CurrentDirectory + xmlPath);
            XmlElement xmlRoot = xmlDocument.DocumentElement;
            XmlElement radioElement = xmlDocument.CreateElement("Radio");
            XmlElement nameElement = xmlDocument.CreateElement("Name");
            XmlElement urlElement = xmlDocument.CreateElement("URL");
            XmlElement iconElement = xmlDocument.CreateElement("Icon");
            XmlText nameText = xmlDocument.CreateTextNode(Radio.Name);
            XmlText urlText = xmlDocument.CreateTextNode(Radio.URL);
            XmlText iconText = xmlDocument.CreateTextNode(Radio.Icon);

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
            if (Radio.Name != string.Empty)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Environment.CurrentDirectory + xmlPath);
                XmlElement xmlRoot = xmlDocument.DocumentElement;
                XmlNode xmlNode = xmlRoot.SelectSingleNode(string.Format($"Radio[Name='{Radio.Name}' and URL='{Radio.URL}']"));
                XmlNode xmlRemoveNode = xmlNode.ParentNode;
                xmlRemoveNode.RemoveChild(xmlNode);
                xmlDocument.Save(Environment.CurrentDirectory + xmlPath);

                MessageBox.Show("Delete succeeded", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                Radio = null;
            }
        }

        public void Update()
        {
            if (Radio.Name != string.Empty)
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Environment.CurrentDirectory + xmlPath);
                XmlElement xmlRoot = xmlDocument.DocumentElement;
                XmlNode xmlNode = xmlRoot.SelectSingleNode(string.Format($"Radio[Name='{Radio.Name}' and URL='{Radio.URL}']"));
                foreach (XmlNode xmlChildNode in xmlNode)
                {
                    if (xmlChildNode.Name == "Name" && xmlChildNode.InnerText != Radio.Name)
                    {
                        xmlChildNode.InnerText = Radio.Name;
                    }
                    if (xmlChildNode.Name == "URL" && xmlChildNode.InnerText != Radio.URL)
                    {
                        xmlChildNode.InnerText = Radio.URL;
                    }
                    if (xmlChildNode.Name == "Icon" && xmlChildNode.InnerText != Radio.Icon)
                    {
                        xmlChildNode.InnerText = Radio.Icon;
                    }
                }
                xmlDocument.Save(Environment.CurrentDirectory + xmlPath);

                MessageBox.Show("Update succeeded", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                Radio = null;
            }
        }

        public void Save()
        {
            if (Radio.Name != string.Empty)
            {
                if (!File.Exists(Environment.CurrentDirectory + Radio.Icon))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "\\Images\\");
                    PngBitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(new Uri(Radio.Icon, UriKind.RelativeOrAbsolute)));
                    using (FileStream stream = new FileStream(Environment.CurrentDirectory + Radio.Icon, FileMode.Create))
                    {
                        encoder.Save(stream);
                    }
                }
                if (Add())
                {
                    MessageBox.Show("Add succeeded", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    Radio = null;
                }
                else
                {
                    MessageBox.Show("Add failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public BitmapImage SetLogo()
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "Image files(*.png; *.jpg; *.bmp; *.bmp)|*.png; *.jpg; *.bmp; *.bmp|All files(*.*)|*.*"
            };
            if (openFile.ShowDialog() == true)
            {
                return new BitmapImage(new Uri(openFile.FileName, UriKind.RelativeOrAbsolute));
            }
            return null;
        }
    }
}
