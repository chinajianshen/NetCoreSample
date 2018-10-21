using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
异步调用委托的步骤如下：

建立一个委托对象，通过IAsyncResult BeginInvoke(string name,AsyncCallback callback,object state)异步调用委托方法，BeginInvoke方法除最后的两个参数外，其他参数都是与方法参数相对应的。
利用EndInvoke(IAsyncResult--上一步BeginInvoke返回的对象)方法就可以结束异步操作，获取委托的运行结果。
    */

    delegate string MyDelegate(string name, int age);

    /// <summary>
    /// 委托异步调用线程（实质是开启线程池线程处理）
    /// myDelegate.BeginInvoke("刘备",22, null, null); 异步调用委托，除最后两个参数外，前面的参数都可以传进去
    /// </summary>
    public class ThreadAsyncDelegate
    {
        //BeginInvoke() 除了最后两个参数，前面的都是你可定义的      

        /// <summary>
        /// 这种方法有一个缺点，就是不知道异步操作什么时候执行完，什么时候开始调用EndInvoke，因为一旦EndInvoke主线程就会处于阻塞等待状态。
        /// </summary>
        public void Process()
        {
            //建立委托
            MyDelegate myDelegate = new MyDelegate(GetString);
            //异步调用委托，除最后两个参数外，前面的参数都可以传进去
            IAsyncResult result = myDelegate.BeginInvoke("张三", 22, null, null);
            Console.WriteLine($"主线程继续工作!,线程ID：{Thread.CurrentThread.ManagedThreadId}");
            //调用EndInvoke(IAsyncResult)获取运行结果，一旦调用了EndInvoke，即使结果还没来得及返回，主线程也阻塞等待了
            //注意获取返回值的方式
            string data = myDelegate.EndInvoke(result);
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},异步委托结果：{data}");
        }

        /// <summary>
        /// 优化Process()方法 轮询等待中执行其他操作
        /// </summary>
        public void Process2()
        {
            MyDelegate myDelegate = new MyDelegate(GetString);
            IAsyncResult result = myDelegate.BeginInvoke("李四", 25, null, null);
            Console.WriteLine($"主线程继续工作!,线程ID：{Thread.CurrentThread.ManagedThreadId}");

            int couter = 1;
            //while (!result.IsCompleted)  //比上个例子 Process()，只是利用多了一个IsCompleted属性，来判断异步线程是否完成
            //while(!result.AsyncWaitHandle.WaitOne(10)) //等待10毫秒
            WaitHandle[] waitHandles = new WaitHandle[] { result.AsyncWaitHandle }; //是否完成了指定数量
            //while(WaitHandle.WaitAny(waitHandles,10)>0) //是否完成了指定数量
            while (!WaitHandle.WaitAll(waitHandles, 10))
            {
                CalcOther(couter);
                couter++;
            }
            string data = myDelegate.EndInvoke(result);
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},异步委托结果：{data}");
        }


        /// <summary>
        /// 优化Process()方法 轮询等待中执行其他操作
        /// </summary>
        public void Process3()
        {
            MyDelegate myDelegate = new MyDelegate(GetString);
            IAsyncResult result = myDelegate.BeginInvoke("李四", 25, null, null);

            IAsyncResult result2 = myDelegate.BeginInvoke("张三", 20, null, null);
            Console.WriteLine($"主线程继续工作!,线程ID：{Thread.CurrentThread.ManagedThreadId}");

            int couter = 1;
            //while (!result.IsCompleted && !result2.IsCompleted)  //比上个例子 Process()，只是利用多了一个IsCompleted属性，来判断异步线程是否完成           
            WaitHandle[] waitHandles = new WaitHandle[] { result.AsyncWaitHandle, result2.AsyncWaitHandle }; //是否完成了指定数量           
            while (!WaitHandle.WaitAll(waitHandles, 10))
            {
                CalcOther(couter);
                couter++;
            }
            string data = myDelegate.EndInvoke(result);
            string data2 = myDelegate.EndInvoke(result2);
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},异步委托结果1：{data}");
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},异步委托结果2：{data2}");
        }

        /// <summary>
        /// 使用轮询方式来检测异步方法的状态非常麻烦，而且影响了主线程，效率不高。
        /// 使 myDelegate.BeginInvoke("刘备",23, new AsyncCallback(Completed), null); 倒数第二个参数，委托中绑定了完成后的回调方法
        public void Process4()
        {
            /*
             回调函数依然是在辅助线程中执行的，这样就不会影响主线程的运行。
             线程池的线程默认是后台线程。但是如果主线程比辅助线程优先完成，那么程序已经卸载，回调函数未必会执行。如果不希望丢失回调函数中的操作，要么把异步线程设为前台线程，要么确保主线程将比辅助线程迟完成。
             */
            //建立委托
            MyDelegate myDelegate = new MyDelegate(GetString2);
            //倒数第二个参数，委托中绑定了完成后的回调方法
            IAsyncResult asyncResult = myDelegate.BeginInvoke("张三", 21, new AsyncCallback(Completed), null);

            //主线程可以继续工作而不需要等待
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},我是主线程，我干我的活，不再理你！");
        }

        /// <summary>
        /// BeginInvoke("刘备",23, new AsyncCallback(Completed), null)还有最后一个参数没用过的。那么最后一个参数是用来干什么 传参
        /// 通过最一个参数对象，封装当前委托，就可以在回调函数直接调用EndInvoke执行结果
        /// </summary>
        public void Process5()
        {
            MyDelegate myDelegate = new MyDelegate(GetString2);
            Person person = new Person(25, "参数老9", myDelegate);
            IAsyncResult result = myDelegate.BeginInvoke("张三", 25, new AsyncCallback(Completed5), person);

            //主线程可以继续工作而不需要等待
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},我是主线程，我干我的活，不再理你！");
        }

        private void Completed5(IAsyncResult result)
        {
            Person person = result.AsyncState as Person;
            string data = person.MyDelegate.EndInvoke(result);
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},异步委托结果：{data}");
            //异步线程执行完毕
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},异步线程完成咯！");
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},回调函数也是由[" + Thread.CurrentThread.Name + "]调用的！");
        }

        private void Completed(IAsyncResult result)
        {
            //获取委托对象，调用EndInvoke方法获取运行结果
            AsyncResult _result = (AsyncResult)result; //此句是核心点
            MyDelegate myDelegate = (MyDelegate)_result.AsyncDelegate;

            //获得参数
            string data = myDelegate.EndInvoke(result);
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},异步委托结果：{data}");
            //异步线程执行完毕
            Console.WriteLine("异步线程完成咯！");
            Console.WriteLine("回调函数也是由[" + Thread.CurrentThread.Name + "]调用的！");
        }

        private string GetString2(string name, int age)
        {
            Thread.CurrentThread.Name = "异步线程";
            //注意，如果不设置为前台线程，则主线程完成后就直接卸载程序了
            Thread.CurrentThread.IsBackground = false;           

            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},我是不是线程池线程:{Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(3000);
            return $"线程ID:{Thread.CurrentThread.ManagedThreadId},我是{name}，今年{age}岁!";
        }

        private void CalcOther(int cnt)
        {
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},其他计算{cnt}工作中。。。");
            Thread.Sleep(300);
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},其他计算{cnt}工作完成。。。");
        }

        private string GetString(string name, int age)
        {
            Console.WriteLine($"线程ID:{Thread.CurrentThread.ManagedThreadId},我是不是线程池线程:{Thread.CurrentThread.IsThreadPoolThread}");
            Thread.Sleep(3000);
            return $"线程ID:{Thread.CurrentThread.ManagedThreadId},我是{name}，今年{age}岁!";
        }
    }

    class Person
    {
        public Person(int id, string name,MyDelegate myDelegate)
        {
            Id = id;
            Name = name;
            MyDelegate = myDelegate;
        }

        public MyDelegate MyDelegate { get; set; }

        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
