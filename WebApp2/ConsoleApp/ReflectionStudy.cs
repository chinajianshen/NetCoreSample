using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using NineskyStudy.Base;
using static NineskyStudy.Base.TestClass;
using System.IO;

namespace ConsoleApp
{
    public class ReflectionStudy
    {
        /// <summary>
        /// https://www.cnblogs.com/wangshenhe/p/3256657.html
        /// </summary>
        /// <param name="processObj"></param>
        public void Process(object processObj)
        {
            #region 获取类型
            //Type t = Type.GetType("System.Data.DataTable,System.Data,Version=1.0.3300.0,  Culture=neutral,  PublicKeyToken=b77a5c561934e089");
            //Type t = Type.GetType("System.String"); //默认引用mscorlib.dll，所以查询里面类型可以这种写法
            Type t = processObj.GetType();
            #endregion

            //判断是否实现接口
            if (t.GetInterface("ITest") != null)
            {
                Console.WriteLine($"对象{t.Assembly.FullName}实现接口ITest");
            }

            //根据类型动态创建对象
            Type tDataTable = Type.GetType("System.Data.DataTable,System.Data,Version=1.0.3300.0,  Culture=neutral,  PublicKeyToken=b77a5c561934e089");
            DataTable table = (DataTable)Activator.CreateInstance(tDataTable);
        }

        public void ProcessQuoteAssembly()
        {
            #region 构造类型实例
            //Activator.CreateInstance() //重载系列
            //Activator.CreateInstanceFrom() //重载系列
            //AppDomain.CurrentDomain.CreateInstance() //重载系列
            //AppDomain.CurrentDomain.CreateInstanceFrom() //重载系列
            #endregion

            //1获取指定类型
            //var assembly = Assembly.GetAssembly(typeof(TestClass));
            //Type t = assembly.GetType("NineskyStudy.Base.TestClass"); 
            //2获取指定类型
            //Type t = typeof(TestClass);   
            //3
            Assembly assembly = Assembly.LoadFrom("NineskyStudy.Base.dll");
            Type t = assembly.GetType("NineskyStudy.Base.TestClass");

            object[] constuctParms = new object[] { "hello" };
            //创建实例1
            //TestClass obj = (TestClass)Activator.CreateInstance(t, constuctParms);
            //创建实例2
            TestClass obj = (TestClass)t.InvokeMember("TestClass", BindingFlags.CreateInstance, null, null, constuctParms);
            //通过类型实例直接调用方法
            Console.WriteLine($"GetTestValue()获取的值是：{obj.GetTestValue()}");
            Console.WriteLine($"GetValue()获取的值是：{obj.GetValue("前缀0-")}");
            Console.WriteLine($"属性Value的值是：{obj.Value}");

            //构造器参数
            constuctParms = new object[] { "timmy" };
            //创建对象
            TestClass obj2 = (TestClass)Activator.CreateInstance(t, constuctParms);
            //获取方法信息
            MethodInfo methodInfo = t.GetMethod("GetValue");
            object[] methodParms = new object[] { "前缀-" };
            string methodResult = (string)methodInfo.Invoke(obj2, methodParms);
            Console.WriteLine($"GetValue()获取值是：{methodResult}");
            methodParms = new object[] { "前缀2-" };
            //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值            
            string methodResult2 = (string)methodInfo.Invoke(obj2, BindingFlags.Public | BindingFlags.Instance, Type.DefaultBinder, methodParms, null);
            Console.WriteLine($"GetValue()获取值是：{methodResult2}");

            //获取属性信息
            PropertyInfo propertyInfo = t.GetProperty("Value");
            string propertyValue = (string)propertyInfo.GetValue(obj2);
            Console.WriteLine($"属性Value的值是：{propertyValue}");
        }

        public void PorcessDelegate()
        {
            //1获取指定类型
            //var assembly = Assembly.GetAssembly(typeof(TestClass));
            //Type t = assembly.GetType("NineskyStudy.Base.TestClass");
            //2获取指定类型
            Type t = typeof(TestClass);
            object[] constuctParms = new object[] { "timmy" };
            //TestClass objInstance = (TestClass)Activator.CreateInstance(t,constuctParms);

            MethodInfo methodInfo = t.GetMethod("GetValue2");

            Func<string, string> delegateMethod = (Func<string, string>)Delegate.CreateDelegate(typeof(Func<string, string>), null, methodInfo);
            string returnValue = delegateMethod("hello");
            Console.WriteLine($"Func<>委托调用GetValue()的值是：{returnValue}");

            //此种方法有错误
            //TestClass obj = new TestClass();
            //TestDelegate delegateMethod2 = (TestDelegate)Delegate.CreateDelegate(t, obj, "GetValue2");
            // string retrunValue2 = delegateMethod2("李四");
            //Console.WriteLine($"TestDelegate委托调用GetValue()的值是：{returnValue}");
        }

