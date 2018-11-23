using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    public class JsonObj<T>
    {
        public static string ToJson(T obj)
        {
            //Newtonsoft.Json.JsonConvert.DeserializeObject()
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, convert);
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
        public static object FromJson(string text, Type t)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Newtonsoft.Json.JsonConvert.DeserializeObject(text, t, convert);
        }
#if CSHARP2
        
#else
        public static dynamic FromJson2(string text)
        {
            //IsoDateTimeConverter convert = new IsoDateTimeConverter();
            //convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            return Newtonsoft.Json.JsonConvert.DeserializeObject(text);
        }
#endif
        public static string ToJson(object o, Type t)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            settings.Converters.Add(convert);
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, t, settings);
        }

        public static string ToJson(object o)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            settings.Converters.Add(convert);
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, settings);
        }

        public static string ToJsonWithNoneName(object o)
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None;
            settings.Converters.Add(convert);
            return Newtonsoft.Json.JsonConvert.SerializeObject(o, settings);
        }

        public static string ToJsonWithNone(object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
    public class JsonResultSetting
    {
        public JsonSerializerSettings Settings { get; private set; }
        public JsonResultSetting()
        {
            IsoDateTimeConverter convert = new IsoDateTimeConverter();
            convert.DateTimeFormat = "yyyy-MM-dd";
            JsonSerializerSettings settings = new JsonSerializerSettings();
            Settings = new JsonSerializerSettings
            {
                //这句是解决问题的关键,也就是json.net官方给出的解决配置选项.                 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

            };
            Settings.Converters.Add(convert);
        }

        public void Handle(System.IO.StringWriter sw, object Data)
        {
            var jrs = JsonSerializer.Create(this.Settings);
            jrs.Serialize(sw, Data);
        }

    }
}
