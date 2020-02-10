using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Baguette_Workshop_New
{
    class XMLSerializator : ISerialization
    {
        DataContractSerializer xmlSerializer;
        public XMLSerializator(Type type)
        {
            xmlSerializer = new DataContractSerializer(type);
        }
        public void Serialization(Object obj, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                xmlSerializer.WriteObject(stream, obj);
            }
        }

        public Object Deserialization(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                return xmlSerializer.ReadObject(stream);
            }
        }

    }

    class JsonSerializator : ISerialization
    {
        DataContractJsonSerializer jsonSerializer;
        public JsonSerializator(Type type)
        {
            jsonSerializer = new DataContractJsonSerializer(type);
        }
        public void Serialization(Object obj, string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                jsonSerializer.WriteObject(stream, obj);
            }
        }
        public Object Deserialization(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate))
            {
                return jsonSerializer.ReadObject(stream);
            }
        }

    }

    /*class BinnarySerializator : ISerialization
    {
        BinaryFormatter binFormater = new BinaryFormatter();
        public void Serialization(Order order)
        {
            using (FileStream stream = new FileStream("order.dat", FileMode.OpenOrCreate))
            {
                binFormater.Serialize(stream, order);
            }
        }
        public Order Deserialization()
        {
            using (FileStream stream = new FileStream("order.dat", FileMode.OpenOrCreate))
            {
                Order newOrder = (Order)binFormater.Deserialize(stream);
                return newOrder;
            }
        }

    }*/

    class Serializator<T>
    {
        private ISerialization ser;
        public Serializator(ISerialization serial)
        {
            ser = serial;
        }
        public void Serialize(T shop, string path)
        {
            ser.Serialization(shop, path);
        }

        public T Deserialize(string path)
        {
            return (T)ser.Deserialization(path);
        }
    }
}
