using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread.ParallelThread
{
    /// <summary>
    ///  并行开发学习
    ///  Parallel下面有三个常用的方法invoke,For和ForEach
    /// </summary>
    public class ParallelStudy
    {
        private Stopwatch stopwatch = new Stopwatch();
        public void ParallelInvokeMethod()
        {
            stopwatch.Start();
            Parallel.Invoke(Run1, Run2);
            stopwatch.Stop();
            Console.WriteLine("Parallel run " + stopwatch.ElapsedMilliseconds + " ms.");

            stopwatch.Restart();
            Run1();
            Run2();
            stopwatch.Stop();
            Console.WriteLine("Normal run " + stopwatch.ElapsedMilliseconds + " ms.");
        }

        private void Run1()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Task 1 is cost 2 sec");
        }
        private void Run2()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Task 2 is cost 3 sec");
        }

        /// <summary>
        ///  并行For方法 并行体是纯计算不用同步资源 并行计算比串行计算快很多
        /// </summary>
        public void ParallelForMethod()
        {
            stopwatch.Reset();
            stopwatch.Start();
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 60000; j++)
                {
                    int sum = 0;
                    sum += i;
                }
            }
            stopwatch.Stop();
            Console.WriteLine("NormalFor run " + stopwatch.ElapsedMilliseconds + " ms.");

            stopwatch.Reset();
            stopwatch.Start();

            Parallel.For(0, 10000, item =>
              {
                  //Console.Write($"{item},");
                  for (int j = 0; j < 60000; j++)
                  {
                      int sum = 0;
                      sum += item;
                  }
              });

            stopwatch.Stop();
            Console.WriteLine("ParallelFor run " + stopwatch.ElapsedMilliseconds + " ms.");

        }

        /// <summary>
        /// 并行For方法 并且同步资源
        /// 并行资源在累加时数据安全,lock资源导致比普通运算慢6倍
        /// 是不是大吃一惊啊？Parallel.For竟然用了15秒多，而for跟之前的差不多。
        /// 这主要是由于并行同时访问全局变量，会出现资源争夺，大多数时间消耗在了资源等待上面。
        /// </summary>
        public void ParallelForMethodSyncResource()
        {
            //Parallel.For(0, 100, i => {
            //    Console.Write(i + "\t");
            //});

            var obj = new object();
            long num = 0;
            ConcurrentBag<long> bag = new ConcurrentBag<long>();

            stopwatch.Reset();
            stopwatch.Restart();
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 60000; j++)
                {
                    num++;
                }
            }
            stopwatch.Stop();
            Console.WriteLine("NormalFor run " + stopwatch.ElapsedMilliseconds + $" ms.{num}");

            num = 0;
            stopwatch.Reset();
            stopwatch.Restart();
            Parallel.For(0, 10000, item =>
             {
                 for (int j = 0; j < 60000; j++)
                 {
                     lock (obj)
                     {
                         num++;
                     }
                 }
             });
            stopwatch.Stop();
            Console.WriteLine("ParallelFor run " + stopwatch.ElapsedMilliseconds + $" ms.{num}");
        }

        /// <summary>
        /// 并行Foreach方法 
        /// </summary>
        public void ParallelForeachMethod()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Parallel.ForEach(list, item =>
             {
                 Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},{item}");
             });
        }

        /// <summary>
        /// 并行计算中停止  ParallelLoopState Break和Stop方法来帮我们实现。
        /// Break: 当然这个是通知并行计算尽快的退出循环，比如并行计算正在迭代100，那么break后程序还会迭代所有小于100的。
        /// Stop：这个就不一样了，比如正在迭代100突然遇到stop，那它啥也不管了，直接退出。
        /// </summary>
        public void ParallelBreak()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            stopwatch.Reset();
            stopwatch.Start();

            Parallel.For(0, 1000, (i, state) =>
              {
                  if (bag.Count == 300)
                  {
                      //state.Stop();//这个就不一样了，比如正在迭代100突然遇到stop，那它啥也不管了，直接退出。
                      state.Break();//如果用break,可能结果是300多个或者300个，
                      return;
                  }
                  bag.Add(i);
              });

            stopwatch.Stop();
            Console.WriteLine("Bag count is " + bag.Count + ", " + stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// 并行计算异常处理
        /// 　首先任务是并行计算的，处理过程中可能会产生n多的异常，那么如何来获取到这些异常呢？普通的Exception并不能获取到异常，
        /// 　然而为并行诞生的AggregateExcepation就可以获取到一组异常。
        /// </summary>
        public void ParallelCatchException()
        {
            stopwatch.Start();
            try
            {
                Parallel.Invoke(new Action[] {Run1_Catch,Run2_Catch });
            }
            catch (AggregateException aex) //并列计算异常在AggreaateException中
            {
                foreach (var ex in aex.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            stopwatch.Stop();
            Console.WriteLine("Parallel run " + stopwatch.ElapsedMilliseconds + " ms.");

            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                Run1_Catch();
                Run2_Catch();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            stopwatch.Stop();
            Console.WriteLine("Normal run " + stopwatch.ElapsedMilliseconds + " ms.");

        }          
        
        private void Run1_Catch()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Task 1 is cost 2 sec");
            throw new Exception("Exception in task 1");
        }

        private void Run2_Catch()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Task 2 is cost 3 sec");
            throw new Exception("Exception in task 2");
        }
    }
}
