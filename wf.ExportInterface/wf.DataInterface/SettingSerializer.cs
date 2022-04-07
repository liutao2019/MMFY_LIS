using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Lib.DataInterface.DataModel
{
    class SettingSerializer
    {
        public static string EntityToXMLString(DataInterfaceCommand entity)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataInterfaceCommand));

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.OmitXmlDeclaration = true;

            StringBuilder stringBuilder = new StringBuilder();

            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
            {
                xmlSerializer.Serialize(xmlWriter, entity, xmlSerializerNamespaces);
            }

            return stringBuilder.ToString();
        }

        public static DataInterfaceCommand XMLStringToEntity(string xmlString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataInterfaceCommand));

            StringReader stringReader = new StringReader(xmlString);

            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = xmlSerializer.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();

            return obj as DataInterfaceCommand;
        }
    }
}
