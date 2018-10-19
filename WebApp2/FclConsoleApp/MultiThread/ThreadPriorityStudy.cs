using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /// <summary>
    /// 线程优先级测试
    /// </summary>
   public class ThreadPriorityStudy
    {
        /// <summary>
        /// t1.Join();      //等待t1执行完之后，主线程再执行，线程间的关系为串行，非并行
        /// </summary>
        public void Process()
        {
            Thread t1 = new Thread(RunPrority);
            t1.Priority = ThreadPriority.Lowest;
            Thread t2 = new Thread(RunPrority);
            t2.Priority = ThreadPriority.Normal;
            Thread t3 = new Thread(RunPrority);
            t3.Priority = ThreadPriority.Highest;
            //由低到高优先级的顺序依次调用
            t1.Start();
            t2.Start();
            t3.Start();          
        }

        public void ThreadInfo()
        {
            Thread t = new Thread(() => {
                Thread t1 = Thread.CurrentThread;// 静态属性，获取当前执行这行代码的线程
                Console.WriteLine("我的优先级是：" + t1.Priority);
                Console.WriteLine("我是否还在执行：" + t1.IsAlive);
                Console.WriteLine("是否是后台线程：" + t1.IsBackground);
                Console.WriteLine("是否是线程池线程：" + t1.IsThreadPoolThread);
                Console.WriteLine("线程唯一标识符：" + t1.ManagedThreadId);
                Console.WriteLine("我的名称是：" + t1.Name);
                Console.WriteLine("我的状态是：" + t1.ThreadState);
            });
            t.Name = "123";
            t.Priority = ThreadPriority.Normal;
            t.Start();
        }

        private void RunPrority()
        {
            Console.WriteLine($"我的优先级是：{Thread.CurrentThread.Priority}");
        }
    }
}
