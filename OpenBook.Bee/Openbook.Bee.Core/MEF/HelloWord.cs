using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core.MEF
{
    public interface IHelloWord
    {
        string SayHello(string str);
        string SayWord(string str);
    }
    [Export(typeof(IHelloWord))]
    internal class HelloWord : IHelloWord
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


    public interface IOpenBook
    {
        string Department();
    }

    [Export(typeof(IOpenBook))]
    public class OpenBook : IOpenBook
    {
        public string Department()
        {
            return "来自技术部";
        }
    }

    [Export(typeof(IOpenBook))]
    public class OpenBookB : IOpenBook
    {
        public string Department()
        {
            return "来自研究部";
        }
    }
}
