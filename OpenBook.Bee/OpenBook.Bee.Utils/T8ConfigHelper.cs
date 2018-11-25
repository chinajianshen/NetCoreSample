using OpenBook.Bee.Entity;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Utils
{
    /// <summary>
    /// T8配置帮助类
    /// </summary>
   public class T8ConfigHelper
    {
        private static string _T8ConfigFilePath;
        private static ConcurrentDictionary<DateType, T8ConfigItemContainer> _T8ItemContainerDic;

        public static T8ConfigEntity T8Config { get; private set; }       

        static T8ConfigHelper()
        {
            _T8ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "T8.config");
            T8Config = SerializableHelper<T8ConfigEntity>.BinaryDeserialize(_T8ConfigFilePath);
            if (T8Config != null)
            {
                _T8ItemContainerDic = T8Config.T8ItemContainerDic;
            }
            else
            {
                T8Config = new T8ConfigEntity();
            }
        }

        /// <summary>
        /// 添加配置项
        /// </summary>
        /// <param name="dateType"></param>
        /// <param name="dataType"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool AddItem(DateType dateType,DataType dataType, T8ConfigItemEntity item)
        {         
            if (item == null)
            {
                return false;
            }
            
            T8ConfigItemContainer t8ConfigItemContainer;
            if (T8Config.T8ItemContainerDic.TryGetValue(dateType,out t8ConfigItemContainer))
            {
                if (dataType == DataType.SaleData)
                {                   
                    t8ConfigItemContainer.T8ConfigItemSale = item;
                }
                else
                {
                    t8ConfigItemContainer.T8ConfigITemOnSale = item;
                }

                if (_T8ItemContainerDic.TryAdd(dateType, t8ConfigItemContainer))
                {
                    T8Config.T8ItemContainerDic = _T8ItemContainerDic;
                    if (SerializableHelper<T8ConfigEntity>.BinarySerializeFile(_T8ConfigFilePath, T8Config))
                    {
                        return true;
                    }
                    else
                    {
                        //T8ConfigItemContainer newitemContainer;
                        //_T8ItemContainerDic.TryRemove(dateType, out newitem);
                        return false;
                    }
                }
            }
           
            return false;
        }

        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool Remove(DateType dateType,DataType dataType)
        {
            T8ConfigItemContainer item;
            if (_T8ItemContainerDic.TryRemove(dateType,out item))
            {
                T8Config.T8ItemContainerDic = _T8ItemContainerDic;
                if (SerializableHelper<T8ConfigEntity>.BinarySerializeFile(_T8ConfigFilePath, T8Config))
                {
                    return true;
                }
                else
                {
                    _T8ItemContainerDic.TryAdd(dateType, item);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加Ftp信息
        /// </summary>
        /// <param name="ftpInfo"></param>
        /// <returns></returns>
        public static bool AddFtpInfo(FtpInfoEntity ftpInfo)
        {
            T8Config.FtpInfo = ftpInfo;
            if (SerializableHelper<T8ConfigEntity>.BinarySerializeFile(_T8ConfigFilePath, T8Config))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加数据库信息
        /// </summary>
        /// <param name="dataBaseInfo"></param>
        /// <returns></returns>
        public static bool AddDataBaseInfo(DataBaseInfoEntity dataBaseInfo)
        {
            T8Config.DataBaseInfo = dataBaseInfo;
            if (SerializableHelper<T8ConfigEntity>.BinarySerializeFile(_T8ConfigFilePath, T8Config))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加T8配置
        /// </summary>
        /// <param name="t8ConfigEntity"></param>
        /// <returns></returns>
        public static bool AddT8Config(T8ConfigEntity t8ConfigEntity)
        {
            T8Config = t8ConfigEntity;
            if (SerializableHelper<T8ConfigEntity>.BinarySerializeFile(_T8ConfigFilePath, T8Config))
            {
                return true;
            }
            return false;
        }

    }
}
