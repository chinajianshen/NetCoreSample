using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSchedule.DBHelper
{
    public class DBConnection
    {
        private static SqlConnection _conn;

        public static SqlConnection Conn
        {
            get
            {
                if (_conn == null || string.IsNullOrEmpty( _conn.ConnectionString))
                { 
                    SqlConnection connection = new SqlConnection(ConstValue.DBConnectionString);
                    _conn = connection;
                }
                return _conn;
            }
        }
        
        public static SqlConnection CreateConnection()
        {
            return Conn;
        }
    }
}
