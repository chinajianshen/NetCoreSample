using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /// <summary>
    /// 多线程运用示例
    /// </summary>
    public class MultiThreadUseSample
    {
        List<string> htmlPageList = new List<string>();      

        /// <summary>
        /// 单线程采集104页面 差不多需要 11420毫秒
        /// 
        /// </summary>
        public void SingleThreadExecute()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SpliderPage();
            sw.Stop();
            Console.WriteLine("采集100个页面完成，用时:" + sw.ElapsedMilliseconds + "毫秒");
        }

        private void SpliderPage()
        {
            string webPageAddress = "Marketing/Rinkinginfo.aspx?para={0}";
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.BaseAddress = "http://www.openbookdata.com.cn/";
            for (int i = 1; i <= 104; i++)
            {
                int year = 2016 * 100 + i; 
                if (i > 52)
                {
                    year = 2017*100+(i-52);
                }      
                string address = string.Format(webPageAddress, year);               
                string html = wc.DownloadString(address);
                Console.WriteLine($"页面{i}:{wc.BaseAddress + address}下载完成");
                htmlPageList.Add(html);               
            }
        }

      
        public void MultiThreadExectue()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(SpliderPage2));
            }

            while (true)
            {
               if (k == 5)
                {
                    sw.Stop();
                    break;
                }
            }

            Console.WriteLine($"采集{totalcnt}个页面完成，用时:" + sw.ElapsedMilliseconds + "毫秒");
            string orderStr = string.Join(",",dicList.Keys.OrderBy(o => o));
        }

        int totalcnt = 1;
        int i = 0;
        //Volatile关键字的本质含义是告诉编译器，声明为Volatile关键字的变量或字段都是提供给多个线程使用的。        
        volatile int k = 1; //注意关键字
        Dictionary<int, string> dicList = new Dictionary<int, string>();
        static object obj = new object();
        private void SpliderPage2(object state)
        {
            string webPageAddress = "Marketing/Rinkinginfo.aspx?para={0}";
            WebClient wc = new WebClient();
            wc.Encoding = Encoding.UTF8;
            wc.BaseAddress = "http://www.openbookdata.com.cn/";

            while (i<=104)
            {              
                int temp = Interlocked.Increment(ref i); //多线程中局部变量归所属线程所有，不会涉及线程安全问题，如果多个线程使用的公共变量，则必须同步，否则会出问题
                int year = temp > 52 ? 2017 * 100 + (temp - 52) : 2016 * 100 + temp;
              
                string address = string.Format(webPageAddress,  year);
                string html = wc.DownloadString(address);
                Console.WriteLine($"页面{temp}:线程ID{Thread.CurrentThread.ManagedThreadId},{"http://www.openbookdata.com.cn/"} ,{address}下载完成");

                lock (obj)
                {
                    dicList.Add(temp, address);
                    htmlPageList.Add(html);
                }
                Interlocked.Increment(ref totalcnt);             
            }
             //k++;
            Interlocked.Increment(ref k);
        }
    }
}
