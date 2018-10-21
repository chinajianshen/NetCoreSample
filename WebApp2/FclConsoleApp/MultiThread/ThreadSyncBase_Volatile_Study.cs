using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
     选择同步对象
任何对所有有关系的线程都可见的对象都可以作为同步对象，但要服从一个硬性规定：它必须是引用类型。
也强烈建议同步对象最好私有在类里面（比如一个私有实例字段）防止无意间从外部锁定相同的对象。
服从这些规则，同步对象可以兼对象和保护两种作用。比如下面List ：

class ThreadSafe {
  List <string> list = new List <string>();
 
  void Test() {
    lock (list) {
      list.Add ("Item 1");

  一个专门字段是常用的(如在先前的例子中的locker) , 因为它可以精确控制锁的范围和粒度。用对象或类本身的类型作为一个同步对象，即：
lock (this) { ... }
或：

lock (typeof (Widget)) { ... }    // 保护访问静态
是不好的，因为这潜在的可以在公共范围访问这些对象。

锁并没有以任何方式阻止对同步对象本身的访问，换言之，x.ToString()不会由于另一个线程调用lock(x) 而被阻止，两者都要调用lock(x) 来完成阻止工作。
     */


    /// <summary>
    /// 线程同步基础学习
    /// http://www.cnblogs.com/kissdodog/archive/2013/04/07/3003822.html
    /// </summary>
    public class ThreadSyncBase_Volatile_Study
    {
        /// <summary>
        /// 多线程更改数据出现错误例子
        /// </summary>
        public void ProcessUnSaleData()
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(UnSaleData));
            }
        }

        static object unSaleDataLock = new object();
        int num = 0;
        private void UnSaleData(object obj)
        {
            //当多线程同时执行该方法时 num就会错乱，此是加锁lock可以解决或 Monitor.Enter() Monitor.Exit()
            //lock (unSaleDataLock)
            //{
            //    Console.WriteLine("当前数字：{0}", ++num);
            //}       

            //获取排他锁
            Monitor.Enter(unSaleDataLock);
            Console.WriteLine("当前数字：{0}", ++num);
            //释放排他锁
            Monitor.Exit(unSaleDataLock);
        }


        Int32 count;//计数值，用于线程同步 （注意原子性，所以本例中使用int32）
        Int32 value;//实际运算值，用于显示计算结果
        /// <summary>
        /// 原子操作同步原理
        /// VolatileWrite：当线程在共享区（临界区）传递信息时，通过此方法来原子性的写入最后一个值；
        /// VolatileRead：当线程在共享区（临界区）传递信息时，通过此方法来原子性的读取第一个值；     
        /// </summary>
        public void ProcessVolatileData()
        {
            //读线程
            Thread thread2 = new Thread(new ThreadStart(VolatileReadData));
            thread2.Start();

            //写线程
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(20);
                Thread thread = new Thread(new ThreadStart(VolatileWriteData));
                thread.Start();
            }
        }

        volatile Int32 countKey=0;//计数值，用于线程同步 （注意原子性，所以本例中使用int32）
        Int32 valueKey=0;//实际运算值，用于显示计算结果
        /// <summary>
        /// Volatile关键字
        /// Volatile关键字的本质含义是告诉编译器，声明为Volatile关键字的变量或字段都是提供给多个线程使用的。
       /// Volatile无法声明为局部变量。作为原子性的操作，Volatile关键字具有原子特性，所以线程间无法对其占有，它的值永远是最新的。
        /// </summary>
        public void ProcessVolatileKeyword()
        {
            //开辟一个线程专门负责读value的值，这样就能看见一个计算的过程
            Thread thread2 = new Thread(new ThreadStart(KeyVolatileRead));
            thread2.Start();
            //开辟10个线程来负责计算，每个线程负责1000万条数据
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(20);
                Thread thread = new Thread(new ThreadStart(KeyVolatileWrite));
                thread.Start();
            }
        }

        /// <summary>
        /// 实际运算写操作
        /// </summary>
        private void KeyVolatileWrite()
        {
            Int32 temp = 0;
            for (int i = 0; i < 10; i++)
            {
                temp += 1;
                Console.WriteLine($"写线程ID:{Thread.CurrentThread.ManagedThreadId},temp:{temp}");
            }
            //真正写入
            valueKey += temp;
            //告诉监听程序，我改变了，读取最新吧！
            countKey = 1;
        }

        /// <summary>
        ///  死循环监控读信息
        /// </summary>
        private void KeyVolatileRead()
        {
            while (true)
            {
                //死循环监听写操作线执行完毕后立刻显示操作结果
                if (countKey == 1)
                {
                    Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId}, 累计计数:{valueKey}");
                    countKey = 0;
                }
            }
        }

        /// <summary>
        /// 实际运算写操作
        /// </summary>
        private void VolatileWriteData()
        {
            Int32 temp = 0;
            for (int i = 0; i < 10; i++)
            {
                temp += 1;
                Console.WriteLine($"写线程ID:{Thread.CurrentThread.ManagedThreadId},temp:{temp}");
            }
            //真正写入
            value += temp;
            Thread.VolatileWrite(ref count, 1);
        }

        /// <summary>
        ///  死循环监控读信息
        /// </summary>
        private void VolatileReadData()
        {
            while (true)
            {
                //死循环监听写操作线执行完毕后立刻显示操作结果
                if (Thread.VolatileRead(ref count) > 0)
                {
                    Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId}, 累计计数:{value}");
                    count = 0;
                }
            }
        }

        /// <summary>
        /// 嵌套Lock测试
        /// 线程只能在最开始的锁或最外面的锁时被阻止。也就是说嵌套在lock(x)里面的lock(x)是可以执行的
        /// </summary>
        public void ProcessNestLock()
        {
            Console.WriteLine("主线程执行开始");
            ThreadPool.QueueUserWorkItem(Run_NestFunc);
            lock (x)
            {
                Console.WriteLine("I have the lock");
                Nest();
                Thread.Sleep(5000);
                Console.WriteLine("I still have the lock");
            }
           
            Console.WriteLine("主线程执行结束");

        }
        static object x = new object();
        private void Nest()
        {
            Console.WriteLine("内层嵌套Lock执行前");
            lock (x)
            {
                Console.WriteLine("内层嵌套Lock执行");
            }
            Console.WriteLine("内层嵌套Lock执行后");
        }

        private void Run_NestFunc(object obj)
        {
            Thread.Sleep(2000);
            Console.WriteLine("Run_NestFunc()执行开始");
            lock (x)
            {
                Console.WriteLine("Run_NestFunc()执行中。。。");
            }
            Console.WriteLine("Run_NestFunc()执行结束");
        }
    }



}
