using Microsoft.Extensions.Configuration;

namespace DapperInCoreLib
{
    public class DapperFactory
    {
        private static DapperFactory _instance;
        private IDapper _dapper;
        private IConfiguration _configuration;

        private readonly string CONNECTION_STRING = "ConnectionStrings";
        private readonly string STUDENT_CONNECTION_STRING = "StudentConnection";
        private readonly string CONNECTION_TYPE = "ConnectionType";

        public static DapperFactory GetInstance(IConfiguration configuration)
        {
            if (null == _instance)
            {
                _instance = new DapperFactory(configuration);
            }

            return _instance;
        }

        private DapperFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDapper GetDapper()
        {
            if (null == _dapper)
            {
                var connectionString = string.Empty;
                var connectionTypeString = string.Empty;
                ConnectionType connType = ConnectionType.SqlServer;
                if (null != _configuration.GetSection(CONNECTION_STRING) && null != _configuration.GetSection(CONNECTION_STRING).GetSection(STUDENT_CONNECTION_STRING))
                {
                    connectionString = _configuration.GetSection(CONNECTION_STRING).GetSection(STUDENT_CONNECTION_STRING).Value;
                }
                if (null != _configuration.GetSection(CONNECTION_TYPE))
                {
                    connectionTypeString = _configuration.GetSection(CONNECTION_TYPE).Value;
                }
                if (!string.IsNullOrWhiteSpace(connectionTypeString))
                {
                    connType = connectionTypeString.ConvertFromString<ConnectionType>();
                }

                _dapper = new DapperBase(connType, connectionString);
            }

            return _dapper;
        }
    }
}