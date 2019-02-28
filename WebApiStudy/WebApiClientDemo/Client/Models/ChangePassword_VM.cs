using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Client.Models
{
    public class ChangePassword_VM : ViewModelBase
    {
        [Required(ErrorMessage = CommLib.Verifier.ERRMSG_CANNOT_BE_NULL)]
        [RegularExpression(CommLib.Verifier.REG_EXP_PASSWORD, ErrorMessage = CommLib.Verifier.ERRMSG_REG_EXP_PASSWORD)]
        public string OldPwd
        {
            get { return GetValue(() => OldPwd); }
            set { SetValue(() => OldPwd, value); }
        }

        [Required(ErrorMessage = CommLib.Verifier.ERRMSG_CANNOT_BE_NULL)]
        [RegularExpression(CommLib.Verifier.REG_EXP_PASSWORD, ErrorMessage = CommLib.Verifier.ERRMSG_REG_EXP_PASSWORD)]
        public string NewPwd
        {
            get { return GetValue(() => NewPwd); }
            set { SetValue(() => NewPwd, value); }
        }

        [Required(ErrorMessage = CommLib.Verifier.ERRMSG_CANNOT_BE_NULL)]
        [RegularExpression(CommLib.Verifier.REG_EXP_PASSWORD, ErrorMessage = CommLib.Verifier.ERRMSG_REG_EXP_PASSWORD)]
        public string ConfirmPwd
        {
            get { return GetValue(() => ConfirmPwd); }
            set { SetValue(() => ConfirmPwd, value); }
        }

        public override string ModelValidate()
        {
            if (NewPwd != ConfirmPwd)
            {
                return "新密码和确认密码输入不相同，重新输入";
            }
            return null;
        }
    }
}
