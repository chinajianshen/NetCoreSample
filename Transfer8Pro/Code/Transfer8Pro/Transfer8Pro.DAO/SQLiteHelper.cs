using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Transfer8Pro.Utils;

namespace Transfer8Pro.DAO
{
    /// <summary>
    /// SQLite帮助类
    /// </summary>
    public class SQLiteHelper
    {
        /// <summary>
        /// 创建数据库和表 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqliteFileFullpath">数据库文件完整路径</param>
        /// <param name="dts"></param>
        public static void CreateDataBase(string connectionString, string sqliteFileFullpath, DataFieldTypeCollection dts)
        {
            if (CreateNewDatabase(sqliteFileFullpath) && dts != null)
            {
                StringBuilder sbCreateTableSql = new StringBuilder();
                sbCreateTableSql.Append("CREATE TABLE mytable (");
                foreach (DataFieldType dt in dts)
                {
                    sbCreateTableSql.Append(CreateColumn(dt) + ",");
                }
                sbCreateTableSql.Remove(sbCreateTableSql.Length - 1, 1);
                sbCreateTableSql.Append(")");


                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    SQLiteCommand command = new SQLiteCommand(sbCreateTableSql.ToString(), conn);
                    command.ExecuteNonQuery();
                    command.Dispose();
                    conn.Close();
                }
            }
            else
            {
                throw new Exception($"SQLite数据库创建失败![{connectionString}]");
            }
        }

        /// <summary>
        /// 得到数据表的数据类型
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataFieldTypeCollection GetDataStruct(DataTable dt)
        {
            DataFieldTypeCollection dtc = new DataFieldTypeCollection();

            foreach (DataColumn dc in dt.Columns)
            {
                DataFieldType t = new DataFieldType();
                t.FiledName = dc.ColumnName.Trim().Replace("\r", "").Replace("\n", "");
                t.Length = dc.MaxLength;
                t.FieldParameterName = $"@{t.FiledName}";

                string dataTypeName = dc.DataType.FullName.ToLower();
                if (dataTypeName.Contains("int"))
                {
                    t.Type = "int";
                }
                else if (dataTypeName.Contains("datetime"))
                {
                    t.Type = "datetime";
                }
                else
                {
                    t.Type = "string";
                }

                dtc.Add(t);
            }

            return dtc;
        }

        /// <summary>
        /// 创建一个空的数据库
        /// </summary>
        private static bool CreateNewDatabase(string dbname)
        {
            try
            {
                if (File.Exists(dbname))
                {
                    File.Delete(dbname);
                }
                SQLiteConnection.CreateFile(dbname);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("创建SQLite数据库失败" + ex.Message);
            }

        }

        private static string CreateColumn(DataFieldType dt)
        {
            string column = dt.FiledName; ;
            switch (dt.Type.ToLower())
            {
                case "string":
                    if (dt.Length == -1)
                    {
                        column += " text";
                    }
                    else
                    {
                        column += " nvarchar(" + dt.Length + ")";
                    }
                    break;

                case "datetime":
                    column += " datetime";
                    break;

                case "int":
                    column += " int";
                    break;

            }
            return column;
        }

        /// <summary>
        /// 创建SQLite连接串
        /// </summary>
        /// <param name="sqliteFileFullpath"></param>
        /// <returns></returns>
        public static string CreateSQLiteFileConnectionString(string sqliteFileFullpath)
        {
            return string.Format(ConfigHelper.GetConfig("LocalDBConnectStr", "Data Source={0};Version=3;"),sqliteFileFullpath);
        }

        /// <summary>
        /// 构造Sqlite数据文件 SQL插入语句
        /// </summary>
        /// <param name="dataFieldTypes"></param>
        /// <returns></returns>
        public static string CreateInsertSql(DataFieldTypeCollection dataFieldTypes)
        {
            string fieldString = string.Join(",", dataFieldTypes.Select(item => item.FiledName));
            string parameterNameString = string.Join(",", dataFieldTypes.Select(item => item.FieldParameterName));
            string insertSql = $"insert into [mytable]({fieldString}) values({parameterNameString})";
            return insertSql;
        }
    }

    /// <summary>
    /// 数据表字段类型
    /// </summary>
    public class DataFieldType
    {
        private string _filedname;

        public string FiledName
        {
            get { return _filedname; }
            set { _filedname = value; }
        }

        public string FieldParameterName { get; set; }

        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _length;

        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }
    }

    public class DataFieldTypeCollection : IList<DataFieldType>
    {
        IList<DataFieldType> lists = new List<DataFieldType>();
        #region IList<DataFieldType> 成员

        public int IndexOf(DataFieldType item)
        {
            return lists.IndexOf(item);
        }

        public void Insert(int index, DataFieldType item)
        {
            lists.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            lists.RemoveAt(index);
        }

        public DataFieldType this[int index]
        {
            get
            {
                return lists[index];
            }
            set
            {
                lists[index] = value;
            }
        }

        #endregion

        #region ICollection<DataFieldType> 成员

        public void Add(DataFieldType item)
        {
            lists.Add(item);
        }

        public void Clear()
        {
            lists.Clear();
        }

        public bool Contains(DataFieldType item)
        {
            return lists.Contains(item);
        }

        public void CopyTo(DataFieldType[] array, int arrayIndex)
        {
            lists.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return lists.Count; }
        }

        public bool IsReadOnly
        {
            get { return lists.IsReadOnly; }
        }

        public bool Remove(DataFieldType item)
        {
            return lists.Remove(item);
        }

        #endregion

        #region IEnumerable<DataFieldType> 成员

        public IEnumerator<DataFieldType> GetEnumerator()
        {
            return lists.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return lists.GetEnumerator();
        }

        #endregion
    }
}
