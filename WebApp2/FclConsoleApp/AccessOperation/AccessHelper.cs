using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FclConsoleApp.AccessOperation
{
    public class AccessHelper
    {
        //private static readonly string dbFolder;
        //private static readonly string connectString;
        //static AccessHelper()
        //{
        //    dbFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AccessDbFolder");
        //    if (!Directory.Exists(dbFolder))
        //    {
        //        try
        //        {
        //            Directory.CreateDirectory(dbFolder);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception($"创建数据库目录失败：{ex.Message}");
        //        }
        //    }
        //    connectString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Jet OLEDB:Engine Type=5";
        //}

        ///// <summary>
        ///// 创建数据库
        ///// </summary>
        ///// <param name="accessDbName"></param>
        ///// <param name="tableName"></param>
        //public static void CreateAccessDB(string dbName, string tableName = "MyTable")
        //{
        //    try
        //    {
        //        string dbFile = Path.Combine(dbFolder, dbName);               
        //        if (!File.Exists(dbFile))
        //        {
        //            Catalog catalog = new Catalog();
        //            catalog.Create(string.Format(connectString,dbFile));
        //        }
               
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}


        ///// <summary>
        ///// 在access数据库中创建表
        ///// </summary>
        ///// <param name="filePath">数据库表文件全路径如D:\\NewDb.mdb 没有则创建 </param> 
        ///// <param name="tableName">表名</param>
        ///// <param name="colums">ADOX.Column对象数组</param>
        //public static void CreateAccessTable(string filePath, string tableName, params ADOX.Column[] colums)
        //{
        //    string dbFile = Path.Combine(dbFolder, filePath);
        //    ADOX.Catalog catalog = new Catalog();
        //    //数据库文件不存在则创建
        //    if (!File.Exists(dbFile))
        //    {
        //        try
        //        {
        //            catalog.Create(string.Format(connectString, dbFile));
        //        }
        //        catch (System.Exception ex)
        //        {

        //        }
        //    }
        //    ADODB.Connection cn = new ADODB.Connection();

        //    cn.Open(string.Format(connectString, dbFile), null, null, -1);
        //    catalog.ActiveConnection = cn;

        //    ADOX.Table table = new ADOX.Table();
        //    table.Name = "FirstTable";

        //    ADOX.Column column = new ADOX.Column();
        //    column.ParentCatalog = catalog;
        //    column.Name = "RecordId";
        //    column.Type = DataTypeEnum.adInteger;
        //    column.DefinedSize = 9;
        //    column.Properties["AutoIncrement"].Value = true;
        //    table.Columns.Append(column, DataTypeEnum.adInteger, 9);
        //    table.Keys.Append("FirstTablePrimaryKey", KeyTypeEnum.adKeyPrimary, column, null, null);
        //    table.Columns.Append("CustomerName", DataTypeEnum.adVarWChar, 50);
        //    table.Columns.Append("Age", DataTypeEnum.adInteger, 9);
        //    table.Columns.Append("Birthday", DataTypeEnum.adDate, 0);
        //    catalog.Tables.Append(table);

        //    cn.Close();


        //}

        //private static Column[] GetColumns()
        //{
        //    ADOX.Column[] columns = {
        //                         new ADOX.Column(){Name="id",Type=DataTypeEnum.adInteger,DefinedSize=9},
        //                         new ADOX.Column(){Name="col1",Type=DataTypeEnum.adWChar,DefinedSize=50},
        //                         new ADOX.Column(){Name="col2",Type=DataTypeEnum.adLongVarChar,DefinedSize=50}
        //                     };
        //    return columns;
        //}


        // AccessDbHelper.CreateAccessTable("d:\\111.mdb", "testTable", columns     
    }
}
