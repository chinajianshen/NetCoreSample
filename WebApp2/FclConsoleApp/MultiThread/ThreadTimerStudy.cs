using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
TimerCallback委托专门用于定时器的操作，这个委托允许我们定义一个定时任务，在指定的间隔之后重复调用。实际的类型与ParameterizedThreadStart委托是一样的。

Timer类的构造函数定义如下：

public Timmer(TimerCallback callback,Object state,long dueTime,long period)
Callback表示一个时间到达时执行的委托，这个委托代表的方法必须符合委托TimerCallback的定义。
State表示当调用这个定时器委托时传递的参数。
dutTime表示从创建定时器到第一次调用时延迟的时间，以毫秒为单位。
Period表示定时器开始之后，每次调用之间的时间间隔，以毫秒为单位。
    */

    /// <summary>
    /// 设置每隔1秒执行一次，相当于每次开一个线程（调用方法执行超过1秒，有可能一个线程执行多次）
    /// </summary>
    public class ThreadTimerStudy
    {
        private int Counter = 1;
        public void Process()
        {
            TimerClass timerClass = new TimerClass();
            timerClass.Timer  = new Timer(ShowTime, timerClass, 0, 1000);                 
        }

        private void ShowTime(object userData)
        {
            if (Counter < 6)
            {

                Console.WriteLine($"计数器：{Counter}，当前线程：{Thread.CurrentThread.ManagedThreadId},{DateTime.Now.ToString()}");
                Thread.Sleep(3000);
                Interlocked.Increment(ref Counter);
            }
            else
            {
                ((TimerClass)userData).Timer.Dispose();
            }
        }
    }

    class TimerClass
    {
        public Timer Timer { get; set; }
    }
}
