using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Grind
{
    class ToDoListManager
    {
        public ObservableCollection<string> toDoList = new ObservableCollection<string>();
        public ObservableCollection<string> doneList = new ObservableCollection<string>();

        public ToDoListManager()
        {

        }

        public void SaveLists() {
            //save the lists
        }

        //https://code.msdn.microsoft.com/windowsapps/CSWinStoreAppSaveCollection-bed5d6e6
        public string ToDoToXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true,
            };

            
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                serializer.Serialize(xmlWriter, toDoList.ToList());
            }
            return stringBuilder.ToString();
        }

        public void ToDoFromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            List<string> value;
            using (StringReader stringReader = new StringReader(xml))
            {
                object deserialized = serializer.Deserialize(stringReader);
                value = (List<string>)deserialized;
            }

            toDoList = new ObservableCollection<string>(value);
        } 

        public string DoneToXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true,
            };


            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                serializer.Serialize(xmlWriter, doneList.ToList());
            }
            return stringBuilder.ToString();
        }

        public void DoneFromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
            List<string> value;
            using (StringReader stringReader = new StringReader(xml))
            {
                object deserialized = serializer.Deserialize(stringReader);
                value = (List<string>)deserialized;
            }

            doneList = new ObservableCollection<string>(value);
        } 
    }
}
