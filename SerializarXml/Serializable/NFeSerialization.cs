using System;
using System.Xml.Serialization;

namespace SerializarXml.Serializable
{
    public class NFeSerialization
    {
        public T GetObjectFromFile<T>(string arquivo) where T : class
        {
            var serialize = new XmlSerializer(typeof(T));

            try
            {
                var xmlArquivo = System.Xml.XmlReader.Create(arquivo);
                return (T)serialize.Deserialize(xmlArquivo);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}