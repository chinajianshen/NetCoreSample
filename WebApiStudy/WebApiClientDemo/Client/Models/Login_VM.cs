using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CommLib;

namespace Client.Models
{
    class Login_VM : ViewModelBase
    {
        [Required(ErrorMessage = Verifier.ERRMSG_CANNOT_BE_NULL)]
        [RegularExpression(Verifier.REG_EXP_USER_NAME, ErrorMessage = Verifier.ERRMSG_REG_EXP_USER_NAME)]
        public string UserName
        {
            get { return GetValue(() => UserName); }
            set { SetValue(() => UserName, value); }
        }

        [Required(ErrorMessage = Verifier.ERRMSG_CANNOT_BE_NULL)]
        [RegularExpression(Verifier.REG_EXP_PASSWORD, ErrorMessage = Verifier.ERRMSG_REG_EXP_PASSWORD)]
        public string Password
        {
            get { return GetValue(() => Password); }
            set { SetValue(() => Password, value); }
        }
    }
}
