using OpenBook.Bee.Utils;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.QuartzApp.QuartzManager
{
    public class TimeJob : IJob
    {
        private string quartzPath;

        public TimeJob()
        {
            quartzPath = Path.Combine(AppPath.App_Root, "QuartzFile");
            if (!Directory.Exists(quartzPath))
            {
                Directory.CreateDirectory(quartzPath);
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            string filefullpath = Path.Combine(quartzPath, "Quartz.txt");
            File.AppendAllText(filefullpath, $"{DateTime.Now}{Environment.NewLine}");
            return Task.CompletedTask;
        }
    }
}
