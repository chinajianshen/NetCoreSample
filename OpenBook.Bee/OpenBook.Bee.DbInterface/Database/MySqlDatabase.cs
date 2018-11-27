using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Database
{
    public class MySqlDatabase : IDatabase
    {
        public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object ExcuteScalarSql(string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }

        public DataTable ExcuteSqlForDataTable(string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }

        public void ExecuteDataToAccess(string accessFileFullpath, string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }

        public void ExecuteDataToSQLite(string sqliteFileFullpath, string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }

        public DataTable GetTableStructure(string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }
    }
}
