using FclConsoleApp.MultiThread;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 多线程
            //AutoResetOperation autoResetOperation = new AutoResetOperation();
            //autoResetOperation.Process();

            //ThreadPriorityStudy threadPriorityStudy = new ThreadPriorityStudy();
            //threadPriorityStudy.Process();
            //threadPriorityStudy.ThreadInfo();

            //ThreadInterruptStudy threadInterruptStudy = new ThreadInterruptStudy();
            //threadInterruptStudy.Process();

            //ThreadParamterStudy paramterThreadStudy = new ThreadParamterStudy();
            //paramterThreadStudy.Process();

            //ThreadTimerStudy threadTimerStudy = new ThreadTimerStudy();
            //threadTimerStudy.Process();

            ThreadPoolStudy threadPoolStudy = new ThreadPoolStudy();
            threadPoolStudy.Process00();
            threadPoolStudy.Process();
            #endregion


            Console.ReadKey();
        }
    }
}
