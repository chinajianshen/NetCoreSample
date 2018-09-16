using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NineskyStudy.Base
{
   /// <summary>
     /// 链接栏目模型
     /// </summary>
     public class CategoryLink
     {
         [Key]
         public int LinkId { get; set; }
 
         /// <summary>
         /// 栏目ID
         /// </summary>
         [Required]
         [Display(Name = "栏目ID")]
         public int CategoryId { get; set; }
 
         /// <summary>
         /// 栏目地址
         /// </summary>
         [Required]
         [DataType(DataType.Url)]
         [StringLength(500)]
         [Display(Name = "栏目地址")]
         public string Url { get; set; }
     }
}
