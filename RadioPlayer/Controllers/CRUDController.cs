using RadioPlayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RadioPlayer.Controllers
{
    public class CRUDController
    {
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
    }
}
