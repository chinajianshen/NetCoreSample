using OpenBook.Bee.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity
{
    public class FirstModel : IService
    {
        private string first = "first";
        public string Do()
        {
            return first;
        }

    }

    public class SecondModel :ISecondService
    {
        private string second = "second";
        public string Do()
        {
            return second;
        }

        public string SecondDo()
        {
            return $"{second}-{second}";
        }
    }
    public class ThirdModel
    {
        private string third = "third";
        public string Do()
        {
            return third;
        }
    }
}
