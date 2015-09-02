using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Tweddle.Commons.Extensions
{
    public static class XMLSerialization
    {

        public static String ToXML<T>(this T obj, Encoding enc)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings xmlWriterSettings = new System.Xml.XmlWriterSettings()
                {
                    // If set to true XmlWriter would close MemoryStream automatically and using would then do double dispose
                    // Code analysis does not understand that. That's why there is a suppress message.
                    CloseOutput = false,
                    Encoding = enc,
                    OmitXmlDeclaration = false,
                    Indent = true
                };
                using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(ms, xmlWriterSettings))
                {
                    //Create our own namespaces for the output
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                    //Add an empty namespace and empty value
                    ns.Add("", "");

                    XmlSerializer s = new XmlSerializer(typeof(T));
                    s.Serialize(xw, obj, ns);
                }

                return enc.GetString(ms.ToArray());
            }
        }

        public static T ToObject<T>(this string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(xml);

            return (T)serializer.Deserialize(sr);
        }
    }
}
