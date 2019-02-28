using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CommLib;
using WebApiContract.Validators;

namespace WebApiContract.Models
{
    //此类是为了减少重复代码
    public class UserInfo_API_Base
    {
        [Required(ErrorMessage = Verifier.ERRMSG_CANNOT_BE_NULL)]
        [RegularExpression(Verifier.REG_EXP_CHINESE_NAME, ErrorMessage = Verifier.ERRMSG_REG_EXP_CHINESE_NAME)]
        public string RealName { get; set; }     //真实姓名

        public float Height { get; set; }        //身高

        public DateTime Birthday { get; set; }   //生日
    }

    //获取用户信息（普通用户只能获取自己的信息）
    //GET api/usersinfo 或 GET /api/usersinfo/{username}
    public class UserInfo_API_Get : UserInfo_API_Base
    {
        public string UserName { get; set; }     //用户名
        public string Role { get; set; }         //角色Administrator, Normal

        public long UpdateTicks { get; set; }    //时间戳（用于PUT和DELETE时候的并行写入冲突检查）
    }

    //增加用户（需要管理员权限）
    //POST api/usersinfo
    public class UserInfo_API_Post : UserInfo_API_Base
    {
        [Required(ErrorMessage = Verifier.ERRMSG_CANNOT_BE_NULL)]
        [RegularExpression(Verifier.REG_EXP_USER_NAME, ErrorMessage = Verifier.ERRMSG_REG_EXP_USER_NAME)]
        public string UserName { get; set; }     //用户名

        [EnuValueValidator(RoleType.ADMINISTARTOR, RoleType.NORMAL)]
        public string Role { get; set; }         //角色Administrator, Normal
    }

    //修改用户信息（普通用户只能修改自己的信息）
    //PUT api/usersinfo/{username}
    public class UserInfo_API_Put : UserInfo_API_Base
    {
        [EnuValueValidator(RoleType.ADMINISTARTOR, RoleType.NORMAL)]
        public string Role { get; set; }         //角色Administrator, Normal, 普通用户无法修改此字段
    }
}
