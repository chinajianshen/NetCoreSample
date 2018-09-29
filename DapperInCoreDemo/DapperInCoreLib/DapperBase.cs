using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace DapperInCoreLib
{
    public class DapperBase : IDapper
    {
        #region Private Fileds
        private IDbConnection _conn;
        private ConnectionType _connectionType;
        private string _connectionString;
        #endregion

        #region Construtor
        public DapperBase(ConnectionType connectionType, string connectionString)
        {
            _connectionType = connectionType;
            _connectionString = connectionString;
        }
        #endregion

        #region Public Property
        private IDbConnection IDbConnection
        {
            get
            {
                switch (_connectionType)
                {
                    case ConnectionType.SqlServer:
                        _conn = new SqlConnection(_connectionString);
                        break;
                    case ConnectionType.Oracle:
                        _conn = new OracleConnection(_connectionString);
                        break;
                    default:
                        _conn = new SqlConnection(_connectionString);
                        break;
                }

                return _conn;
            }
        }
        #endregion

        #region SELECT
        public List<T> GetList<T>(string sqlString, object param = null, CommandType? commandType = CommandType.Text, int? commandTimeout = 180)
        {
            var list = new List<T>();
            
            using (var db = IDbConnection as DbConnection)
            {
                IEnumerable<T> ts = null;
                if (null == param)
                {
                    ts = db.Query<T>(sqlString, null, null, true, commandTimeout, commandType);
                }
                else
                {
                    ts = db.Query<T>(sqlString, param, null, true, commandTimeout, commandType);
                }
                if (null != ts)
                {
                    list = ts.AsList();
                }
            }

            return list;
        }

        public List<T> GetList<T>(string sqlString)
        {
            return GetList<T>(sqlString, null, CommandType.Text);
        }

        public List<T> GetList<T>(string sqlString, object param)
        {
            if (null == param)
            {
                return GetList<T>(sqlString);
            }

            return GetList<T>(sqlString, param);
        }
        #endregion

        #region INSERT
        public bool Insert(string sqlString, object param = null, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }

        public bool Insert<T>(string sqlString, List<T> list, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            var intResult = 0;

            if (null != list && 0 < list.Count)
            {
                using (var db = IDbConnection as DbConnection)
                {
                    intResult = db.Execute(sqlString, list, null, commandTimeOut, commandType);
                }
            }

            return intResult > 0;
        }
        #endregion

        #region UPDATE
        public bool Update(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }
        #endregion

        #region DELETE
        public bool Delete(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            return ExecuteNonQuery(sqlString, param, commandType, commandTimeOut);
        }
        #endregion

        #region Private Methods
        private bool ExecuteNonQuery(string sqlString, object param, CommandType commandType = CommandType.Text, int? commandTimeOut = 5)
        {
            var intResult = 0;
            using (var db = IDbConnection as DbConnection)
            {
                if (null == param)
                {
                    intResult = db.Execute(sqlString, null, null, commandTimeOut, commandType);

                }
                else
                {
                    intResult = db.Execute(sqlString, param, null, commandTimeOut, commandType);
                }
            }

            return intResult > 0;
        }
        #endregion
    }
}