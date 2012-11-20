using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;

namespace ShippingService.Shared
{
    public static class CompressedSerializer
    {
        public enum Serializer
        {
            XML,
            Binary
        }
        public static T Decompress<T>(byte[] compressedData, Serializer serializer) where T : class
        {
            T result = null;
            using (MemoryStream memory = new MemoryStream())
            {
                memory.Write(compressedData, 0, compressedData.Length);
                memory.Position = 0L;

                result = Decompress<T>(memory, serializer);
            }

            return result;
        }

        public static T Decompress<T>(Stream stream, Serializer serializer) where T : class
        {
            T result = null;

            using (GZipStream zip = new GZipStream(stream, CompressionMode.Decompress, true))
            {
                zip.Flush();
                switch (serializer)
                {
                    case Serializer.Binary:
                        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        result = formatter.Deserialize(zip) as T;
                        break;
                    case Serializer.XML:
                        XmlSerializer ser = new XmlSerializer(typeof(T));
                        result = ser.Deserialize(zip) as T;
                        break;
                }

            }
            return result;
        }

        
        public static byte[] Compress<T>(T data, Serializer serializer)
        {
            byte[] result = null;
            using (MemoryStream memory = new MemoryStream())
            {
                Compress(data, serializer, memory);
                result = memory.ToArray();
            }

            return result;
        }

        public static void Compress<T>(T data, Serializer serializer, Stream stream)
        {
            using (GZipStream zip = new GZipStream(stream, CompressionMode.Compress, true))
            {
                switch (serializer)
                {
                    case Serializer.Binary:
                        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                        formatter.Serialize(zip, data);
                        break;
                    case Serializer.XML:
                        XmlSerializer ser = new XmlSerializer(data.GetType());
                        ser.Serialize(zip, data);
                        break;
                }
            }
        }
       
    } 

}
