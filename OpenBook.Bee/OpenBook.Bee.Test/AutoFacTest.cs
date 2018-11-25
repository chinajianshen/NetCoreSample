using Openbook.Bee.Core.AutoFac;
using OpenBook.Bee.Core;
using OpenBook.Bee.Tester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Test
{
    public class AutoFacTest
    {
        public void Test1()
        {
            CAbstract cAbstract = AutoFacContainer.Resolve<CAbstract>();
            string d00 = cAbstract.Do();
            string d01 = cAbstract.Rule.Validate();

            IAutoService autoService = AutoFacContainer.Resolve<IAutoService>();
            string b00 = autoService.ShowAuto();

            ATest atest = AutoFacContainer.Resolve<ATest>();
            string c00 = atest.title();

            var service = AutoFacContainer.Resolve<IService>();
            string add = service.Do();
            var add01 = AutoFacContainer.ResolveNamed<IService>(typeof(FirstModel).Name);
            var add02 = AutoFacContainer.ResolveNamed<IService>(typeof(FirstModel2).Name);


            ISecondService service2 = AutoFacContainer.Resolve<ISecondService>();
            string add2 = service2.SecondDo();
            string add22 = service2.Do();

            ThirdModel service3 = AutoFacContainer.Resolve<ThirdModel>();
            string add3 = service3.Do();
        }
    }
}
