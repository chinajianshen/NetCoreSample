using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Openbook.Bee.Core.AutoFac
{
    public class AutoFacContainer
    {
        public static IContainer Container
        {
            get
            {
                return _container;
            }
        }

        private static IContainer _container;

        private AutoFacContainer()
        {
        }

        public static void Initialize(Action<ContainerBuilder> action)
        {
            var builder = new ContainerBuilder();

            if (action != null)
            {
                action(builder);
            }

            _container = builder.Build();
        }

        public static bool IsRegistered<TService>()
        {
            ThrowIfNotInitialized();            
            return _container.IsRegistered<TService>();
        }

        public static bool IsRegistered(Type serviceType)
        {
            ThrowIfNotInitialized();
            return _container.IsRegistered(serviceType);
        }

        public static TService Resolve<TService>(params Parameter[] parameters)
        {            
            ThrowIfNotInitialized();
            return _container.Resolve<TService>(parameters);
        }

        public static object Resolve(Type serviceType, params Parameter[] parameters)
        {
            ThrowIfNotInitialized();
            return _container.Resolve(serviceType, parameters);
        }

        public static TService ResolveNamed<TService>(string serviceName, params Parameter[] parameters)
        {
            ThrowIfNotInitialized();           
            return _container.ResolveNamed<TService>(serviceName, parameters);
        }

        public static object ResolveNamed(string serviceName, Type serviceType, params Parameter[] parameters)
        {
            ThrowIfNotInitialized();
            return _container.ResolveNamed(serviceName, serviceType, parameters);
        }

        public static bool TryResolve<TService>(out TService service)
        {
            ThrowIfNotInitialized();
            return _container.TryResolve<TService>(out service);
        }

        public static bool TryResolve(Type serviceType, out object service)
        {
            ThrowIfNotInitialized();
            return _container.TryResolve(serviceType, out service);
        }

        private static void ThrowIfNotInitialized()
        {
            if (_container == null)
                throw new InvalidOperationException("Container should be initialized before using it.");
        }

    }

    public class AutoFacContainer2
    {
        private Autofac.IContainer container;
        #region 使用单例模式对对象实例化
        private static AutoFacContainer2 aufacContainer = null;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AutoFacContainer2 GetInstance()
        {
            if (aufacContainer == null)
            {
                //使用原子操作方式实现单例模式，解决了多线程问题与双重判断加锁的代码多问题
                Interlocked.CompareExchange<AutoFacContainer2>(ref aufacContainer, new AutoFacContainer2(), null);
            }
            return aufacContainer;
        }

        /// <summary>
        /// 
        /// </summary>
        private AutoFacContainer2() => container = BuildAutofacContainer();
        #endregion

        private Autofac.IContainer BuildAutofacContainer()
        {
            try
            {
                var builder = new ContainerBuilder();
                RegisterTypes(builder);
                var container = builder.Build(ContainerBuildOptions.None);
                return container;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void RegisterTypes(ContainerBuilder builder)
        {
            #region 注册User

            #endregion
        }

        #region 兼容EF和ADO.NET
        public T GetObject<T>() => container.Resolve<T>();

        public T GetObject<T>(string name) => container.ResolveNamed<T>(name);
        #endregion
    }
}
