using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /// <summary>
    /// Semaphore可用于进程级交互。
    /// 用于控制线程的访问数量，默认的构造函数为initialCount和maximumCount，表示默认设置的信号量个数和最大信号量个数。
    /// 当你WaitOne的时候，信号量自减，当Release的时候，信号量自增，然而当信号量为0的时候，后续的线程就不能拿到WaitOne了，
    /// 所以必须等待先前的线程通过Release来释放。
    /// </summary>
    public class SemaphoreStudy
    {
        //初始可以授予2个线程信号，因为第3个要等待前面的Release才能得到信号
        Semaphore sem = new Semaphore(2, 10);

        /// <summary>
        /// Semaphore用法
        /// </summary>
        public void Process1()
        {
            Console.WriteLine("主线程开始");
            Thread t1 = new Thread(Run1);
            t1.Start();

            Thread t2 = new Thread(Run2);
            t2.Start();

            Thread t3 = new Thread(Run3);
            t3.Start();

            Thread t4 = new Thread(Run4);
            t4.Start();
            Console.WriteLine("主线程结束");
        }


        Semaphore processSemaphore = new Semaphore(3, 10, "ProcessSemaphore");
        /// <summary>
        ///  进程之间交互
        ///  Semaphore sem = new Semaphore(3, 10, "命名Semaphore"); 第三个参数必须命名
        /// </summary>
        public void ProcessMutual()
        {
            Thread t1 = new Thread(ProcessRun1);
            t1.Start();

            Thread t2 = new Thread(ProcessRun2);
            t2.Start();
        }

        private void ProcessRun1()
        {
            processSemaphore.WaitOne();
            Console.WriteLine("进程：" + Process.GetCurrentProcess().Id + "  我是Run1:" + DateTime.Now.TimeOfDay);
            int cnt =processSemaphore.Release(); //如果不释放，此信号量一直占用着，其他用不了
            Console.WriteLine($"进程：{ Process.GetCurrentProcess().Id},ProcessSemaphore数量：{cnt}");
        }

        private void ProcessRun2()
        {
            processSemaphore.WaitOne();
            Console.WriteLine("进程：" + Process.GetCurrentProcess().Id + "  我是Run2:" + DateTime.Now.TimeOfDay);
            //两秒后
            Thread.Sleep(2000);
            int cnt = processSemaphore.Release(); //如果不释放，此信号量一直占用着，其他用不了
            Console.WriteLine($"进程：{ Process.GetCurrentProcess().Id},ProcessSemaphore数量：{cnt}");
        }

        private void Run1()
        {
            sem.WaitOne();

            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},大家好，我是Run1;" + DateTime.Now.ToString("mm:ss"));
            //两秒后
            Thread.Sleep(2000);

            sem.Release();
        }

        private void Run2()
        {
            sem.WaitOne();

            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},大家好，我是Run2;" + DateTime.Now.ToString("mm:ss"));
            //两秒后
            Thread.Sleep(2000);

            sem.Release();
        }

        private void Run3()
        {
            sem.WaitOne();

            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},大家好，我是Run3;" + DateTime.Now.ToString("mm:ss"));
            //两秒后
            Thread.Sleep(2000);

            sem.Release();
        }

        private void Run4()
        {
            sem.WaitOne();

            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},大家好，我是Run4;" + DateTime.Now.ToString("mm:ss"));
            //两秒后
            Thread.Sleep(2000);

            sem.Release();
        }

    }
}
