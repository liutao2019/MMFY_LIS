using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Lib.EntityCore
{
    public class EntityXMLConverter
    {
        public static string EntityToXMLString<TEntity>(TEntity entity) where TEntity : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TEntity));

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

        public static TEntity XMLStringToEntity<TEntity>(string xml) where TEntity : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TEntity));

            StringReader stringReader = new StringReader(xml);

            XmlTextReader xmlReader = new XmlTextReader(stringReader);

            object obj = xmlSerializer.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();

            return obj as TEntity;
        }
    }
}
