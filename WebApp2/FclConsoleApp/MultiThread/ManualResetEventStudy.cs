using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
     该对象有两种信号量状态True和False。构造函数设置初始状态。简单来说，

如果构造函数由true创建，则第一次WaitOne()不会阻止线程的执行，而是等待Reset后的第二次WaitOne()才阻止线程执行。
如果构造函数有false创建，则WaitOne()必须等待Set()才能往下执行。
　　一句话总结就是：是否忽略第一次阻塞。 ture忽略第一次阻塞  false不忽略

　　方法如下：
WaitOne：该方法用于阻塞线程，默认是无限期的阻塞，支持设置等待时间，如果超时就放弃阻塞，不等了，继续往下执行；
Set:手动修改信号量为True，也就是恢复线程执行；
ReSet:重置状态； 
     */

    /// <summary>
    /// 手动信号量 
    /// </summary>
    public class ManualResetEventStudy
    {
        //一开始设置为false,当遇到WaitOne()时，需要Set()才能继续执行
        ManualResetEvent mr = new ManualResetEvent(false);

        /// <summary>
        /// ManualResetEvent简单用法
        /// </summary>
        public void Process()
        {
            Thread t = new Thread(Run);
            t.Start();

            //等待辅助线程执行完毕之后，主线程才继续执行
            Console.WriteLine("主线程一边做自己的事，一边等辅助线程执行!" + DateTime.Now.ToString("mm:ss"));
            mr.WaitOne();
            Console.WriteLine("收到信号，主线程继续执行" + DateTime.Now.ToString("mm:ss"));
        }

        ManualResetEvent mr2 = new ManualResetEvent(false);

        public void ProcessManualResetStatus()
        {
            Thread t = new Thread(Run2);
            t.Start();
            mr2.WaitOne();
            Console.WriteLine("第一次等待完成!" + DateTime.Now.ToString("mm:ss"));
            mr2.Reset(); //重置后，又能WaitOne()啦
            mr2.WaitOne(3000);// mr2.Reset()注销则立马执行 因为此时有信号可以不用等待
            Console.WriteLine("第二次等待完成!" + DateTime.Now.ToString("mm:ss"));
        }

        private void Run2()
        {
            Thread.Sleep(1000);
            mr2.Set();
            Thread.Sleep(2000);
            mr2.Set();
        }

        private void Run()
        {
            //模拟长时间任务
            Thread.Sleep(3000);
            Console.WriteLine("辅助线程长时间任务完成！" + DateTime.Now.ToString("mm:ss"));
            mr.Set();
        }
    }
}
