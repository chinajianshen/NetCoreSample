using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
     在学习异步之前先来说说异步的好处，例如对于不需要CPU参数的输入输出操作，可以将实际的处理步骤分为以下三步：

启动处理；
实际的处理，此时不需要CPU参数；
任务完成后的处理；
　　以上步骤如果仅仅使用一个线程，当线程正在处理UI操作时就会出现“卡”的现象。

　　如果使用异步的处理方式，则这三步处理过程涉及到两个线程，主线程中启动第一步；第一步启动后，主线程结束(如果不结束，只会让该线程处于无作为的等待状态)；第二步不需要CPU参与；第二步完成之后，在第二个线程上启动第三步；完成之后第二个线程结束。这样的处理过程中没有一个线程需要处于等待状态，使得运行的线程得到充分利用。
    */

    /// <summary>
    /// http://www.cnblogs.com/kissdodog/archive/2013/03/29/2988212.html
    /// </summary>
    public class ThreadAsyncIOStudy
    {
        private readonly string  folderPath;
        private readonly string filePath;

        /// <summary>
        /// 在FileStream中异步调用I/O线程,当使用BeginRead和BeginWrite方法在执行大量读或写时效果更好，但对于少量读/写，这些方法速度可能比同步还要慢，因为进行线程间的切换需要大量时间
        /// </summary>
        public ThreadAsyncIOStudy()
        {
            folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(),"TestFileFolder");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            filePath = Path.Combine(folderPath, "test.txt");
            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("path是文件的相对路径或绝对路径\r\nmode确定如何打开或创建文件\r\n");
                    writer.Flush();
                }
            }
        }

        /// <summary>
        ///  需要在FileStream中异步调用I/O线程，必须使用以下构造函数建立FileStream对象，并把useAsync设置为true。
        /// </summary>
        public void ProcessFileStream_BeginWrite()
        {
            /*
             FileStream stream = new FileStream(string path,FileMode mode,FileAccess access,FileShare share,int bufferSize,bool useAsync);
　　参数说明：

path是文件的相对路径或绝对路径；
mode确定如何打开或创建文件；
access确认访问文件的方式；
share确定文件如何进程共享；
bufferSize是代表缓冲区大小，一般默认最小值为8，在启动异步读取或写入时，文件大小一般大于缓冲大小；
userAsync代表是否启动异步I/O线程。
             */

            int a, b;
            ThreadPool.GetMaxThreads(out a, out b);
            Console.WriteLine($"线程：{Thread.CurrentThread.ManagedThreadId},原有辅助线程数" + a + "   " + "原有I/O线程数" + b);

            //文件名 文件创建方式 文件权限 文件进程共享 缓冲区大小为1024 是否启动异步I/O线程为true
            //FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true); //如果文件有内容这种方式将内容写到前面
            FileStream stream = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 1024, true); //追加
            //这里要注意，如果写入的字符串很小，则.Net会使用辅助线程写，因为这样比较快
            byte[] bytes = Encoding.Default.GetBytes("\r\n这里要注意，如果写入的字符串很小，则.Net会使用辅助线程写，因为这样比较快\r\n");
            stream.BeginWrite(bytes, 0, (int)bytes.Length, new AsyncCallback(Callback_Write), stream);
            ThreadPool.GetAvailableThreads(out a, out b);
            Console.WriteLine($"线程：{Thread.CurrentThread.ManagedThreadId},现有辅助线程数" + a + "   " + "现有有I/O线程数" + b);
            Console.WriteLine("线程：{Thread.CurrentThread.ManagedThreadId},主线程继续干其他活!");
        }

        /// <summary>
        ///  异步读
        ///  　注意，如果文件过小，小于缓冲区1024，那么可能会调用工作者线程而非I/O线程操作。但是根据我的观察，只是读取文件时文件过小可能会调用辅助线程操作，但是写入时不会。
        /// </summary>
        public void ProcessFileStream_BgeinRead()
        {
            int a, b;
            ThreadPool.GetAvailableThreads(out a, out b);
            Console.WriteLine("原有辅助线程:" + a + "原有I/O线程:" + b);

            byte[] byteData = new byte[1024];
            FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024, true);

            //把FileStream对象，byte[]对象，长度等有关数据绑定到FileDate对象中，以附带属性方式送到回调函数
            Hashtable ht = new Hashtable();
            ht.Add("Length", (int)stream.Length);
            ht.Add("Stream", stream);
            ht.Add("ByteData", byteData);

            //启动异步读取,倒数第二个参数是指定回调函数，倒数第一个参数是传入回调函数中的参数
            stream.BeginRead(byteData, 0, (int)ht["Length"], new AsyncCallback(Callback_Read), ht);

            Console.WriteLine("主线程工作");
            ThreadPool.GetAvailableThreads(out a, out b);
            Console.WriteLine("现有辅助线程:" + a + "现有I/O线程:" + b);
        }

        private void Callback_Read(IAsyncResult result)
        {
            Thread.Sleep(2000);
            //参数result.AsyncState实际上就是Hashtable对象，以FileStream.EndRead完成异步读取
            Hashtable ht = (Hashtable)result.AsyncState;
            FileStream stream = (FileStream)ht["Stream"];
            int length = stream.EndRead(result);           
            stream.Close();
            Console.WriteLine($"stream.EndRead(result)返回长度：{length}");
            string str = Encoding.Default.GetString(ht["ByteData"] as byte[]);
            Console.WriteLine($"读取结果,长度{str.Length}：\r\n {str}");
        }

        private void Callback_Write(IAsyncResult result)
        {
            Console.WriteLine($"线程：{Thread.CurrentThread.ManagedThreadId}，是否是线程池线程：{Thread.CurrentThread.IsThreadPoolThread}");
            Console.WriteLine($"线程：{Thread.CurrentThread.ManagedThreadId},异步写入开始");
            //显示线程池现状
            Thread.Sleep(2000);
            //通过result.AsyncState再强制转换为FileStream就能够获取FileStream对象，用于结束异步写入
            FileStream stream = (FileStream)result.AsyncState;
            stream.EndWrite(result);
            stream.Flush();
            stream.Close();
            Console.WriteLine($"线程：{Thread.CurrentThread.ManagedThreadId},异步写入结束");
        }
    }
}
