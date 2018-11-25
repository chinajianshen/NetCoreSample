using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OpenBook.Bee.Entity
{
    /// <summary>
    /// 传吧配置文件类
    /// </summary>
    [Serializable]
    public class T8ConfigEntity
    {
        private  ConcurrentDictionary<DateType, T8ConfigItemContainer> _T8ItemContainerDic;

        public  ConcurrentDictionary<DateType, T8ConfigItemContainer> T8ItemContainerDic
        {
            get
            {
                return _T8ItemContainerDic;
            }

            set
            {
                _T8ItemContainerDic = value;
            }
        }

        public T8ConfigEntity()
        {
            _T8ItemContainerDic = new ConcurrentDictionary<DateType, T8ConfigItemContainer>();
            FtpInfo = new FtpInfoEntity();
            DataBaseInfo = new DataBaseInfoEntity();
        }               

        /// <summary>
        /// 商家Pos类型
        /// </summary>
        public PosType PosType { get; set; }

        /// <summary>
        /// 数据库文件类型
        /// </summary>
        public DbFileType DbFileType { get; set; }

        /// <summary>
        /// FTP信息
        /// </summary>
        public FtpInfoEntity FtpInfo { get; set; }

        /// <summary>
        /// 数据库信息
        /// </summary>
        public DataBaseInfoEntity DataBaseInfo { get; set; }
    }

    /// <summary>
    /// T8配置项容器类
    /// </summary>
    [Serializable]
    public class T8ConfigItemContainer
    {
        /// <summary>
        /// 销售数据配置项
        /// </summary>
        public T8ConfigItemEntity T8ConfigItemSale { get; set; }

        /// <summary>
        /// 在架数据配置项
        /// </summary>
        public T8ConfigItemEntity T8ConfigITemOnSale { get; set; }
    }

    /// <summary>
    /// 传8配置项
    /// </summary>
    [Serializable]
    public class T8ConfigItemEntity
    {
        /// <summary>
        /// 日期类型
        /// </summary>
        public DateType DateType { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public DataType DataType { get; set; }       

        /// <summary>
        /// SQL语句
        /// </summary>
        public string SqlString { get; set; }

        /// <summary>
        /// 定时开始日期点
        /// 如 月 15 当月15号
        ///    周 1周一（计算当周具体日期）
        /// </summary>
        //public int TimingStartDate { get; set; }

        /// <summary>
        /// 定时开始时间
        /// </summary>
        //public DateTime TimingStartTime { get; set; }      

        /// <summary>
        ///  定时结束日期点
        /// </summary>
        //public int TimingEndDate { get; set; }

        /// <summary>
        /// 定时结束时间
        /// </summary>
        //public DateTime TimingEndTime { get; set; }       
    }
}