        public void ProcessAppDomain()
        {

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Console.WriteLine(assembly.FullName);
            }
        }

        public void LoadAllAssemblyByApp()
        {
            string dllPath = Directory.GetCurrentDirectory();
            string[] dlls = Directory.GetFiles(dllPath, "*.dll");
            foreach (var path in dlls)
            {
                Assembly assembly3 = Assembly.LoadFrom(path);
                //Assembly assembly2 = Assembly.Load(path); //此种加载直接地址出错误
                Assembly assembly2 = Assembly.Load("ConsoleApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
                Assembly assembly = Assembly.LoadFile(path);
                Console.WriteLine(assembly.FullName);
            }
        }

        public void GetReflectionInfo()
        {
            Assembly assembly = Assembly.LoadFrom("NineskyStudy.Base.dll");
            //Type[] types = assembly.GetTypes(); //该方法会获取程序集所有类（通常大部分需要）
            Type[] types = assembly.GetExportedTypes(); //如果只需要查找自己开发写的用此方法

            foreach (Type type in types)
            {
                //type.GetInterfaces(""); //获取指定实现接口集
                //type.GetCustomAttributes("");//获取使用自定义特性集
                if (type.Name.Contains("TestClass"))
                {
                    //获取类型的结构信息
                    ConstructorInfo[] constructorInfos = type.GetConstructors();

                    //获取类型的字段信息
                    FieldInfo[] fieldInfos = type.GetFields();

                    //BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance
                    MemberInfo[] memberInfos = type.GetMembers();

                    //获取方法信息
                    MethodInfo[] myMethodInfo = type.GetMethods();
                    //判断方法是否有自定义特性
                    foreach (var methodInfo in myMethodInfo)
                    {
                        object[] customAttr = methodInfo.GetCustomAttributes(typeof(CustomTestAttribue), false);
                        if (customAttr.Length > 0)
                        {
                            Console.WriteLine($"方法{methodInfo.Name}：自定义特性：{customAttr[0].ToString()}");
                        }
                    }

                    //获取属性信息
                    PropertyInfo[] myproperties = type.GetProperties();

                    //获取事件信息
                    EventInfo[] Myevents = type.GetEvents();
                }
            }
        }

        public void InvokeMember()
        {
            Assembly assembly = Assembly.LoadFrom("NineskyStudy.Base.dll");
            Type type = assembly.GetType("NineskyStudy.Base.TestClass");

            //创建实例方法1
            TestClass obj = (TestClass)Activator.CreateInstance(type, new object[] { "jim"});
            //创建实例方法2
            TestClass obj2 = (TestClass)type.InvokeMember("TestClass", BindingFlags.CreateInstance, null, null, new object[] { "jim" });

            //调用目标方法1
            string result = (string)type.InvokeMember("GetValue", BindingFlags.InvokeMethod, Type.DefaultBinder, obj, new object[] { "张三" });
            //调用目标方法2
            MethodInfo mi = type.GetMethod("GetValue");
            string result2 = (string)mi.Invoke(obj, new object[] { "张三" });
            //调用目标方法3
            string result3 = obj.GetValue("张三");

            //属性的读写操作方法1
            obj.GetType().GetProperty("Value").SetValue(obj, "李四");
            string piname = (string)obj.GetType().GetProperty("Value").GetValue(obj, null);
            //属性的读写操作方法2
            PropertyInfo pi = type.GetProperty("Value");
            pi.SetValue(obj, "李四1");
            string piname2 = (string)pi.GetValue(obj, null);
        }
        /// <summary>
        /// 反射调用泛型类
        /// </summary>
        public void TestGenericType()
        {
            Assembly assembly = Assembly.LoadFrom("NineskyStudy.Base.dll");
            Type type = assembly.GetType("NineskyStudy.Base.MyGeneric<T>");
            //检测是否是泛型 type.IsGenericType
            //为泛型类型参数指定System.String类型，并创建实例
            MyGeneric<string> genericObj = (MyGeneric<string>)Activator.CreateInstance(type.MakeGenericType(new Type[] { typeof(System.String) }));

            //生成泛型方法
            MethodInfo m = genericObj.GetType().GetMethod("GetName").MakeGenericMethod(new Type[] { typeof(System.String)});
            //调用泛型方法
            var value = m.Invoke(genericObj, new object[] { "a" });
        }
    }

    public interface ITest
    {
        void GetTest(int id);
    }

    public class TestClass2 : ITest
    {
        public void GetTest(int id)
        {
            Console.WriteLine("实现ITest接口GetTest（）方法");
        }
    }
}
