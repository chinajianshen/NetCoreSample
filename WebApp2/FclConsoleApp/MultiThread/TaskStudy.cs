using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /// <summary>
    /// Tasks命名空间下的类试图使用任务的概念来解决线程处理的复杂问题。任务(Task)包含一个操作，以及依赖哪个任务的完成才能开始。    
    /// </summary>
    public class TaskStudy
    {
        /// <summary>
        /// 假设有任务A，B，C，D。其中C依赖A和B的完成，而D依赖A的完成。代码该怎么写呢？ 
        /// 
        /// </summary>
        public void Process()
        {
            /*
             需要注意的是，StartNew方法将立即执行，并不会等待后续的任务加入后才开始，这个是让我刚开始学习时很困惑的。
             而且StartNew方法不是一个同步方法，这意味着将立即执行后面的语句，因此，我们也就模拟出了任务A和B”同时”执行的现象。
           */

            TaskFactory factory = new TaskFactory();
            Task a = factory.StartNew((new TestAction("A", 2)).Do);
            Task b = factory.StartNew((new TestAction("B", 5)).Do);
            Task c = factory.ContinueWhenAll(new Task[] { a, b },
                       (preTasks) => (new TestAction("C", 1)).Do());
            Task d = factory.ContinueWhenAll(new Task[] { a }, 
                      ((preTasks) => (new TestAction("D", 1)).Do()));
        }

    }
     class TestAction
    {
        private int _p;
        private string _actionName;
        public TestAction(string actionName,int p)
        {
            _actionName = actionName;
            _p = p;
        }

        public void Do()
        {
            Console.WriteLine("开始执行" + _actionName);
            Thread.Sleep(new TimeSpan(0, 0, _p));
            Console.WriteLine("执行完毕" + _actionName);
        }
    }
}
