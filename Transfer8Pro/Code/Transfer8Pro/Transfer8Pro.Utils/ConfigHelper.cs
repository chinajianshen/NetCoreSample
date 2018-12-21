namespace Transfer8Pro.Utils
{
    public class ConfigHelper
    {
        public static string GetConfig(string key,string default_value="")
        {
            return System.Configuration.ConfigurationManager.AppSettings[key] ?? default_value;
        }

        public static string GetDBConnectStringConfig(string key)
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings[key] != null)
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
            }
            else
                return string.Empty;
        }
    }
}
