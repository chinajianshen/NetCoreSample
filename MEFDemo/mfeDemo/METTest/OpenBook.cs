using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace METTest
{
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
