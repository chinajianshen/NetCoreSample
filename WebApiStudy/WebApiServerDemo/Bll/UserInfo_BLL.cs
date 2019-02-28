using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class UserInfo_BLL
    {
        public string UserName { get; set; }     //用户名
        public string RealName { get; set; }     //真实姓名
        public float Height { get; set; }        //身高
        public DateTime Birthday { get; set; }   //生日
        public string Role { get; set; }         //角色Administrator, Normal

        public long UpdateTicks { get; set; }    //时间戳
    }
}
