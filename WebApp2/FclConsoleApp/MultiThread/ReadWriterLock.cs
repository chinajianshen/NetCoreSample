using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread
{
    /*
     写入串行，读取并行；

　　如果程序中大部分都是读取数据的，那么由于读并不影响数据，ReadWriterLock类能够实现”写入串行“，”读取并行“。

　　常用方法如下：
AcquireWriterLock: 获取写入锁； ReleaseWriterLock：释放写入锁。
AcquireReaderLock: 获取读锁； ReleaseReaderLock：释放读锁。
UpgradeToWriterLock:将读锁转为写锁；DowngradeFromWriterLock：将写锁还原为读锁。
    */

    /// <summary>    
    /// 读写锁ReadWriterLock 写入串行，读取并行；
    /// </summary>
    public class ReadWriterLock
    {
        List<string> listStr = new List<string>();
        ReaderWriterLock rw = new ReaderWriterLock();

        /// <summary>
        /// 读写锁有等待时间限制，如果超时则会引发异常，可以循环读几次如 Run3
        /// </summary>
        public void ProcessReadWriterLock()
        {
            Thread t1 = new Thread(Run1);
            Thread t2 = new Thread(Run2);
            Thread t3 = new Thread(Run3);

            t1.Start();
            t1.Name = "刘备";

            t2.Start();
            t2.Name = "关羽";

            t3.Start();
            t3.Name = "张飞";
        }

        private void Run1()
        {
            //获取写锁2秒
            rw.AcquireWriterLock(2000);
            Console.WriteLine(Thread.CurrentThread.Name + "正在写入!");
            listStr.Add("曹操混蛋");
            listStr.Add("孙权王八蛋");
            //Thread.Sleep(1200);
            Thread.Sleep(3000);
            listStr.Add("周瑜个臭小子");
            rw.ReleaseWriterLock();
        }

        //此方法异常，超时，因为写入时不允许读(那么不用测也能猜到更加不允许写咯)
        private void Run2()
        {
            try
            {
                //获取读锁1秒
                //rw.AcquireReaderLock(1000); //如果写锁要2秒 读锁只等1秒肯定超时，所以，读锁等待时间要比写锁锁定时间要长
                rw.AcquireReaderLock(1000);
                Console.WriteLine(Thread.CurrentThread.Name + "正在读取!");
                foreach (string str in listStr)
                {
                    Console.WriteLine(str);
                }
                rw.ReleaseReaderLock();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}读取异常：{ex.Message}");
            }           
        }

        private void Run3()
        {
            int i = 0;
            while (true)
            {               
                try
                {
                    //获取读锁1秒          
                    rw.AcquireReaderLock(1000);
                    Console.WriteLine(Thread.CurrentThread.Name + "正在读取!");
                    foreach (string str in listStr)
                    {
                        Console.WriteLine(str);
                    }
                    rw.ReleaseReaderLock();
                    break;
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name}：第{i}次读取数据失败,{ex.Message}");
                    if (i == 3)
                    {
                        Console.WriteLine($"{Thread.CurrentThread.Name}尝试{i}次读取失败，退出不再读取");
                        break;
                    }
                    i++;
                }
            }
            
        }
    }
}
