using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MapDataTools
{
    using System.Xml.Serialization;

    public class JsonHelper
    {
        /// <summary>
        /// JSON序列化
        /// </summary>
        internal static string Serializer(object t)
        {
            // return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(t);
            var type = t.GetType();
            var ser = new DataContractJsonSerializer(type);
            string result;
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                result = Encoding.UTF8.GetString(ms.ToArray());
            }
            return result;
        }

        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            //return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(t);

            var ser = new DataContractJsonSerializer(typeof(T));
            string result;
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                result = Encoding.UTF8.GetString(ms.ToArray());
            }
            return result;
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {

            return new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<T>(jsonString);
            var ser = new DataContractJsonSerializer(typeof(T));
            var result = default(T);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                result = (T)ser.ReadObject(ms);
            }
            return result;
        }

        public static string ToJson(object obj)
        {
            var seriali = new System.Web.Script.Serialization.JavaScriptSerializer();
            seriali.MaxJsonLength = int.MaxValue;
            return seriali.Serialize(obj);
        }


        internal static T Cast<T>(string json)
        {
            T obj = default(T);
            if (string.IsNullOrEmpty(json))
            {
                return obj;
            }
            string[] arr = json.Split(',');
            if (arr == null || arr.Length <= 0)
            {
                return obj;
            }
            return obj;
        }
    }

    public class XmlHelper
    {
        public static string ToXml<T>(T entity)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            using (StringWriter sw = new StringWriter())
            {
                new System.Xml.Serialization.XmlSerializer(typeof(T)).Serialize(sw, entity, ns);
                return sw.ToString();
            }
        }

        public static T ToEntity<T>(string xml)
        {
            XmlSerializer xmlSearializer = new XmlSerializer(typeof(T));
            T info = (T)xmlSearializer.Deserialize(new StreamReader(new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(xml))));
            return info;
        }
    }
}
