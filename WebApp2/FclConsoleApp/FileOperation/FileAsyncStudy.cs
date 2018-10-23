using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

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

        /// <summary>
        /// 异步删除文件
        /// </summary>
        private Task DelFileAsync(string fileName)
        {
            return Task.Run(() => File.Delete(fileName));
        }

        public  void FileOperatonAsync()
        {
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},主线程开始");
            Task result = WriteFileAsync(); //创建文件
            result.Wait();
            Task<List<string>> readTask = ReadFileAsync();
            readTask.Wait();
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},主线程结束");
        }


        private async Task<List<string>> ReadFileAsync()
        {
            string filePath = Path.Combine(CurrDirectoryPath, "FileAsyn.txt");
            return await ReadFileAsync(filePath);
        }
        /// <summary>
        /// 普通读取和异步读取 23M文件普通读取快10倍  234M文件普通读比异步读也是将近10倍
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task<List<string>> ReadFileAsync(string filePath)
        {
            List<string> contentList = new List<string>();
            //ConcurrentBag<string> contentList = new ConcurrentBag<string>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None,1024, FileOptions.Asynchronous)) //FileOptions.Asynchronous 异步读
            {
                using (var sr = new StreamReader(stream))
                {
                    while (sr.Peek() > -1)
                    {
                        string line = await sr.ReadLineAsync();
                        contentList.Add(line);
                    }
                }
            }
            sw.Stop();
            Console.WriteLine($"异步读取用时:{sw.ElapsedMilliseconds}，共读取{contentList.Count}行");

            sw.Restart();
            //string[] contentArray = File.ReadAllLines(filePath); //一步读取花时最短
            List<string> contentList2 = new List<string>();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, 1024)) //FileOptions.Asynchronous 异步读
            {
                using (var sr = new StreamReader(stream))
                {
                    while (sr.Peek() > -1)
                    {
                        string line = sr.ReadLine();
                        contentList2.Add(line);
                    }
                }
            }
            sw.Stop();
            //Console.WriteLine($"普通读取用时:{sw.ElapsedMilliseconds}，共读取{contentArray.Length}行");
            Console.WriteLine($"普通读取用时:{sw.ElapsedMilliseconds}，共读取{contentList2.Count}行");

            return contentList;
        }

        private async Task WriteFileAsync()
        {
            string filePath = Path.Combine(CurrDirectoryPath, "FileAsyn.txt");
            await CreateFileAsync(filePath);

          
           
        }

        /// <summary>
        /// 234M byte[]数据 异步创建文件比同步快点（不是太多）
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
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


            //异步2 创建文件 此种方法卡死
            //byte[] buffer3 = Encoding.UTF8.GetBytes(fileContent);
            //sw.Restart();
            //string filePath3 = Path.Combine(CurrDirectoryPath, "FileAsyn3.txt");
            //using (var stream =File.Create(filePath3, 1024, FileOptions.Asynchronous)) //FileOptions.Asynchronous
            //{
            //    using (var streamWriter = new StreamWriter(stream))
            //    {
            //        Console.WriteLine("3. Uses I/O Threads: {0}", stream.IsAsync);
            //        char[] chars3 = Encoding.UTF8.GetChars(buffer3);
            //        await streamWriter.WriteAsync(chars3);
            //    }
            //}
            //sw.Stop();
            //Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},异步2用时：{sw.ElapsedMilliseconds}");

            sw.Reset();
            sw.Start();
            //文件数据量小，普通添加更快
            string filePath2 = Path.Combine(CurrDirectoryPath, "FileAsyn2.txt");
            byte[] buffer2 = Encoding.UTF8.GetBytes(fileContent);
            File.WriteAllBytes(filePath2, buffer2);
            sw.Stop();
            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},同步用时：{sw.ElapsedMilliseconds}");

            

            Console.WriteLine($"线程ID：{Thread.CurrentThread.ManagedThreadId},成功创建文件");
        }

        /// <summary>
        /// 并行1百万循环和普通循环添加StringBuilder数据 ，普通循环是并行数倍
        /// </summary>
        /// <returns></returns>
        private string CreateFileContent()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var sb = new StringBuilder();
            SpinLock spinLock = new SpinLock(false);
            Parallel.For(0, 10000000, i =>
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
            for (int i = 0; i < 10000000; i++)
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
