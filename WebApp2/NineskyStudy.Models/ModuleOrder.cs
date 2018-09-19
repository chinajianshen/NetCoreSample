using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NineskyStudy.Models
{
    /// <summary>
    /// 模块排序类型
    /// </summary>
    public class ModuleOrder
    {
        [Key]
        public int ModuleOrderId { get; set; }

        /// <summary>
        /// 模块ID
        /// </summary>
        [Required]
        [Display(Name = "模块ID")]
        public int ModuleId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        [Display(Name = "名称")]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required]
        [Display(Name = "值")]
        public int Order { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public virtual Module Module { get; set; }
    }
}
