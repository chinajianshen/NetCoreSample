using OpenBook.Bee.Entity;
using OpenBook.Bee.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Database.Utility
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

    }
}
