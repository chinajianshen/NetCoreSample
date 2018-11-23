using OpenBook.Bee.Interface;
using OpenBook.Bee.Tester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Test.AutoFac
{
    public class AutoFacTest
    {
        public void Test1()
        {
            IService service = AutoFacContainer.Resolve<IService>();
            string add = service.Do();
        }
    }
}
