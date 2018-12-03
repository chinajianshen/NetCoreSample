using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace OpenBook.Bee.QuartzTopshelf
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                             //1.我们用HostFactory.Run来设置一个宿主主机。我们初始化一个新的lambda表达式X，来显示这个宿主主机的全部配置。
            {

                x.Service<TownCrier>(s =>                                   //2.告诉Topshelf ，有一个类型为“towncrier服务”,通过定义的lambda 表达式的方式，配置相关的参数。
                {

                    s.ConstructUsing(name => new TownCrier());          //3.告诉Topshelf如何创建这个服务的实例，目前的方式是通过new 的方式，但是也可以通过Ioc 容器的方式：getInstance<towncrier>()。

                    s.WhenStarted(tc => tc.Start());                       //4.开始 Topshelf 服务。

                    s.WhenStopped(tc => tc.Stop());                       //5.停止 Topshelf 服务。

                });

                x.RunAsLocalSystem();                                         //6.这里使用RunAsLocalSystem() 的方式运行，也可以使用命令行(RunAsPrompt())等方式运行。

                x.SetDescription("Sample Topshelf Host");                  //7.设置towncrier服务在服务监控中的描述。

                x.SetDisplayName("Stuff");                                    //8.设置towncrier服务在服务监控中的显示名字。

                x.SetServiceName("Stuff");                                    //9.设置towncrier服务在服务监控中的服务名字。

            });
        }
    }
}
