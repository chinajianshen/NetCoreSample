using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace METTest
{
    [Export(typeof(IHelloWord))]
   internal class HelloWord:IHelloWord
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
