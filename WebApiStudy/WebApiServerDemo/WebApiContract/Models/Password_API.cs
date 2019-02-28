using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using CommLib;

namespace WebApiContract.Models
{
    public class Password_API_Put
    {
        // 这是经过加密的密码（需要解密后再验证）
        public string Password { get; set; }
    }
}
