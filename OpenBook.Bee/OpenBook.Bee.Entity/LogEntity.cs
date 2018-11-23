using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    [Table("ST_SysLogs")]
    [Serializable]
    public class LogEntity
    {
        private int _LogID;
        [PrimaryKey]
        public int LogID
        {
            get { return _LogID; }
            set { _LogID = value; }
        }
        private int _UserID;

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        private string _LogTitle;

        public string LogTitle
        {
            get { return _LogTitle; }
            set { _LogTitle = value; }
        }
        private string _LogContent;

        public string LogContent
        {
            get { return _LogContent; }
            set { _LogContent = value; }
        }

        private DateTime _LogTime = DateTime.Now;
        public DateTime LogTime
        {
            get { return _LogTime; }
            set { _LogTime = value; }
        }

        private int _LogTypeID;

        public int LogTypeID
        {
            get { return _LogTypeID; }
            set { _LogTypeID = value; }
        }
        private string _LogMeta;

        public string LogMeta
        {
            get { return _LogMeta; }
            set { _LogMeta = value; }
        }
        private string _ExInfo;

        public string ExInfo
        {
            get { return _ExInfo; }
            set { _ExInfo = value; }
        }

        private Guid _UnqiueID;

        public Guid UnqiueID
        {
            get { return _UnqiueID; }
            set { _UnqiueID = value; }
        }

        public static string ToJsonString(LogEntity obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        public static LogEntity Parse(string jsonstring)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<LogEntity>(jsonstring);
        }
    }

    public class UserSearchLogs
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ChannelID { get; set; }
        public string SearchCondition { get; set; }
        public DateTime SearchTime { get; set; }
        public int LogID { get; set; }
        public string ExInfo { get; set; }

    }
}
