using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
     　Interrupt和Abort：这两个关键字都是用来强制终止线程，不过两者还是有区别的。

        1、Interrupt:  抛出的是 ThreadInterruptedException 异常。

　　　　 Abort:  抛出的是  ThreadAbortException 异常。

        2、Interrupt：如果终止工作线程，只能管到一次，工作线程的下一次sleep就管不到了，相当于一个contine操作。如果线程正在sleep状态，则通过Interrypt跳过一次此状态也能够达到唤醒效果。

　　　　 Abort：这个就是相当于一个break操作，工作线程彻底停止掉。 当然，你也已在catch（ThreadAbortException ex）{...} 中调用Thread.ResetAbort（）取消终止。
     */
    public class ThreadInterruptStudy
    {
        public void Process()
        {
            Thread t1 = new Thread(Run);
            t1.Start();
            Thread.Sleep(3000);
            //当Interrup时，线程已进入for循环，中断第一次之后，第二次循环无法再停止 相当Continue
            t1.Interrupt();
            t1.Join();// 等待t1执行完之后，主线程再执行，线程间的关系为串行，非并行
            Console.WriteLine("============================================================");

            Thread t2 = new Thread(Run);
            t2.Start();
            //Thread.Sleep(3000);
            t2.Abort();
            Console.WriteLine("============================================================");
            t2.Join();

            Thread t3 = new Thread(Run);
            t3.Start();
            Console.WriteLine("============================================================");
            t3.Join();
        }

        private void Run()
        {
            //多线程同时执行 for()中i变量并没有错乱 执行正常
            for (int i = 1; i <= 5; i++)
            {
                try
                {
                    Thread.Sleep(2000);
                    Console.WriteLine($"当前线程{Thread.CurrentThread.ManagedThreadId},第{i}次Sleep!");
                }
                catch (ThreadInterruptedException ex)
                {
                    Console.WriteLine($"ThreadInterruptedException异常：第{i}次Sleep被中断,{ex.Message}");
                }
                catch(ThreadAbortException ex)
                {
                    Console.WriteLine($"ThreadAbortException异常：第{i}次Sleep被中断,{ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception异常：第{i}次Sleep被中断,{ex.Message}");
                }
            }
        }
    }
}
