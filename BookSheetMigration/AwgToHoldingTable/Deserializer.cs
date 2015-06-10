using System.Xml.Linq;
using System.Xml.Serialization;

namespace BookSheetMigration
{
    public class Deserializer<T> where T : new()
    {

        private XElement response;

        public Deserializer(XElement response)
        {
            this.response = response;
        }

        public T deserializeResponse()
        {
            if (response == null)
            {
                return new T();
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                var reader = response.CreateReader();
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
