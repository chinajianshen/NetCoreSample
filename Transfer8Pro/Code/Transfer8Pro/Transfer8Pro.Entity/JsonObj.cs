using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Transfer8Pro.Entity
{
    public class JsonObj<T>
    {
        public static string ToJson(T obj)
        {
            //Newtonsoft.Json.JsonConvert.DeserializeObject()
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string json= Newtonsoft.Json.JsonConvert.SerializeObject(obj,convert);
            return json;
        }


        public static T FromJson(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                IsoDateTimeConverter convert = new IsoDateTimeConverter();
                convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(text, convert);
            }
            else
                return default(T);
           
        }

        public static object FromJson2(string text)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(convert);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(text, settings);
        }

    }

    public class JsonObj
    {
        public static object FromJson(string text,Type t)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Newtonsoft.Json.JsonConvert.DeserializeObject(text, t,convert);
        }

        public static string ToJson(object o, Type t)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            settings.Converters.Add(convert);
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, t, settings);
        }
    }
}
