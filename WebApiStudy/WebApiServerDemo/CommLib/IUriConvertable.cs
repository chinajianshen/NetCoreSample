using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommLib
{
    /// <summary>
    /// 生成：?UserName=XXXX&Age=30
    /// </summary>
    public interface IUriConvertable
    {
        string QueryString { get; }
    }
}
