using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
     　要搞清楚线程、信号量、事件这三者的关系。实际上3个东西并无具体联系，各自有各自的作用，但是配合起来使用，威力无穷。

　　下面用一个例子，结合事件、信号量、线程来实现如下功能：
主线程启动辅助线程执行一个长时间任务；
辅助线程完成时，触发完成事件()，调用委托，让主线程继续执行；
    */

    /// <summary>
    /// 线程运用例子
    /// 　以上虽然短短几十行代码，但是我却开发了两年多.Net之后才能够领悟。
    /// 　其主要作用是什么，以上达到了线程控制的目的，当我们开发一个核心模块时(LongTimeWork)，仅仅暴露出一个事件(Completed)，
    /// 　调用的人配合上信号量(AutoResetEvent)，就能够随意调用你的核心模块。这也是WF4的调用方式。
    /// </summary>
    public class ThreadUseSample
    {
        AutoResetEvent ar = new AutoResetEvent(false); //初始化参数为true时，则 ar.WaitOne();直接通过，不阻塞等待 ar.Set();
        void MyEventHandler(object sender,EventArgs e)
        {
            ar.Set();
        }

        public void Process()
        {
            LongTimeWork ltw = new LongTimeWork();
            //ltw.Completed += MyEventHandler;
            ltw.Completed += new EventHandler(MyEventHandler);

            Thread t = new Thread(ltw.MyLongTimeWord);
            t.Start();

            //继续忙我的
            Console.WriteLine("等待辅线程时继续忙我的。。。");
            Thread.Sleep(1000);

            //等待辅助线程完成
            ar.WaitOne();
            Console.WriteLine("主线程完成!");
        }
    }

    class LongTimeWork
    {
        //定义一个事件
        public event EventHandler Completed;

        public void MyLongTimeWord()
        {
            Thread.Sleep(4000);
            Console.WriteLine("辅助线程长时间任务完成!");

            //当辅助线程完成时，触发已完成事件
            if (Completed != null)
            {
                Completed(this, new EventArgs());
            }
        }
    }
}
