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

    /*
     AutoResetEvent与ManualResetEvent的区别在于AutoResetEvent 的WaitOne会改变信号量的值为false，让其等待阻塞。
　　比如说初始信号量为True，如果WaitOne超时信号量将自动变为False，而ManualResetEvent则不会。

　　第二个区别：
ManualResetEvent：每次可以唤醒一个或多个线程；
AutoResetEvent：每次只能唤醒一个线程；
    */

    /// <summary>
    /// 信号量
    /// </summary>
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

        AutoResetEvent ar = new AutoResetEvent(true);
       public void Process2()
        {
            Thread t = new Thread(Run);
            t.Start();

            Console.WriteLine($"当前时间1：{DateTime.Now.ToString()}");
            bool state = ar.WaitOne(5000);
            Console.WriteLine("当前的信号量状态:{0}", state);
            Console.WriteLine($"当前时间2：{DateTime.Now.ToString()}");

            state = ar.WaitOne(1000); //再次调用必须加参数等待时间，否则就无限等待，加上时间参数，到时间会自动执行
            Console.WriteLine("再次WaitOne后现在的状态是:{0}", state);

            state = ar.WaitOne(1000); //如果不加参数，阻止当前线程等待
            Console.WriteLine("再次WaitOne后现在的状态是:{0}", state);
        }

        private void Run()
        {
            Console.WriteLine("当前时间" + DateTime.Now.ToString("mm:ss"));
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
