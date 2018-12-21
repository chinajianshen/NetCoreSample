using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Transfer8Pro.Core.Infrastructure;
using Transfer8Pro.Core.Service;
using Transfer8Pro.DAO;
using Transfer8Pro.DAO.DataHandlers;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.Core
{
    public class Common
    {
        /// <summary>
        /// 解密数据
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string DecryptData(string decryptString)
        {
            if (string.IsNullOrEmpty(decryptString))
            {
                throw new ArgumentNullException("DecryptDBConnString()方法，参数decryptString值为空");
            }         

            string key = GetEncryptKey();

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("配置中未读取到客户密钥串");
            }

            return RijndaelCrypt.Decrypt(decryptString, key);
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string EncryptData(string encryptString)
        {
            if (string.IsNullOrEmpty(encryptString))
            {
                throw new ArgumentNullException("EncryptDBConnString()方法，参数encryptString值为空");
            }
            
            string key = GetEncryptKey();

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("配置中未读取到客户密钥串");
            }
            return RijndaelCrypt.Encrypt(encryptString, key);
        }

        /// <summary>
        /// 获取用户密钥
        /// </summary>
        /// <returns></returns>
        public static string GetEncryptKey()
        {
             SystemConfigEntity systemConfig = new SystemConfigService().FindSystemConfig((int)SystemConfigs.EncryptKey);
            if (systemConfig != null)
            {
                return systemConfig.ExSetting01;
            }
            else
            {
                throw new Exception("未设置开卷分配的密钥");
            }
        }

        /// <summary>
        /// 获取当前可执行产品构造器
        /// </summary>
        /// <returns></returns>
        public static ADbFileProductBuilder GetDbFileProductBuilder()
        {
            SystemConfigService service = new SystemConfigService();

            SystemConfigEntity systemConfig = service.FindSystemConfig((int)SystemConfigs.OpenbookSysType);
            if (systemConfig.Status == 0)
            {
                 throw new Exception($"系统未配置开卷系统类型");               
            }

            ADbFileProductBuilder productBuilder = null;
            if (systemConfig.Status == 1)
            {
                productBuilder = new DbFileProductBuilder();
            }
            else
            {
                productBuilder = new T8DbFileProductBuilder();
            }

            return productBuilder;
        }

       
        /// <summary>
        /// 检测SQLite数据库是否存在
        /// </summary>
        /// <returns></returns>
        public static bool IsExistSQLiteDB()
        {
            string localDbPath = "";
            if (!RegexExpressionUtil.ValidateFilePathReg.IsMatch(ConfigHelper.GetConfig("SQLitePath")))
            {
                localDbPath = Path.Combine(AppPath.App_Root, ConfigHelper.GetConfig("SQLitePath"));
            }
            else
            {
                localDbPath = ConfigHelper.GetConfig("SQLitePath");
            }

            if (!File.Exists(localDbPath))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 生成小时列表
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> DigitalHours()
        {
            List<KVEntity> list = new List<KVEntity>();
            list.Add(new KVEntity { K = "", V = "-999" });

            for (int i = 0; i < 24; i++)
            {
                list.Add(new KVEntity { K = i.ToString(), V = i.ToString() });
            }
            return list;
        }

        /// <summary>
        /// 生成分钟列表
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> DigitalMinutes()
        {
            List<KVEntity> list = new List<KVEntity>();
            list.Add(new KVEntity { K = "", V = "-999" });
            for (int i = 1; i < 60; i++)
            {
                list.Add(new KVEntity { K = i.ToString(), V = i.ToString() });
            }
            return list;
        }

        /// <summary>
        /// 执行多次 生成分钟列表
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> DigitalMultiTimesMinutes()
        {
            List<KVEntity> list = new List<KVEntity>();
            list.Add(new KVEntity { K = "", V = "-999" });
            list.Add(new KVEntity { K = "1分", V = "0/1" });
            list.Add(new KVEntity { K = "5分", V = "0/5" });
            list.Add(new KVEntity { K = "10分", V = "0/10" });
            list.Add(new KVEntity { K = "20分", V = "0/20" });
            list.Add(new KVEntity { K = "30分", V = "0/30" });
            list.Add(new KVEntity { K = "1小时", V = "0/1" });
            list.Add(new KVEntity { K = "2小时", V = "0/2" });
            list.Add(new KVEntity { K = "3小时", V = "0/3" });
            list.Add(new KVEntity { K = "6小时", V = "0/6" });
            list.Add(new KVEntity { K = "8小时", V = "0/8" });
            list.Add(new KVEntity { K = "12小时", V = "0/12" });
            return list;
        }

        /// <summary>
        /// 生成周列表
        /// 1周日 2周一 .... 7周六
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> DigitalWeeks()
        {
            List<KVEntity> list = new List<KVEntity>();         
            list.Add(new KVEntity { K = "周一", V = "2" });
            list.Add(new KVEntity { K = "周二", V = "3" });
            list.Add(new KVEntity { K = "周三", V = "4" });
            list.Add(new KVEntity { K = "周四", V = "5" });
            list.Add(new KVEntity { K = "周五", V = "6" });
            list.Add(new KVEntity { K = "周六", V = "7" });
            list.Add(new KVEntity { K = "周日", V = "1" });

            return list;
        }
     
        /// <summary>
        /// 生成月列表
        /// 1-28号 及最后一天
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> DigitalMonths()
        {
            List<KVEntity> list = new List<KVEntity>();          
            for (int i = 1; i <= 31; i++)
            {
                list.Add(new KVEntity { K = $"{i}号", V = i.ToString() });
                //if (i <= 28)
                //{
                //    list.Add(new KVEntity { K = $"{i}号", V = i.ToString() });
                //}
                //else
                //{
                //    list.Add(new KVEntity { K = "最后一天", V = "L" });
                //}

            }
            return list;
        }

        /// <summary>
        /// 获取所有数据类型
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> DataTypeList()
        {
            List<KVEntity> list = new List<KVEntity>();
            list.Add(new KVEntity { K = "", V = "-999" });
            list.Add(new KVEntity { K = "销售数据", V = ((int)DataTypes.Sale).ToString() });
            list.Add(new KVEntity { K = "库存数据", V = ((int)DataTypes.Stock).ToString() });
            list.Add(new KVEntity { K = "书目数据", V = ((int)DataTypes.Book).ToString() });
            list.Add(new KVEntity { K = "门店数据", V = ((int)DataTypes.Store).ToString() });
            list.Add(new KVEntity { K = "流水数据", V = ((int)DataTypes.Flow).ToString() });
            list.Add(new KVEntity { K = "会员数据", V = ((int)DataTypes.Member).ToString() });
            return list;
        }

        /// <summary>
        /// 获取所有数据库类型
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity<int>> DbTypeList()
        {
            List<KVEntity<int>> list = new List<KVEntity<int>>();
            list.Add(new KVEntity<int> { K = "", V = "-999" });
            List<KVEntity> dbTypelist = DataHandlerFactory.DBTypes;
            if (dbTypelist.Count > 0)
            {
                foreach (KVEntity item in dbTypelist)
                {
                    if (item.K.Contains("SqlServer_DataHandler"))
                    {
                        list.Add(new KVEntity<int> { K= item.V,V =item.K, T1=(int)DbTypes.Sqlserver });
                    }
                    else if (item.K.Contains("Oracle_DataHandler"))
                    {
                        list.Add(new KVEntity<int> { K = item.V, V = item.K, T1 = (int)DbTypes.Oracle });
                    }
                    else if (item.K.Contains("MySql_DataHandler"))
                    {
                        list.Add(new KVEntity<int> { K = item.V, V = item.K, T1 = (int)DbTypes.MySql });
                    }
                    else if (item.K.Contains("Common_DataHandler"))
                    {
                        list.Add(new KVEntity<int> { K = item.V, V = item.K, T1 = (int)DbTypes.Oledb });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取数据库预留
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static KVEntity DbReservedString(DbTypes dbType)
        {
            KVEntity entity = new KVEntity(); ;
            switch (dbType)
            {
                case DbTypes.Sqlserver:
                    entity.K = "server=192.168.0.14;database=T8DataTest;uid=sa;pwd=sa.;min pool size=10;max pool size=300;Connection Timeout=10;";
                    entity.V = @"SELECT isbn,Title as  booktitle,PublishName as publisher_name,price, SalesCount as amount,Author as author_editor,SalesDateTime as selldate,StoreID as werks
FROM T8_BookInfo
WHERE SalesDateTime>= @StartTime and SalesDateTime<= @EndTime";
                    break;
                case DbTypes.Oracle:
                    entity.K = "User ID = ox; Password = ox_pwd; Data Source = (DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 192.168.1.60)(PORT = 18991)))(CONNECT_DATA = (SERVICE_NAME = ORCL12)))";
                    entity.V = @"SELECT isbn,Title booktitle,price,PublishName publisher_name,WriteTime selldate 
FROM ob_bookinfo_m 
WHERE publish1id>0 and WriteTime>= :StartTime and WriteTime <= :EndTime";
                    break;
                case DbTypes.MySql:
                    break;
                case DbTypes.Oledb:
                    break;
                default:
                    entity = null;
                    break;
            }
            return entity;
        }

        /// <summary>
        /// 连接数据库测试
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="dataHandlerTypeName"></param>
        /// <returns></returns>
        public static bool DBConnectOpen(string connStr,string dataHandlerTypeName)
        {
            IDataHandler dataHandler = DataHandlerFactory.GetHandler(dataHandlerTypeName);
            dataHandler.DBConnectStr = connStr;
            return dataHandler.DBConnectOpen();
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="dataHandlerTypeName"></param>
        /// <param name="sql"></param>
        /// <param name="op"></param>
        /// <param name="ed"></param>
        /// <returns></returns>
        public static DataTable GetDataList( string connStr,string dataHandlerTypeName, string sql, DateTime op,DateTime ed)
        {
            IDataHandler dataHandler = DataHandlerFactory.GetHandler(dataHandlerTypeName);
            dataHandler.DBConnectStr = connStr;
            return dataHandler.GetDataList(sql, op, ed);
        }

        /// <summary>
        /// 获取数据类型列表
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> GetDataTypeList()
        {
            List<KVEntity> list = new List<KVEntity>();
            list.Add(new KVEntity { K = "", V = "-999" });
            foreach (DataTypes dataType in Enum.GetValues(typeof(DataTypes)))
            {
                list.Add(new KVEntity { K = dataType.ToString(),V = ((int)dataType).ToString() });
            }
            return list;
        }

        /// <summary>
        /// 获取时间类型列表
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> GetCycleTypeList()
        {
            List<KVEntity> list = new List<KVEntity>();
            list.Add(new KVEntity { K = "", V = "-999" });
            foreach (CycleTypes dataType in Enum.GetValues(typeof(CycleTypes)))
            {
                list.Add(new KVEntity { K = dataType.ToString(), V = ((int)dataType).ToString() });
            }
            return list;
        }

        /// <summary>
        /// 获取任务状态列表
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> GetTaskStatusList()
        {
            List<KVEntity> list = new List<KVEntity>();
            list.Add(new KVEntity { K = "", V = "-999" });
            foreach (TaskStatus dataType in Enum.GetValues(typeof(TaskStatus)))
            {
                list.Add(new KVEntity { K = dataType.ToString(), V = ((int)dataType).ToString() });
            }
            return list;
        }

        /// <summary>
        /// 获取任务启用状态
        /// </summary>
        /// <returns></returns>
        public static List<KVEntity> GetTaskEnabledStatusList()
        {
            List<KVEntity> list = new List<KVEntity>();
            list.Add(new KVEntity { K = "", V = "-999" });
            list.Add(new KVEntity { K = "启用", V = "1" });
            list.Add(new KVEntity { K = "停用", V = "2" });
            return list;
        }
    }
}
