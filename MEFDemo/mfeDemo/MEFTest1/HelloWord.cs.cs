using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using METTest;
using System.ComponentModel.Composition;
namespace MEFTest1
{
     [Export(typeof(IHelloWord))]
    public class HelloWord:IHelloWord
    {

        public string SayHello(string str)
        {
            return "Hello" + str;
        }

        public string SayWord(string str)
        {
            return str;
        }
    }
}
