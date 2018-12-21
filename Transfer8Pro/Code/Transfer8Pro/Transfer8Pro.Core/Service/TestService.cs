using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Transfer8Pro.DAO;
using Transfer8Pro.DAO.DataHandlers;

namespace Transfer8Pro.Core.Service
{
    public class TestService
    {
        public void Test()
        {
            IDataHandler handler = DataHandlerFactory.GetHandler("Transfer8Pro.DAO.DataHandlers.SqlServer_DataHandler");  //new SqlServer_DataHandler();
            handler.DBConnectStr = "server=192.168.0.14;database=OX;uid=sa;pwd=sa.";
            string sql = "SELECT top 100 AuthorID,AuthorName FROM [OX].[dbo].[AT_AuthorInfo] WHERE CreateTime>@StartTime AND CreateTime<@EndTIme ";
            handler.OutputData(sql, new DateTime(2018, 3, 1), new DateTime(2018, 4, 1), @"R:\tmp.jl");
        }

        public void Test_Oracle()
        {

            var s = DataHandlerFactory.DBTypes;

            IDataHandler handler = DataHandlerFactory.GetHandler("Transfer8Pro.DAO.DataHandlers.Oracle_DataHandler");  //new SqlServer_DataHandler();
            handler.DBConnectStr = "Data Source=(DESCRIPTION=(ADDRESS_LIST= (ADDRESS=(PROTOCOL=TCP)(HOST=192.168.1.60) (PORT=18991)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=ORCL12)));User Id=ox;Password=ox_pwd";
            string sql = "select * from OX_KINDINFO where CreatedTime>:StartTime AND CreatedTime<:EndTIme  ";
            handler.OutputData(sql, new DateTime(2018, 11, 1), new DateTime(2018, 12, 1), @"R:\tmp.jl");
        }
    }
}
