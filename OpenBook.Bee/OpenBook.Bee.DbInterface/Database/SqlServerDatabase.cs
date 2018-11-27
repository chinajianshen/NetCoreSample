using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.DbInterface.Database
{
    public class SqlServerDatabase : IDatabase
    {
        public IDbConnection Connection => throw new NotImplementedException();

        public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object ExcuteScalarSql(string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }

        public DataTable ExcuteSqlForDataTable(string querySql, string startTime, string endTime)
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

        public void Open()
        {
            throw new NotImplementedException();
        }
       
        public void ExecuteDataToAccess(string dbFileFullPath, string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }

        public void ExecuteDataToSQLite(string dbFileFullPath, string querySql, string startTime, string endTime)
        {
            throw new NotImplementedException();
        }      
    }
}
