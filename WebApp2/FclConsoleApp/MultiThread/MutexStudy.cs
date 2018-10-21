using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /// <summary>
    /// “互相排斥”（mutual exclusion）同步的简单形式（所以名为互斥体(mutex)）
    /// Mutex是一个同步基元，它与前面提到的锁最大的区别在于它支持进程间同步
    /// Mutex允许同一个线程多次重复访问共享区，但是对于别的线程那就必须等待，它甚至支持不同进程中的线程同步，这点更能体现他的优势，
    /// 但是劣势也是显而易见的，那就是巨大的性能损耗和容易产生死锁的困扰，所以除非需要在特殊场合，否则 我们尽量少用为妙，
    /// 这里并非是将Mutex的缺点说的很严重，而是建议大家在适当的场合使用更为适合的同步方式,
    /// Mutex 就好比一个重量型的工具，利用它则必须付出性能的代价。
    /// </summary>
    public class MutexStudy
    {
        /*
    　Mutex和Monitor的区别:
Monitor不是waitHandle的子类，它具有等待和就绪队列的实际应用；
Monitor无法跨进程中实现线程同步，但是Mutex可以；
相对而言两者有明显的性能差距，mutex相对性能较弱但是功能更为强大，monitor则性能比较好；
两者都是用锁的概念来实现同步不同的是monitor一般在方法（函数）调用方加锁；mutex一般在方法（函数）内部加锁，即锁定被调用端；
Monitor和Lock多用于锁定被调用端，而Mutex则多用锁定调用端。
        */
        /// <summary>
        /// 互斥体小例子
        /// </summary>
        public void ProcessMutex()
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(RunMutex);
            }
        }

        /*当给Mutex取名的时候能够实现进程同步，不取名实现线程同步。

　　Mutex有两种类型：未命名的局部mutex和已命名的系统mutex。

本地mutex仅存在与进程当中，进程内可见；
已命名的系统mutex在整个操作系统中可见，可用于同步进程活动；*/
        int pocessSync_count = 0;
        Mutex pocessSync_mutex = new Mutex(false, "xxoo");
        /// <summary>
        /// 进程间同步
        /// Mutex pocessSync_mutex = new Mutex(false, "xxoo"); 已命名的系统mutex在整个操作系统中可见，可用于同步进程活动
        /// </summary>
        public void ProcessSync()
        {
            //使用线程输出等待状态
            Thread t1 = new Thread(ShowMyWord);
            t1.Start();

            Run(t1);
        }

        /// <summary>
        /// 互斥体控制控制台程序仅能启动一次
        /// 由于Mutex能够用于进程间同步，因此我们可以很轻易地利用它实现控制程序只能启动一次的效果。
        /// </summary>
        public void ConsoleAppStartOne()
        {
            bool IsFirstCreate;
            Mutex instance = new Mutex(true, "NewApplication", out IsFirstCreate);
            if (IsFirstCreate) //赋予了线程初始所属权，也就是首次使用互斥体
            {
                instance.ReleaseMutex();
            }
            else
            {
                Console.WriteLine("你已经启动了一个程序,本程序将于5秒后自动退出！");
                Thread.Sleep(5000);
                return;
            }
            Console.WriteLine("程序启动成功!");           
        }

        void Run(Thread t1)
        {
            //这个WaitOne方法要么返回true，要么一直不返回(不会返回false)，所以没办法用if来判断
            //于是，用个线程输出等待状态
            pocessSync_mutex.WaitOne();
            Console.WriteLine("终于轮到老子了！  " + DateTime.Now.TimeOfDay.ToString());
            //停止线程t1,不要再输出等待状态
            t1.Abort();
            //模拟干活十秒
            Thread.Sleep(10000);
            Console.WriteLine("干完！  " + DateTime.Now.TimeOfDay.ToString());
            //释放 Mutex
            pocessSync_mutex.ReleaseMutex();
        }

        void ShowMyWord(object obj)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(2000);
                Console.WriteLine("我的心在等待，一直在等待！   " + DateTime.Now.TimeOfDay.ToString());
            }
        }


        Mutex mutex = new Mutex();
        int count = 0;
        private void RunMutex(object obj)
        {
            //阻止当前线程(如果去掉这两行) 这相当于lock 或 monitor.entry mointor.exit
            mutex.WaitOne();
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},当前数字：{++count}");
            //释放Mutex
            mutex.ReleaseMutex();
        }
    }
}
