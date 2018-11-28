using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Utils
{
    public class SerializableHelper<T>  where T:class,new()
    {
        private static BinaryFormatter binFormat;
        private static object lockObj = new object();
        static SerializableHelper()
        {
            binFormat = new BinaryFormatter();
        }

        /// <summary>
        /// 二进制序列化文件
        /// </summary>
        /// <param name="fileName">全路径文件</param>
        /// <param name="item">对象</param>
        /// <returns></returns>
        public static bool BinarySerializeFile(string fileName,T item)
        {
            try
            {
                lock (lockObj)
                {
                    using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        binFormat.Serialize(fStream, item);
                        fStream.Close();
                    }
                }               
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="fileName">全路径文件</param>
        /// <returns></returns>
        public static T BinaryDeserialize(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            T item;

            try
            {
                lock (lockObj)
                {
                    using (FileStream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read,FileShare.Read))
                    {
                        item = binFormat.Deserialize(fStream) as T;
                        fStream.Close();
                    }
                }
                return item;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }          
        }

        /// <summary>
        /// 二进制序列化文件
        /// </summary>
        /// <param name="fileName">全路径文件</param>
        /// <param name="item">对象</param>
        /// <returns></returns>
        public static bool BinarySerializeFile(string fileName, List<T> item)
        {
            try
            {
                lock (lockObj)
                {
                    using (Stream fStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                    {
                        binFormat.Serialize(fStream, item);
                        fStream.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return false;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="fileName">全路径文件</param>
        /// <returns></returns>
        public static List<T> BinaryDeserializeList(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            List<T> items;

            try
            {
                lock (lockObj)
                {
                    using (FileStream fStream = new FileStream(fileName, FileMode.Open, FileAccess.Read,FileShare.Read))
                    {
                        items = binFormat.Deserialize(fStream) as List<T>;
                        fStream.Close();
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return null;
            }
        }

    }
}
