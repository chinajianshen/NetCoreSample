using ADOX;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Database.Utility
{
    /// <summary>
    /// Access帮助类
    /// https://www.cnblogs.com/liyugang/archive/2012/11/17/2775393.html
    /// </summary>
    public class AccessHelper
    {   
        /// <summary>
        /// 创建Access库
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="accessFileFullpath"></param>
        /// <param name="dts"></param>
        public static void CreateDataBase(string connectionString, string accessFileFullpath, DataFieldTypeCollection dts)
        {
            CatalogClass cat = null;
            try
            {
                cat = new CatalogClass();
                cat.Create(connectionString);
            }
            catch (Exception ex)
            {
                throw new Exception($"创建Access数据库失败,路径[{accessFileFullpath}],异常信息:[{ex.Message}]");
            }


            #region 新建表   
            try
            {
                TableClass tbl = new TableClass();
                tbl.ParentCatalog = cat;
                tbl.Name = "mytable";

                foreach (DataFieldType dt in dts)
                {
                    //增加一个文本字段   
                    ColumnClass col2 = new ColumnClass();
                    col2.ParentCatalog = cat;
                    col2.Name = dt.FiledName;
                    col2.Properties["Jet OLEDB:Allow Zero Length"].Value = true;
                    switch (dt.Type.ToLower())
                    {
                        case "string":
                            col2.Type = ADOX.DataTypeEnum.adLongVarWChar;
                            tbl.Columns.Append(col2, ADOX.DataTypeEnum.adLongVarWChar, 16);
                            break;
                        case "datetime":
                            tbl.Columns.Append(col2, ADOX.DataTypeEnum.adDate, dt.Length);
                            break;
                        case "int":
                            tbl.Columns.Append(col2, ADOX.DataTypeEnum.adInteger, dt.Length);
                            break;
                    }
                }
                //把表加入数据库(非常重要)
                cat.Tables.Append(tbl);

                //转换为ADO连接,并关闭
                (cat.ActiveConnection as ADODB.Connection).Close();
                cat.ActiveConnection = null;
                cat = null;
            }
            catch(Exception ex)
            {
                throw new Exception($"创建Access数据库表失败,路径[{accessFileFullpath}],异常信息:[{ex.Message}]");
            }          
            #endregion
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
                string temp = dc.DataType.FullName.ToLower();
                if (temp.Contains("int"))
                {
                    t.Type = "int";
                }
                else if (temp.Contains("datetime"))
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
    }
}
