using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Models
{
    /// <summary>
    /// 树节点
    /// </summary>
    public class zTreeNode
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public int pId { get; set; }

        /// <summary>
        /// 结点名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 图标
        /// </summary>

        public string icon { get; set; }
        /// <summary>
        /// 打开的图片
        /// </summary>
        public string iconClose { get; set; }
        /// <summary>
        /// 关闭的图标
        /// </summary>
        public string iconOpen { get; set; }
        /// <summary>
        /// 图标css
        /// </summary>

        public string iconSkin { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 打开方式
        /// </summary>
        public string target { get; set; }

        public zTreeNode()
        {
            target = "_self";
        }
    }
}
