using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SwaggerCustom.Test.Models
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>

        public string Name { get; set; }
    }
}