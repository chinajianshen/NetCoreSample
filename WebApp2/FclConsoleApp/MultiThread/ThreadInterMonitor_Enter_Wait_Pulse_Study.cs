using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /// <summary>
    /// Monitor.Wait与Monitor.Pulse
    /// Wait() 释放锁定资源，进入等待状态直到被唤醒；
    /// Pulse() 和 PulseAll() 方法用来通知Wait()的线程醒来；
    /// </summary>
    public class ThreadInterMonitor_Enter_Wait_Pulse_Study
    {
        static object obj = new object();

        /// <summary>
        /// 两个线程交互对话
        /// 1首先两个线程争夺同步资源obj
        /// 2最先抢到obj,则另一个线程等待占有线程释放
        /// 3占有线程Monitor.Wait(obj)（暂时释放锁，让等待资源进来），则占有者开始等待资源
        /// 4后来占用线程处理逻辑  Monitor.Pulse(obj);//唤醒另一进程 并让自己处于待待中 Monitor.Wait(obj);
        /// 5来回两个线程进行交互
        /// </summary>
        public void ProcessThreadInter()
        {
            Thread t1 = new Thread(Run1);
            Thread t2 = new Thread(Run2);

            //线程顺序不能变，线程1先执行，否则就死锁了
            t1.Name = "刘备";
            t1.Start();

            t2.Name = "关羽";
            t2.Start();

           

        }

        private void Run1(object state)
        {
            //获取排他锁
            Monitor.Enter(obj);
            Console.WriteLine(Thread.CurrentThread.Name + ":二弟，你上哪去了？");

            Monitor.Wait(obj); //暂时释放锁，让关羽线程进入

            Console.WriteLine(Thread.CurrentThread.Name + ":你混蛋！");

            Monitor.Pulse(obj);//唤醒关羽线程 
            //释放排他锁
            Monitor.Exit(obj);
        }

        private void Run2(object state)
        {
            //获取排他锁
            Monitor.Enter(obj);
            Console.WriteLine(Thread.CurrentThread.Name + ":老子跟曹操了！");

            Monitor.Pulse(obj);
            Monitor.Wait(obj);

            Console.WriteLine(Thread.CurrentThread.Name + ":投降吧，曹孟德当世英雄，竖子不足与谋！！");

            //释放排他锁
            Monitor.Exit(obj);
        }
    }
}
