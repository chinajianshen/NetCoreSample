using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /*
       单例模式的使用自然是当我们的系统中某个对象只需要一个实例的情况，例如:操作系统中只能有一个任务管理器,操作文件时,同一时间内只允许一个实例对其操作等,既然现实生活中有这样的应用场景,自然在软件设计领域必须有这样的解决方案了(因为软件设计也是现实生活中的抽象)，所以也就有了单例模式了。
    */

    /// <summary>
    /// 单例模式
    /// 确保一个类只有一个实例,并提供一个全局访问点
    /// </summary>
    public class Singleton_1
    {
        // 定义一个静态变量来保存类的实例
        private static Singleton_1 uniqueInstance;

        // 定义一个标识确保线程同步
        private static readonly object locker = new object();

        // 定义私有构造函数，使外界不能创建该类实例
        private Singleton_1()
        {

        }

        public static Singleton_1 GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"
            // 双重锁定只需要一句判断就可以了

            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    // 如果类的实例不存在则创建，否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new Singleton_1();
                    }
                }
            }
            return uniqueInstance;
        }

        public void GetMethod()
        {            
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},运行方法");
            Thread.Sleep(1000);
        }

    }

    public class SingletonSample
    {
        public void Process()
        {
            Console.WriteLine("开始...");
           Task t1 = Task.Factory.StartNew(() =>
            {
                Singleton_1.GetInstance().GetMethod();
            });

           Task t2=  Task.Factory.StartNew(() =>
            {
                Singleton_1.GetInstance().GetMethod();
            });

           Task t3 = Task.Factory.StartNew(() =>
            {
                Singleton_1.GetInstance().GetMethod();
            });

            Task.WaitAll(t1, t2, t3);

            Console.WriteLine("结束...");
        }
    }
}
