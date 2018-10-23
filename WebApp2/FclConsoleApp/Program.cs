using FclConsoleApp.FileOperation;
using FclConsoleApp.MultiThread;
using FclConsoleApp.MultiThread.ParallelThread;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp
{
    class Program
    {
        private static string CurrDirectoryPath;
        static Program()
        {
            CurrDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "FileDirectory");
            if (!Directory.Exists(CurrDirectoryPath))
            {
                Directory.CreateDirectory(CurrDirectoryPath);
            }
        }
        static void Main(string[] args)
        {
            #region 多线程
            //AutoResetOperation autoResetOperation = new AutoResetOperation();
            //autoResetOperation.Process();
            //autoResetOperation.Process2();

            //ThreadPriorityStudy threadPriorityStudy = new ThreadPriorityStudy();
            //threadPriorityStudy.Process();
            //threadPriorityStudy.ThreadInfo();

            //ThreadInterruptStudy threadInterruptStudy = new ThreadInterruptStudy();
            //threadInterruptStudy.Process();

            //ThreadParamterStudy paramterThreadStudy = new ThreadParamterStudy();
            //paramterThreadStudy.Process();

            //ThreadTimerStudy threadTimerStudy = new ThreadTimerStudy();
            //threadTimerStudy.Process();

            //ThreadPoolStudy threadPoolStudy = new ThreadPoolStudy();
            //threadPoolStudy.Process00();
            //threadPoolStudy.Process();
            //threadPoolStudy.ProcessUseAutoResetEvent();

            //ThreadAsyncDelegate threadAsyncDelegate = new ThreadAsyncDelegate();
            //threadAsyncDelegate.Process();
            //threadAsyncDelegate.Process2();
            //threadAsyncDelegate.Process3();
            //threadAsyncDelegate.Process4();
            //threadAsyncDelegate.Process5();

            //ThreadAsyncIOStudy iOStudy = new ThreadAsyncIOStudy();
            //iOStudy.ProcessFileStream_BeginWrite();
            //iOStudy.ProcessFileStream_BgeinRead();

            //AsyncWebRequestStudy webRequestStudy = new AsyncWebRequestStudy();
            //webRequestStudy.ProcessHttpWebRequest();

            //ThreadSyncBase_Volatile_Study threadSyncBaseStudy = new ThreadSyncBase_Volatile_Study();
            //threadSyncBaseStudy.ProcessUnSaleData();
            //threadSyncBaseStudy.ProcessVolatileData();
            //threadSyncBaseStudy.ProcessVolatileKeyword();
            //threadSyncBaseStudy.ProcessNestLock();

            //ThreadInterMonitor_Enter_Wait_Pulse_Study monitorInter = new ThreadInterMonitor_Enter_Wait_Pulse_Study();
            //monitorInter.ProcessThreadInter();

            //ReadWriterLock readWriterLock = new ReadWriterLock();
            //readWriterLock.ProcessReadWriterLock();

            //MutexStudy mutexStudy = new MutexStudy();
            //mutexStudy.ProcessMutex();
            //mutexStudy.ProcessSync();
            //mutexStudy.ConsoleAppStartOne();

            //ManualResetEventStudy manualReset = new ManualResetEventStudy();
            //manualReset.Process();
            //manualReset.ProcessManualResetStatus();

            //SemaphoreStudy semaphore = new SemaphoreStudy();
            //semaphore.Process1();
            //semaphore.ProcessMutual();

            //小例子
            //ThreadUseSample threadUseSample = new ThreadUseSample();
            //threadUseSample.Process();

            //小例子
            //MultiThreadUseSample multiThreadUseSample = new MultiThreadUseSample();
            //multiThreadUseSample.SingleThreadExecute();
            //multiThreadUseSample.MultiThreadExectue();

            //Task_CancellationTokenSourceStudy taskStudy = new Task_CancellationTokenSourceStudy();
            //taskStudy.Process();
            //taskStudy.TaskCancelMethod();
            //taskStudy.TaskUnNestMethod();
            //taskStudy.TaskNestMethod();

            //Task综合示例
            //taskStudy.TaskSynthesizeSample();

            //taskStudy.TaskDealException();
            //taskStudy.TaskDeadLock();

            //自旋锁
            //taskStudy.SpinLock();

            #endregion

            #region 并行编程
            //ParallelStudy parallelStudy = new ParallelStudy();
            //parallelStudy.ParallelInvokeMethod();
            //parallelStudy.ParallelForMethod();
            //parallelStudy.ParallelForMethodSyncResource();
            //parallelStudy.ParallelForeachMethod();
            //parallelStudy.ParallelBreak();
            //parallelStudy.ParallelCatchException();

            ParallelEnumerableStudy parallelEnumerableStudy = new ParallelEnumerableStudy();
            //parallelEnumerableStudy.ListWithParallel_UnSafety();
            //parallelEnumerableStudy.ConcurrentBagWithPalle();           
            //parallelEnumerableStudy.AsParallelPLinq();
            //parallelEnumerableStudy.ConcurrentQueueSample();
            //parallelEnumerableStudy.ConcurrentStackSample();
            //parallelEnumerableStudy.ConcurrentDictionaryWithPalle();
            //parallelEnumerableStudy.BlockingCollectionSample();

            //并行综合运用小例子
            //ParallelSample parallelSample = new ParallelSample();
            //parallelSample.Example_1();
            //parallelSample.TaskUseCancellationTokenSource();
            //parallelSample.Example_2();
            //parallelSample.BarrierExample();
            #endregion

            #region 文件操作   
            //FileStudy fileStudy = new FileStudy();

            //创建并删除 （文件及子目录）
            //fileStudy.DirectoryOperation();
            //fileStudy.ReadFileOperation();
            //fileStudy.WriteFileOperation();
            //fileStudy.GetPath();
            //fileStudy.FileAttributeOperation();
            //fileStudy.MoveFolderOperation();
            //fileStudy.CopyFolderOperation();

            string msg = string.Empty;
            string sourcefile = Path.Combine(CurrDirectoryPath, "1.txt");
            string destfile = Path.Combine(CurrDirectoryPath, "2.txt");
            string destfile2 = Path.Combine(CurrDirectoryPath, "3.txt");
            //bool isSuccess = FileHelper.CopyFile(sourcefile,destfile,out msg,1024*1024);
            //bool isSuccess2 = FileHelper.CopyFilePlus(sourcefile, destfile2, out msg, 1024*1024);
            Task<bool> t =  FileHelper.CopyFileAsync(sourcefile, destfile);
            t.Wait();
            bool isSuccess3 = t.Result;
            #endregion

            #region 异步文件操作
            //FileAsyncStudy fileAsyncStudy = new FileAsyncStudy();
            //fileAsyncStudy.FileOperatonAsync();
            #endregion


            Console.ReadKey();
        }
    }
}
