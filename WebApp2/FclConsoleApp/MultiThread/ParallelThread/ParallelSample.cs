using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread.ParallelThread
{
    /// <summary>
    ///   并行一个小用法
    /// </summary>
   public class ParallelSample
    {
        private ConcurrentBag<string> _bag = new ConcurrentBag<string>();  //多线程无序集合
        private ConcurrentDictionary<int, string> _dictionary = new ConcurrentDictionary<int, string>();  //多线程键值对集合
        private ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();  //多线程队列
        private ConcurrentStack<string> _stack = new ConcurrentStack<string>();  //多线程堆栈
        private Barrier _barrier = null; //屏障同步 多任务多阶段协同工作
        private SpinLock _spinLock = new SpinLock(false);//自旋锁
        private CountdownEvent _ce = new CountdownEvent(Environment.ProcessorCount); //CPU核对
        private SemaphoreSlim _ss = new SemaphoreSlim(Environment.ProcessorCount, 50);
        private ManualResetEventSlim _mres = new ManualResetEventSlim(false);
        private object _object = new object();

        public void Example_1()
        {
            //Parallel.Invoke(Example1, Example2, Example3, Example4);//并行执行N个方法，执行顺序是随机的，由CPU决定

            Parallel.For(1, 100, (index, loopState) =>
              {
                  Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},并行执行第{index}次");
                  if (index > 30)
                  {
                      loopState.Stop();//立即中止循环
                  }
              });

            List<Product> products = GetProducts();
            Parallel.ForEach(products, (model, loopState) => {
                Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},{model.Name}");
                if (model.SellPrice > 60)
                {
                    loopState.Break();//执行完当前迭代后再中止循环
                }
            });

            //bool lockTaken = false;
            //_spinLock.Enter(ref lockTaken);  //获取锁
            //_spinLock.Exit(false);  //释放锁
            //_ce.Signal();  //注册信号量
            //_ce.Wait();  //等待所有任务完成
            //_ce.Reset();  //重置信号量
            //_ss.Wait();  //使用信号量
            //_ss.Release();  //释放信号量
            //_ss.Release(10);  //释放10个信号量
            //_mres.Wait();  //等待直到设置为可用
            //_mres.Set();  //设置为可用
            //_mres.Reset();  //设置为不可用
        }

        public void TaskUseCancellationTokenSource()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
          
            Task task = new Task(() =>
            {
                SpinWait.SpinUntil(() => {                    
                    Console.WriteLine($"线程ID:{ Thread.CurrentThread.ManagedThreadId},SpinWait.SpinUntil()");
                    Thread.Sleep(1000);
                    return false;
                }, 2000);  //自旋2秒（不释放CPU资源等待2秒）
            }, cts.Token);  //新建任务并添加任务取消参数

            Console.WriteLine($"task状态：{task.Status.ToString()}");
            task.Start();
            Console.WriteLine("task.start()后面语句执行");
            //Thread.Sleep(1000);
            //cts.Cancel();
            Console.WriteLine("task.Cancel()后面语句执行");
            Task.WaitAll(new Task[] { task }, 2000);  //等待任务全部结束或超时2秒 如果cts.Cancel();此时会出现异常
        }

        public void Example_2()
        {
            Task<string> taskResult = Task<string>.Factory.StartNew(() => "TanSea");// 任务带泛型参数来接收返回值
            //Task.WaitAll(taskResult);
            //Console.Write(taskResult.Result.ToString());

            Task continueTask = taskResult.ContinueWith(t => Console.WriteLine(t.Result.ToString())); //等待任务taskResult结束后执行continueTask任务
            Console.WriteLine("主线程执行");
        }

        /// <summary>
        /// 屏障同步
        /// </summary>
        public void BarrierExample()
        {
            _barrier = new Barrier(Environment.ProcessorCount, b => Console.WriteLine(b.CurrentPhaseNumber));
            _barrier.SignalAndWait();
            Console.Write("主线程执行");
            int number = _barrier.ParticipantsRemaining;
            Console.WriteLine($"number:{number}");

        }

        private void Example1()
        {
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},我是方法1");
        }
        private void Example2()
        {
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},我是方法2");
        }
        private void Example3()
        {
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},我是方法3");
        }
        private void Example4()
        {
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},我是方法4");
        }

        private List<Product> GetProducts()
        {
            List<Product> result = new List<Product>();
            for (int index = 1; index < 100; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                result.Add(model);
            }
            return result;
        }
    }
   

    class Product
    {
        public string Category { get; set; }

        public string Name { get; set; }

        public decimal SellPrice { get; set; }
    }
}
