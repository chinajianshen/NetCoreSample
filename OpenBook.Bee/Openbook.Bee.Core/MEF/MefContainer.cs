using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core.MEF
{
   public class MefContainer
    {
        [ImportMany]
        public IEnumerable<IHelloWord> HelloWord { get; set; }

        [ImportMany]
        public IEnumerable<IOpenBook> OpenBook { get; set; }

        //如果接口只有一个实现类，可以这样写，否则不行
        //[Import]
        //public IHelloWord HelloWord { get; set; }

        //[Import]
        //public IOpenBook OpenBook { get; set; } 

        public MefContainer()
        {
            InitContainer();
        }



        private void InitContainer()
        {
            AggregateCatalog catelog = new AggregateCatalog();
            catelog.Catalogs.Add(new DirectoryCatalog(Directory.GetCurrentDirectory(), "OpenBook*.dll"));//查找部件，当前应用程序        

            //catelog.Catalogs.Add(new DirectoryCatalog(@"../../../MEFTest1/bin/Debug"));//这个我们通过路径找到部件
            //catelog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
            CompositionContainer container = new CompositionContainer(catelog);//声明容器
            container.ComposeParts(this);//把容器中的部件组合到一起
                                         //CompositionBatch Batch = new CompositionBatch();
                                         //Batch.AddPart(this);
                                         //container.Compose(Batch);

            //HelloWord = container.GetExportedValue<IHelloWord>();//这样也可以实例化借口
            //Console.WriteLine(HelloWord.SayHello("eric"));
            //Console.WriteLine(HelloWord.SayWord("_eric"));

        }
    }
}
