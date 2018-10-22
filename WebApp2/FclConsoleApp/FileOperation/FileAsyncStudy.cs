using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.FileOperation
{
    /*
     这里主要说明2种异步写入文件的方法：

1）异步编程模型API转为Task——使用Task.Factory.FromAsync方法
2）对于StreamWriter使用WriteAsync方法
请记得对stream对象使用FileOptions.Asynchronous选项
    */

    /// <summary>
    /// 异步读写文件  Task.Factory.FromAsync，WriteAsync
    /// https://www.cnblogs.com/zxtceq/p/7851038.html
    /// </summary>
    public class FileAsyncStudy
    {
        public static readonly string CurrDirectoryPath;

        static FileAsyncStudy()
        {
            CurrDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "FileDirectory");
            if (!Directory.Exists(CurrDirectoryPath))
            {
                Directory.CreateDirectory(CurrDirectoryPath);
            }
        }

        public void FileOperatonAsync()
        {
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},主线程开始");
            Task result = WriteFileAsync();
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},主线程结束");
        }

        private async Task WriteFileAsync()
        {
            string filePath = Path.Combine(CurrDirectoryPath, "FileAsyn.txt");
            await CreateFileAsync(filePath);
        }

        private async Task CreateFileAsync(string filePath)
        {
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},开始创建文件");
            string fileContent = CreateFileContent();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 1024, FileOptions.Asynchronous))
            {
                Console.WriteLine("2. Uses I/O Threads: {0}", stream.IsAsync);
                byte[] buffer = Encoding.UTF8.GetBytes(fileContent);
                var writeTask = Task.Factory.FromAsync(stream.BeginWrite, stream.EndWrite, buffer, 0, buffer.Length, null);
                await writeTask;
            }
            sw.Stop();
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},异步用时：{sw.ElapsedMilliseconds}");

            sw.Reset();
            sw.Restart();
            //文件数据量小，普通添加更快
            string filePath2 = Path.Combine(CurrDirectoryPath, "FileAsyn2.txt");
            byte[] buffer2 = Encoding.UTF8.GetBytes(fileContent);
            File.WriteAllBytes(filePath2, buffer2);
            sw.Stop();
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},同步用时：{sw.ElapsedMilliseconds}");

            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},成功创建文件");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string CreateFileContent()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var sb = new StringBuilder();
            SpinLock spinLock = new SpinLock(false);
            Parallel.For(0, 1000000, i =>
            {
                bool lockTaken = false;
                try
                {
                    spinLock.Enter(ref lockTaken);
                    sb.AppendFormat("{0}\t{1}\t{2}\t{3}", new Random(i).Next(0, 99999), new Random(i).Next(0, 99999), new Random(i).Next(0, 99999), new Random(i).Next(0, 99999));
                    sb.AppendLine();
                }
                finally
                {
                    if (lockTaken)
                    {
                        spinLock.Exit(false);
                    }
                }
            });
            sw.Stop();
            Console.WriteLine($"Parallel For生成时间{sw.ElapsedMilliseconds}");


            sw.Reset();
            sw.Restart();
            sb.Clear();
            for (int i = 0; i < 1000000; i++)
            {
                sb.AppendFormat("{0}\t{1}\t{2}\t{3}", new Random(i).Next(0, 99999), new Random(i).Next(0, 99999), new Random(i).Next(0, 99999), new Random(i).Next(0, 99999));
                sb.AppendLine();
            }
            sw.Stop();
            Console.WriteLine($"普通循环生成时间{sw.ElapsedMilliseconds}");
            return sb.ToString();

        }
    }


}
