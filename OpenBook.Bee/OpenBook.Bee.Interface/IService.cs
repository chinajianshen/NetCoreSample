using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Interface
{
    public interface IService
    {
        string Do();
    }

    public interface ISecondService : IService
    {
        string SecondDo();
    }
}
