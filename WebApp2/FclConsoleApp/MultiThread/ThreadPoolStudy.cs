using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
   管理线程开销最好的方式：

尽量少的创建线程并且能将线程反复利用(线程池初始化时没有线程，有程序请求线程则创建线程)；
最好不要销毁而是挂起线程达到避免性能损失(线程池创建的线程完成任务后以挂起状态回到线程池中，等待下次请求)；
通过一个技术达到让应用程序一个个执行工作，类似于一个队列(多个应用程序请求线程池，线程池会将各个应用程序排队处理)；
如果某一线程长时间挂起而不工作的话，需要彻底销毁并且释放资源(线程池自动监控长时间不工作的线程，自动销毁)；
如果线程不够用的话能够创建线程，并且用户可以自己定制最大线程创建的数量(当队列过长，线程池里的线程不够用时，线程池不会坐视不理)；
　　微软早就替我们想到了，为我们实现了线程池。

　　CLR线程池并不会在CLR初始化时立即建立线程，而是在应用程序要创建线程来运行任务时，线程池才初始化一个线程。

　　线程池初始化时是没有线程的，线程池里的。线程的初始化与其他线程一样，但是在完成任务以后，该线程不会自行销毁，而是以挂起的状态返回到线程池。直到应用程序再次向线程池发出请求时，线程池里挂起的线程就会再度激活执行任务。

　　这样既节省了建立线程所造成的性能损耗，也可以让多个任务反复重用同一线程，从而在应用程序生存期内节约大量开销。  
     */

    /// <summary>
    /// 通过CLR线程池所建立的线程总是默认为后台线程，优先级数为ThreadPriority.Normal。
    /// 使用CLR线程池的工作者线程一般有两种方式：
    /// 通过ThreadPool.QueueUserWorkItem() 方法；通过委托；   
    /// </summary>
    public class ThreadPoolStudy
    {      
        public void Process00()
        {
            //工作者线程最大数目，I/O线程的最大数目
            ThreadPool.SetMaxThreads(1000, 1000);

            //启动工作者线程 只能调用有object参数 且无返回值
            ThreadPool.QueueUserWorkItem(RunWorkerThread);

            ThreadPool.QueueUserWorkItem(RunWorkerThread, "参数1");

        }

        public void Process()
        {
            int workerThreads = 0;
            int IOThreads = 0;
            //获取可用线程
            ThreadPool.GetMaxThreads(out workerThreads, out IOThreads);
            Console.WriteLine("最多可用线程："+workerThreads.ToString() + "   " + IOThreads.ToString()); //默认都是1000

            //获取空闲线程，由于现在没有使用异步线程，所以为空
            ThreadPool.GetAvailableThreads(out workerThreads, out IOThreads);
            Console.WriteLine("剩余空闲线程："+workerThreads.ToString() + "   " + IOThreads.ToString()); //默认都是1000
        }      
        
        private void RunWorkerThread(object state)
        {            
            Console.WriteLine($"RunWorkerThread开始工作,线程ID：{Thread.CurrentThread.ManagedThreadId}");
            if (state != null)
            {
                Console.WriteLine($"参数为：{state.ToString()}");
            }
            Console.WriteLine($"工作者线程启动成功!,线程ID：{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
        }
    }
}
