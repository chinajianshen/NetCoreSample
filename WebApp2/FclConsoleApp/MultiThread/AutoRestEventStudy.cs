using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
AutoResetEvent	通知正在等待的线程已发生事件
ManualResetEvent	通知正在等待的线程已发生事件
Interlocked	为多个线程共享的变量提供原子操作
Monitor	提供同步对对象的访问的机制
Mutex	一个同步基元，也可用于进程间同步
Thread	创建并控制线程，设置其优先级并获取其状态
ThreadPool 	提供一个线程池，该线程池可用于发送工作项、处理异步 I/O、代表其他线程等待以及处理计时器
WaitHandle	封装等待对共享资源的独占访问的操作系统特定的对象
ReadWriterLock	读写锁
Semaphore	控制线程的访问数量
     */

    public class AutoResetOperation
    {
       public void Process()
        {
            AutoRestEventStudy autoRest = new AutoRestEventStudy();
            Thread thread1 = new Thread(new ThreadStart(autoRest.GetCar));
            thread1.Start();

            Thread thread2 = new Thread(new ThreadStart(autoRest.GetHouse));
            thread2.Start();

            Thread thread3 = new Thread(new ThreadStart(autoRest.GetWife));
            thread3.Start();

            AutoResetEvent.WaitAll(autoRest.autoEvents);
            //或
            //WaitHandle.WaitAll(autoRest.autoEvents);

            autoRest.ShowHappy();
        }
    }

    /// <summary>
    /// AutoResetEvent	通知正在等待的线程已发生事件
    /// </summary>
    public class AutoRestEventStudy
    {
        //建立事件信号数组
        public AutoResetEvent[] autoEvents = null;

        public AutoRestEventStudy()
        {
            autoEvents = new AutoResetEvent[] {
                new AutoResetEvent(false), 
                new AutoResetEvent(false),
                new AutoResetEvent(false)
            };
        }

        public void GetCar()
        {
            Thread.Sleep(5000);
            Console.WriteLine("捡到奔驰");
            autoEvents[0].Set();
        }

        public void GetHouse()
        {
            Thread.Sleep(2000);
            Console.WriteLine("赚到房子");
            autoEvents[1].Set();
        }

        public void GetWife()
        {
            Console.WriteLine("骗到老婆");
            autoEvents[2].Set();
        }

        public void ShowHappy()
        {
            Console.WriteLine("人生要有的都有了，好开心");
        }
    }


}
