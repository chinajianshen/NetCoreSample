using OpenBook.Bee.Database;
using OpenBook.Bee.Entity;
using OpenBook.Bee.Database.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenBook.Bee.Database.Database;

namespace OpenBook.Bee.Database
{
    /// <summary>
    /// 数据库工厂类
    /// </summary>
    public class DatabaseFactory
    {
        public static IDatabase CreateDatabase(DataBaseInfoEntity dataBaseInfo)
        {
            switch (dataBaseInfo.DataBaseType)
            {
                case DatabaseType.SqlServer:
                    return CreateDatabaseInstance(typeof(SqlServerDatabase), dataBaseInfo);
                case DatabaseType.MySQL:
                    return CreateDatabaseInstance(typeof(MySqlDatabase), dataBaseInfo);
                case DatabaseType.Oracle:
                    return CreateDatabaseInstance(typeof(OracleDatabase), dataBaseInfo);

                default:
                    throw new Exception("Unsupported database type!");
            }
        }

        /// <summary>
        /// 创建数据库对象 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="dataBaseInfo"></param>
        /// <returns></returns>
        private static IDatabase CreateDatabaseInstance(Type dbType, DataBaseInfoEntity dataBaseInfo)
        {
            if (dataBaseInfo == null)
            {
                return null;
            }

            string connString = DataBaseHelper.CreateConnectionString(dataBaseInfo);

            ConstructorInfo constructor = dbType.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                return null;
            }
            IDatabase database = constructor.Invoke(null) as IDatabase;

            if (database != null && !string.IsNullOrEmpty(connString))
            {
                database.ConnectionString = connString;
            }
            
            return database;

        }
    }
}
