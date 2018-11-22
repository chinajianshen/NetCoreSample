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
        private static ConcurrentDictionary<DateType, T8ConfigItemEntity> _T8ItemDic;

        public static T8ConfigEntity T8ConfigEntity { get; private set; }       

        static T8ConfigHelper()
        {
            _T8ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "T8.config");
            T8ConfigEntity = SerializableHelper<T8ConfigEntity>.BinaryDeserialize(_T8ConfigFilePath);
            if (T8ConfigEntity != null)
            {
                _T8ItemDic = T8ConfigEntity.T8ConfigItemDic;
            }
        }

        /// <summary>
        /// 添加配置项
        /// </summary>
        /// <param name="item"></param>
        public static bool AddItem(DateType dateType, T8ConfigItemEntity item)
        {
            if ( _T8ItemDic.TryAdd(dateType, item))
            {
                T8ConfigEntity.T8ConfigItemDic = _T8ItemDic;
                if (SerializableHelper<T8ConfigEntity>.BinarySerializeFile(_T8ConfigFilePath, T8ConfigEntity))
                {
                    return true;
                }
                else
                {
                    T8ConfigItemEntity newitem;
                    _T8ItemDic.TryRemove(dateType, out newitem);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 移除项
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool Remove(DateType dateType)
        {
            T8ConfigItemEntity item;
            if (_T8ItemDic.TryRemove(dateType,out item))
            {
                T8ConfigEntity.T8ConfigItemDic = _T8ItemDic;
                if (SerializableHelper<T8ConfigEntity>.BinarySerializeFile(_T8ConfigFilePath, T8ConfigEntity))
                {
                    return true;
                }
                else
                {                   
                    _T8ItemDic.TryAdd(dateType, item);
                    return false;
                }
            }
            return false;
        }

    }
}
