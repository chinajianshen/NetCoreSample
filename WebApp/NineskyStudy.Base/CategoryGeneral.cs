using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NineskyStudy.Base
{
    /// <summary>
      /// 常规栏目模型
      /// </summary>
      public class CategoryGeneral
      {
          [Key]
          public int GeneralId { get; set; }
  
          /// <summary>
          /// 栏目ID
          /// </summary>
          [Required]
          [Display(Name = "栏目ID")]
          public int CategoryId { get; set; }
  
          /// <summary>
          /// 栏目视图
          /// </summary>
          [Required]
          [StringLength(200)]
          [Display(Name = "栏目视图")]
          public string View { get; set; }
  
          /// <summary>
          /// 模块名称
          /// </summary>
          [Required]
          [StringLength(50)]
          [Display(Name = "模块名称")]
          public string Module { get; set; }
  
          /// <summary>
          /// 内容视图
          /// </summary>
          [Required]
          [StringLength(200)]
          [Display(Name = "内容视图")]
          public string ContentView { get; set; }
  
          /// <summary>
          /// 内容排序
          /// </summary>
          [Required]
          [StringLength(200)]
          [Display(Name = "内容排序")]
          public int? ContentOrder { get; set; }
  
          /// <summary>
          /// 栏目
          /// </summary>
          public virtual Category Category { get; set; }
      }
  }
