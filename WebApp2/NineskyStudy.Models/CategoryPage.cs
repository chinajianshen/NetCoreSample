using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NineskyStudy.Models
{
    /// <summary>
    /// 单页栏目模型
    /// </summary>
    public class CategoryPage
    {
        [Key]
        public int PageId { get; set; }

        /// <summary>
        /// 栏目ID
        /// </summary>
        [Required]
        [Display(Name = "栏目ID")]
        public int CategoryId { get; set; }

        /// <summary>
        /// 栏目内容
        /// </summary>
        [StringLength(10000)]
        [Display(Name = "栏目内容")]
        public string Content { get; set; }

        /// <summary>
        /// 栏目
        /// </summary>
        public virtual Category Category { get; set; }

    }
}
