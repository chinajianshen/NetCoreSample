using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Models
{
    /// <summary>
    /// 错误模型
    /// </summary>
    public class ErrorModel
    {
         /// <summary>
         /// 错误
         /// </summary>
         public string Title { get; set; }
 
         /// <summary>
         /// 名称
         /// </summary>
         public string Name { get; set; }
 
         /// <summary>
         /// 描述
         /// </summary>
         public string Description { get; set; }
    }
}
