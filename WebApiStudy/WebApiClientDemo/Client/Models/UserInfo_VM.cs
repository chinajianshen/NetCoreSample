using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CommLib;

namespace Client.Models
{
    class UserInfo_VM : ViewModelBase
    {
        public string UserName
        {
            get { return GetValue(() => UserName); }
            set { SetValue(() => UserName, value); }
        }

        //真实姓名
        [Required(ErrorMessage = Verifier.ERRMSG_CANNOT_BE_NULL)]
        [RegularExpression(Verifier.REG_EXP_CHINESE_NAME, ErrorMessage = Verifier.ERRMSG_REG_EXP_CHINESE_NAME)]
        public string RealName
        {
            get { return GetValue(() => RealName); }
            set { SetValue(() => RealName, value); }
        }

        //身高
        public float Height
        {
            get { return GetValue(() => Height); }
            set { SetValue(() => Height, value); }
        }

        //生日
        public DateTime Birthday
        {
            get { return GetValue(() => Birthday); }
            set { SetValue(() => Birthday, value); }
        }

        //角色
        public string Role
        {
            get { return GetValue(() => Role); }
            set { SetValue(() => Role, value); }
        }

        //时间戳，从服务器端获取到的，不会发生变化，不需要GetValue/SetValue这种方式去实现通知
        public long UpdateTicks { get; set; }

        //查看/增加/编辑
        public EnuModelType ModelType
        {
            get { return GetValue(() => ModelType); }
            set { SetValue(() => ModelType, value); }
        }
    }
}
