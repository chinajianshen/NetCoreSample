using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DapperInCoreLib
{
    public enum ConnectionType
    {
        SqlServer = 0,
        Oracle = 1,
    }

    public static class EmenExtension
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        public static string GetDescription(this object obj)
        {
            return GetDescription(obj, false);
        }

        private static string GetDescription(this object obj, bool isTop)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            try
            {
                Type enumType = obj.GetType();
                DescriptionAttribute dna = null;
                if (isTop)
                {
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(enumType, typeof(DescriptionAttribute));
                }
                else
                {
                    FieldInfo fieldInfo = enumType.GetField(System.Enum.GetName(enumType, obj));
                    dna = (DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));
                }
                if (dna != null && dna.Description != null)
                {
                    return dna.Description;
                }
            }
            catch
            { }

            return obj.ToString();
        }

        /// <summary>
        /// 取枚举
        /// </summary>
        public static T ConvertFromString<T>(this string strValue) where T : struct
        {
            T t = default(T);
            if (typeof(Enum) != typeof(T).BaseType)
            {
                return t;
            }
            return (T)Enum.Parse(typeof(T), strValue);
        }

        public static T ConvertFromObj<T>(this int intVal) where T : struct
        {
            var t = default(T);
            if (typeof(Enum) != typeof(T).BaseType)
            {
                return t;
            }
            return (T)Enum.ToObject(typeof(T), intVal);
        }

        /// <summary>
        /// 通过key获取枚举
        /// </summary>
        public static T GetEnumByEnumKey<T>(this object enumKey) where T : struct
        {
            var tResult = default(T);
            if (typeof(Enum) == typeof(T).BaseType)
            {
                var tArray = (T[])Enum.GetValues(typeof(T));
                foreach (var tItem in tArray)
                {
                    if (tItem.ToString() == enumKey.ToString())
                    {
                        return tItem;
                    }
                }
            }

            return tResult;
        }

        /// <summary>
        /// 获取枚举的List(key value desc)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumT<T>> GetEnumTList<T>() where T : struct
        {
            var tEnumList = new List<EnumT<T>>();
            T enumT = default(T);
            var enumNameList = Enum.GetNames(typeof(T));
            enumNameList.ToList().ForEach(x =>
            {
                if (!tEnumList.Exists(o => o.Key == x))
                {
                    enumT = (T)Enum.Parse(typeof(T), x);
                    tEnumList.Add(new EnumT<T>()
                    {
                        Key = x,
                        Value = Convert.ToInt32(enumT),
                        Text = GetDescription(enumT)
                    });
                }
            });

            return tEnumList;
        }
    }

    /// <summary>
    /// 枚举类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumT<T> where T : struct
    {
        /// <summary>
        /// Key
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; set; }
    }
}