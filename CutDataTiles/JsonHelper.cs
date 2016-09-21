using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace CutDataTiles
{
   /// <summary>
    /// 在VS中需要添加引用System.Web.Script.Serialization的时候，请先引用System.Web.Extensions
    /// </summary>
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
}

