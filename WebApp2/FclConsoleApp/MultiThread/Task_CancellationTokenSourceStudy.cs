using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
     * 在.net 4.0以后，线程池引擎考虑了未来的扩展性，已经充分利用多核微处理器架构，只要在可能的情况下，我们应该尽量使用task，而不是线程池。
     * 
     对于多线程，我们经常使用的是Thread。在我们了解Task之前，如果我们要使用多核的功能可能就会自己来开线程，然而这种线程模型在.net 4.0之后
     被一种称为基于“任务的编程模型”所冲击，因为task会比thread具有更小的性能开销，不过大家肯定会有疑惑，任务和线程到底有什么区别呢？
    
 任务和线程的区别：
1、任务是架构在线程之上的，也就是说任务最终还是要抛给线程去执行。
2、任务跟线程不是一对一的关系，比如开10个任务并不是说会开10个线程，这一点任务有点类似线程池，但是任务相比线程池有很小的开销和精确的控制。

  Task的简略生命周期：
Created：表示默认初始化任务，但是“工厂创建的”实例直接跳过。
WaitingToRun: 这种状态表示等待任务调度器分配线程给任务执行。
RanToCompletion：任务执行完毕
    */

    /// <summary>
    /// Tasks命名空间下的类试图使用任务的概念来解决线程处理的复杂问题。任务(Task)包含一个操作，以及依赖哪个任务的完成才能开始。  
    /// http://www.cnblogs.com/yunfeifei/p/4106318.html
    /// http://www.cnblogs.com/yunfeifei/p/4111112.html
    /// http://www.cnblogs.com/yunfeifei/p/4122084.html
    /// </summary>
    public class Task_CancellationTokenSourceStudy
    {
        /*
 一、 创建Task的方法有两种，一种是直接创建——new一个出来，一种是通过工厂创建。下面来看一下这两种创建方法：
        //第一种创建方式，直接实例化
         var task1 = new Task(() =>
         {
            //TODO you code
         });
          task1.Start();
          task1.Wait(); //阻塞直到任务完成

          //第二种创建方式，工厂创建
         var task2 = Task.Factory.StartNew(() =>
         {
            //TODO you code
         });

 二、Task的任务控制
 1、Task.Wait
在上个例子中，我们已经使用过了，task1.Wait();就是等待任务执行完成，我们可以看到最后task1的状态变为Completed。

2、Task.WaitAll
看字面意思就知道，就是等待所有的任务都执行完成

3、Task.WaitAny
这个用发同Task.WaitAll，就是等待任何一个任务完成就继续向下执行

4、Task.ContinueWith
就是在第一个Task完成后自动启动下一个Task，实现Task的延续
       */


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
            var result = d.ContinueWith<string>(task =>
            {//d任务完成后执行，并带有返回值 
                Console.WriteLine("task d fininshed!");
                return "当前任务完成";
            });
            Console.WriteLine(result);

            //在每次调用ContinueWith方法时，每次会把上次Task的引用传入进来，以便检测上次Task的状态，比如我们可以使用上次Task的Result属性来获取返回值。我们还可以这么写：
            var SendFeedBackTask = Task.Factory.StartNew(() => { Console.WriteLine("Get some Data!"); })
                            .ContinueWith<bool>(s => { return true; })
                            .ContinueWith<string>(r =>
                            {
                                if (r.Result)
                                {
                                    return "Finished";
                                }
                                else
                                {
                                    return "Error";
                                }
                            });
            Console.WriteLine(SendFeedBackTask.Result);

            //其实上面的写法简化一下，可以这样写：
            Task.Factory.StartNew<string>(() => { return "One"; }).ContinueWith(ss => { Console.WriteLine(ss.Result); });
        }

        /// <summary>
        /// Task取消
        /// </summary>
        public void TaskCancelMethod()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            //可以生成多个CancellationToken  当tokenSource.Cancel()都会执行
            CancellationToken token = tokenSource.Token;
            CancellationToken token2 = tokenSource.Token;

            Task task = Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},{i}");
                    Thread.Sleep(1000);
                    if (token.IsCancellationRequested) //判断是否取消
                    {
                        Console.WriteLine("task Abort mission success!");
                        return;
                    }
                }
            }, token);

            Task task2 = Task.Factory.StartNew(() =>
            {
                for (var i = 0; i < 10000; i++)
                {
                    Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},{i}");
                    Thread.Sleep(1000);
                    if (token.IsCancellationRequested) //判断是否取消
                    {
                        Console.WriteLine("task2 Abort mission success!");
                        return;
                    }
                }
            }, token2);

            token.Register(() => { //注册一个取消时触发的委托 委托优先于token.IsCancellationRequested执行
                Console.WriteLine("Canceled");
            });

            Console.WriteLine("Press enter to cancel task...");
            Console.ReadKey();
            tokenSource.Cancel();//取消操作

        }

        /// <summary>
        /// Task非关联嵌套
        /// </summary>
        public void TaskUnNestMethod()
        {
            /*
          Task中的嵌套分为两种，关联嵌套和非关联嵌套，就是说内层的Task和外层的Task是否有联系，下面我们编写代码先来看一下非关联嵌套   
           */

            var pTask = Task.Factory.StartNew(() => {
                Console.WriteLine("Parent task Start!");
                var cTask = Task.Factory.StartNew(() => {
                    Console.WriteLine("Childen task Start!");
                    Thread.Sleep(2000);
                    Console.WriteLine("Childen task finished!");
                });
                Console.WriteLine("Parent task finished!");
            });

            pTask.Wait(); //等待父Task执行完，并不包括里面子Task
            Console.WriteLine("Flag");
        } 

        /// <summary>
        /// Task关联嵌套
        /// </summary>
        public void TaskNestMethod()
        {
            var pTask = Task.Factory.StartNew(() => {
                Console.WriteLine("Parent task Start!");
                var cTask = Task.Factory.StartNew(() => {
                    Console.WriteLine("Childen task Start!");
                    Thread.Sleep(2000);
                    Console.WriteLine("Childen task finished!");
                },TaskCreationOptions.AttachedToParent); //TaskCreationOptions.AttachedToParent和父Task关联
                Console.WriteLine("Parent task finished!");
            });


            pTask.Wait(); //等待父Task执行完，并不包括里面子Task
            Console.WriteLine("Flag");
        }

        /// <summary>
        /// Task综合示例
        /// 任务2和任务3要等待任务1完成后，取得任务1的结果，然后开始执行
        /// 任务4要等待任务2完成，取得其结果才能执行，
        /// 最终任务3和任务4都完成了，合并结果，任务完成
        /// </summary>
        public void TaskSynthesizeSample()
        {
            Task.Factory.StartNew(() => {
                var t1 = Task.Factory.StartNew<int>(() => {
                    Console.WriteLine("Task 1 running...");
                    return 1;
                });
                t1.Wait();//等待任务1完成
                
                //t3要等待t1完成，取其结果执行
                var t3 = Task.Factory.StartNew<int>(() => {
                    Console.WriteLine("Task 3 running...");
                    return t1.Result + 3;
                });

                //t4依赖于t2，根据t2结果执行操作
                var t2_t4 = Task.Factory.StartNew<int>(() => {
                    Console.WriteLine("Task 2 running...");
                    return t1.Result + 2;
                })
                .ContinueWith<int>(task => {//ContinueWith等待t2执行完取其结果
                    Console.WriteLine("Task 4 running...");
                    return task.Result + 4;
                });

                Task.WaitAll(t3, t2_t4);//等待任务3和任务2和4完成
                //最后相加
                var result = Task.Factory.StartNew(() => {
                    Console.WriteLine("Task Finished! The result is {0}", t3.Result + t2_t4.Result);
                });

            });
        }

        /// <summary>
        /// Task异常处理 要用AggregateException
        ///看到父任务pTask异常信息 看不到子任务cTask的，因为他被中断了
        /// </summary>
        public void TaskDealException()
        {
            try
            {
                var pTask = Task.Factory.StartNew(() => {
                    var cTask = Task.Factory.StartNew(() => {
                        System.Threading.Thread.Sleep(2000);
                        throw new Exception("cTask Error!");
                        Console.WriteLine("Childen task finished!");
                    });
                    throw new Exception("pTask Error!");
                    Console.WriteLine("Parent task finished!");
                });

                pTask.Wait(); //一定要有个地方等待任务完成
            }
            catch (AggregateException aex)
            {
                foreach (Exception inner in aex.InnerExceptions)
                {
                    Console.WriteLine(inner.Message);
                }
            }

            Console.WriteLine("Flag");
        }

        /// <summary>
        /// 1、死锁问题 
        /// </summary>
        public void TaskDeadLock()
        {
            Task[] tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() =>
            {               
                Console.WriteLine("Task 1 Start running...");
                while (true) //无限循环
                {
                    System.Threading.Thread.Sleep(1000);
                }
                Console.WriteLine("Task 1 Finished!");
            });
            tasks[1] = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task 2 Start running...");
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Task 2 Finished!");
            });
            //Task.WaitAll(tasks);  //t1无限循环，导致程序假死，导致没有反应

            Task.WaitAll(tasks, 10000); //设置最大等待时间为10秒(项目中可以根据实际情况设置)，如果超过5秒就输出哪个任务出错了
            for (var i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].Status != TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("Task {0} Error! Status:{1}", i + 1,tasks[i].Status);
                }
            }

        }

        /// <summary>
        /// 自旋锁
        ///我们初识多线程或者多任务时，第一个想到的同步方法就是使用lock或者Monitor，
        ///然而在4.0 之后微软给我们提供了另一把利器——spinLock，
        ///它比重量级别的Monitor具有更小的性能开销，它的用法跟Monitor很相似
        /// </summary>
        public void SpinLock()
        {
            /*
            Parallel.For用起来方便，但是在实际开发中还是尽量少用，因为它的不可控性太高，有点简单粗暴的感觉，可能带来一些不必要的"麻烦",最好还是使用Task，因为Task的可控性较好 
            */
            Stopwatch sw = new Stopwatch();
            object obj = new object();
            
            long sum1 = 0;
            long sum2 = 0;
            sw.Start();
            Parallel.For(1, 100000,( i,state) =>
             {
                 //sum1 += i; //多线程非安全代码
                 try
                 {
                     Monitor.Enter(obj);
                     sum1 += i;
                 }
                 catch(AggregateException aex)
                 {
                     state.Stop();
                 }
                 finally
                 {
                     Monitor.Exit(obj);
                 }
                
             });
            sw.Stop();
            Console.WriteLine("Monitor同步锁,值：{0},用时{1}毫秒",sum1,sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Restart();
            SpinLock slock = new SpinLock(false);
            Parallel.For(0,100000,(i,state) => {
                bool lockTaken = false;
                try
                {
                    slock.Enter(ref lockTaken);
                    sum2 += i;
                }
                catch (AggregateException aex)
                {
                    state.Stop();
                }
                finally
                {
                    if (lockTaken)
                        slock.Exit(false);
                }
            });
            sw.Stop();
            Console.WriteLine("SpinLock同步锁,值：{0},用时{1}毫秒", sum2, sw.ElapsedMilliseconds);
        }
    }
    class TestAction
    {
        private int _p;
        private string _actionName;
        public TestAction(string actionName, int p)
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
