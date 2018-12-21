using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Transfer8Pro.DAO.DataHandlers;
using Transfer8Pro.Entity;
using Transfer8Pro.Utils;

namespace Transfer8Pro.DAO
{
    public class DataHandlerFactory
    {

        static DataHandlerFactory()
        {
            datahandlerTypes = Plugin_ClassLoader<IDataHandler>.LoadTypes();
            dbtypes = new List<KVEntity>();
            foreach (var kv in datahandlerTypes)
            {
                var cs= kv.Value.GetCustomAttributes(typeof(DBTypeNameAttribute), false);
                if (cs.Length > 0)
                {
                    dbtypes.Add(new KVEntity() { K = kv.Key, V = (cs[0] as DBTypeNameAttribute).Name });
                }
            }
            dbtypes.Sort(new Comparison<KVEntity>((v1, v2) => {
                return v1.V.CompareTo(v2.V);
            }));
        }

        static List<KVEntity> dbtypes = new List<KVEntity>();
        static Dictionary<string, Type> datahandlerTypes = new Dictionary<string, Type>();
        static Dictionary<string, IDataHandler> datahandlers = new Dictionary<string, IDataHandler>();

        static object locker = new object();

        public static List<KVEntity> DBTypes {  
            get { return dbtypes; }
        }

        public static IDataHandler GetHandler(string typename)
        {

            if (datahandlers.ContainsKey(typename))
            {
                return datahandlers[typename];
            }

            lock (locker)
            {
                if (datahandlers.ContainsKey(typename))
                {
                    return datahandlers[typename];
                }
                else
                {
                    if (datahandlerTypes.ContainsKey(typename))
                    {
                        IDataHandler handler = Activator.CreateInstance(datahandlerTypes[typename]) as IDataHandler;
                        datahandlers.Add(typename, handler);
                        return handler;
                    }
                    else
                        return null;
                }
            }
        }



    }

    public class Plugin_ClassLoader<T>
    {
        public static Dictionary<string, Type> LoadTypes(string targetfolder, Func<string, bool> filter_func = null)
        {
            Dictionary<string, Type> _CommandDic = new Dictionary<string, Type>();

            string[] files = Directory.GetFiles(targetfolder, "*.dll");
            try
            {
                foreach (string str in files)
                {
                    string filename = Path.GetFileName(str);

                    if (filter_func == null || filter_func(filename))
                    {
                        Dictionary<string, Type> tmp = LoadTypes(str);
                        foreach (var item in tmp)
                        {
                            if (!_CommandDic.ContainsKey(item.Key))
                            {
                                _CommandDic.Add(item.Key, item.Value);
                            }
                        }
                    }
                }
                return _CommandDic;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new Dictionary<string, Type>();
            }
        }

        public static Dictionary<string, Type> LoadTypes(string libpath = "")
        {
            try
            {
                Dictionary<string, Type> _CommandDic = new Dictionary<string, Type>();
                string interface_fullname = typeof(T).FullName;
                Assembly assembly = string.IsNullOrEmpty(libpath) ? Assembly.GetExecutingAssembly() : Assembly.LoadFile(libpath);
                if (assembly != null)
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.GetInterface(interface_fullname) != null && type.GetCustomAttributes(typeof(PluginIgnoreAttribute), true).Length == 0)
                        {
                            _CommandDic.Add(type.FullName, type);
                        }
                    }
                }
                return _CommandDic;
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
                return new Dictionary<string, Type>();
            }

        }
    }
    public class PluginIgnoreAttribute : Attribute
    {

    }
}
