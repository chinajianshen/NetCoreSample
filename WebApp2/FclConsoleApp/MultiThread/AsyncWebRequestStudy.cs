using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
System.Net.WebRequest是.NET为实现Internet的"请求/响应模型"而开发的一个abstract基类。它主要有三个子类：
FtpWebRequest，FileWebRequest使用"file://路径"的URI方式实现对本地资源和内部文件的请求/响应；
HttpWebRequest，FtpWebRequest使用FTP文件传输协议实现文件请求/响应；
FileWebRequest，HttpWebRequest用于处理HTTP的页面请求/响应；
   */

    /// <summary>
    /// 异步WebRequest
    /// http://www.cnblogs.com/kissdodog/archive/2013/03/29/2988212.html
    /// </summary>
    public class AsyncWebRequestStudy
    {
        /// <summary>
        /// 注意：请求与响应不能使用同步与异步混合开发模式，即当请求写入使用GetRequestStream同步模式，即使响应使用BeginGetResponse异步方法，
        /// 操作也与GetRequestStream方法在于同一线程内。      
        /// BeginGetRequestStream、EndGetRequestStream用于异步向HttpWebRequest对象写入请求信息;
        ///BeginGetResponse、EndGetResponse用于异步发送页面请求并获取返回信；
        /// </summary>
        public void ProcessHttpWebRequest()
        {
            int a, b;
            ThreadPool.GetAvailableThreads(out a, out b);
            Console.WriteLine("原有辅助线程:" + a + "原有I/O线程:" + b);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://www.baidu.com");
            webRequest.Method = "post";

            //对写入数据的RequestStream对象进行异步请求
            IAsyncResult result = webRequest.BeginGetResponse(new AsyncCallback(EndHttpGetResponse), webRequest);

            Thread.Sleep(1000);
            ThreadPool.GetAvailableThreads(out a, out b);
            Console.WriteLine("现有辅助线程:" + a + "现有I/O线程:" + b);

            Console.WriteLine("主线程继续干其他事!");

        }

        private void EndHttpGetResponse(IAsyncResult result)
        {
            Thread.Sleep(2000);
            //结束异步请求，获取结果
            HttpWebRequest webRequest = (HttpWebRequest)result.AsyncState;
            WebResponse webResponse = webRequest.EndGetResponse(result);

            Stream stream = webResponse.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string html = sr.ReadToEnd();
            stream.Close();
            sr.Close();
            Console.WriteLine(html);
        }
    }
}
