using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NineskyStudy.Models
{
    public enum LinkTarget
    {
        [Display(Name = "在新窗口中打开")]
        _blank,
        [Display(Name = "在同一框架中打开[默认]")]
        _self,
        [Display(Name = "在父框架中打开")]
        _parent,
        [Display(Name = "在窗口主体中打开")]
        _top
    }
}
