using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace METTest
{
    [Export(typeof(IHelloWord))]
    internal class HelloWordB : IHelloWord
    {
        public string SayHello(string str)
        {
            return "我是HelloB:" + str;
        }

        public string SayWord(string str)
        {
            return "B_" + str;
        }
    }
}
