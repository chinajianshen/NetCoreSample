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
        private static ConcurrentDictionary<string, T8ConfigItemEntity> _T8ConfigItemDic;

        /// <summary>
        /// T8配置实体
        /// </summary>
        public static T8ConfigEntity T8Config { get; private set; }

        /// <summary>
        /// T8配置项字典表
        /// </summary>
        public static ConcurrentDictionary<string, T8ConfigItemEntity> T8ConfigItemDic
        {
            get
            {
                if (_T8ConfigItemDic == null || _T8ConfigItemDic.Keys.Count == 0)
                {
                    InitT8ConfigItems();
                }
                return _T8ConfigItemDic;
            }
         }


        private static object lockObj = new object();

        static T8ConfigHelper()
        {
            InitT8Config();
        }

        private static void InitT8Config()
        {
            _T8ConfigFilePath = Path.Combine(Directory.GetCurrentDirectory(), "T8.config");
            T8Config = SerializableHelper<T8ConfigEntity>.BinaryDeserialize(_T8ConfigFilePath);
            if (T8Config != null)
            {
                _T8ItemContainerDic = T8Config.T8ItemContainerDic;
                InitT8ConfigItems();
            }
            else
            {
                T8Config = new T8ConfigEntity();
                _T8ConfigItemDic = new ConcurrentDictionary<string, T8ConfigItemEntity>();
            }
        }

        /// <summary>
        /// 初始化T8配置项
        /// </summary>
        private static void InitT8ConfigItems()
        {
            if (_T8ConfigItemDic == null || _T8ConfigItemDic.Keys.Count == 0)
            {
                lock (lockObj)
                {
                    if (_T8ConfigItemDic == null || _T8ConfigItemDic.Keys.Count == 0)
                    {
                        if (T8Config != null && T8Config.T8ItemContainerDic.Keys.Count > 0)
                        {
                            _T8ConfigItemDic = new ConcurrentDictionary<string, T8ConfigItemEntity>();
                            foreach (var item in T8Config.T8ItemContainerDic.Values)
                            {
                                var configItem = item.T8ConfigItemSale;
                                _T8ConfigItemDic.TryAdd(configItem.ConfigItemKey,configItem);
                                var configItemOnShelf = item.T8ConfigITemOnShelf;
                                _T8ConfigItemDic.TryAdd(configItemOnShelf.ConfigItemKey, configItemOnShelf);
                            }
                        }
                        else
                        {
                            InitT8Config();
                        }
                    }
                }
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
                    t8ConfigItemContainer.T8ConfigITemOnShelf = item;
                }

                if (_T8ItemContainerDic.TryAdd(dateType, t8ConfigItemContainer))
                {
                    T8Config.T8ItemContainerDic = _T8ItemContainerDic;
                    if (SerializableHelper<T8ConfigEntity>.BinarySerializeFile(_T8ConfigFilePath, T8Config))
                    {
                        _T8ConfigItemDic = new ConcurrentDictionary<string, T8ConfigItemEntity>();
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
                    _T8ConfigItemDic = new ConcurrentDictionary<string, T8ConfigItemEntity>() ;
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
                _T8ConfigItemDic = new ConcurrentDictionary<string, T8ConfigItemEntity>();
                return true;
            }
            return false;
        }

       
        public static T8ConfigEntity CloneT8Config()
        {
            if (T8Config != null)
            {
                lock (lockObj)
                {
                    if (T8Config != null)
                    {
                        return DeepCopyUtil.DeepCopyByBin<T8ConfigEntity>(T8Config);
                    }                   
                }               
            }
            return null;
        }

        public static ConcurrentDictionary<string, T8ConfigItemEntity> CloneT8ConfigItem() {
            if (T8ConfigItemDic != null)
            {
                lock (lockObj)
                {
                    if (T8ConfigItemDic != null)
                    {
                        return DeepCopyUtil.DeepCopyByBin<ConcurrentDictionary<string, T8ConfigItemEntity>>(T8ConfigItemDic);
                    }
                }
            }
            return null;
        }
    }
}
