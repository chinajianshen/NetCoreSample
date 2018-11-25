using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using METTest;
using System.Reflection;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.ComponentModel.Composition.Primitives;

namespace MEFConsoleApplication
{
    //[Export] //不用写
    class Program
    {
        [ImportMany]
        public IEnumerable<IHelloWord> HelloWord { get; set; }

        //[Import]
        //public IHelloWord HelloWord { get; set; }

        [ImportMany]
        public IEnumerable<IOpenBook> OpenBook { get; set; }

        //[Import]
        //public IOpenBook OpenBook { get; set; }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.Method();
        }

        public void Method()
        {
            AggregateCatalog catelog = new AggregateCatalog();
            catelog.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory()));//查找部件，当前应用程序        

            //catelog.Catalogs.Add(new DirectoryCatalog(@"../../../MEFTest1/bin/Debug"));//这个我们通过路径找到部件
            //catelog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            CompositionContainer  container = new CompositionContainer(catelog);//声明容器
            container.ComposeParts(this);//把容器中的部件组合到一起
                                         //CompositionBatch Batch = new CompositionBatch();
                                         //Batch.AddPart(this);
                                         //container.Compose(Batch);
                                         //HelloWord = container.GetExportedValue<IHelloWord>();//这样也可以实例化借口
                                         //Console.WriteLine(HelloWord.SayHello("eric"));
                                         //Console.WriteLine(HelloWord.SayWord("_eric"));

          


            foreach (var item in HelloWord)
            {
                Console.WriteLine(item.SayHello("eric"));
            }

            foreach (var item in OpenBook)
            {
                Console.WriteLine(item.Department());
            }

            //Console.WriteLine(HelloWord.SayHello("eric"));
            //Console.WriteLine(OpenBook.Department());

            Console.Read();
        }
    }
}
