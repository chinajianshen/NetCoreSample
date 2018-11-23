using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;

namespace OpenBook.Bee.Test
{
    public class T8ConfigSetting
    {

        public void CreateConfig(bool isNewCreate)
        {
            if (isNewCreate)
            {
                T8ConfigEntity t8ConfigEntity = T8ConfigHelper.T8Config;


                t8ConfigEntity.T8ConfigItemDic = new System.Collections.Concurrent.ConcurrentDictionary<DateType, T8ConfigItemEntity>();

                t8ConfigEntity.FtpInfo = new FtpInfoEntity
                {
                    ServerAddress = "ftp://openbook.cn",
                    UserName = "t8jyqssd",
                    UserPassword = "t8jyqssd"
                };
                t8ConfigEntity.DataBaseInfo = new DataBaseInfoEntity
                {
                    DataBaseTitle = "Sql Server数据库",
                    ServerName = "192.168.0.14",
                    DataBaseName = "T8DataTest",
                    UserName = "sa",
                    UserPassword = "sa.",
                    DataBaseType = DataBaseType.SqlSever
                };


                T8ConfigItemEntity t8ConfigItem = new T8ConfigItemEntity
                {
                    DateType = DateType.Month,
                    DataType = DataType.SaleData,
                    SqlString = "select ISBN AS '书号',Title '书名',PublishName '出版社',Price '定价',KindName '分类',SalesCount '销量',Author '作者',SalesDateTime '销售时间',StoreID '仓号' FROM T8_BookInfo where SalesDateTime >= @StartTime and SalesDateTime <= @EndTime ",

                };
                t8ConfigEntity.T8ConfigItemDic.TryAdd(DateType.Month, t8ConfigItem);

                t8ConfigItem = new T8ConfigItemEntity
                {
                    DateType = DateType.Week,
                    DataType = DataType.SaleData,
                    SqlString = "select ISBN AS '书号',Title '书名',PublishName '出版社',Price '定价',KindName '分类',SalesCount '销量',Author '作者',SalesDateTime '销售时间',StoreID '仓号' FROM T8_BookInfo where SalesDateTime >= @StartTime and SalesDateTime <= @EndTime ",

                };
                t8ConfigEntity.T8ConfigItemDic.TryAdd(DateType.Week, t8ConfigItem);

                T8ConfigHelper.AddT8Config(t8ConfigEntity);
            }

        }

    }
}
