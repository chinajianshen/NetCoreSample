using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Transfer8Pro.Core.Infrastructure;

namespace Transfer8Pro.Test
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //注册IOC依赖
            AutoFacConfiguration.RegisterDependencies();

            Application.Run(new MainFrm());

        }
    }
}
