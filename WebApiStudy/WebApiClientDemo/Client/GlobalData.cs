using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    static class GlobalData
    {
        //public static string s_strServiceAddress = "http://localhost.:57955/api/";
        public static string s_strServiceAddress = "http://localhost:57955/api/";

        public static string GetResUri(string strResName)
        {
            return s_strServiceAddress + strResName;
        }

        //当前用户，登录时候赋值
        public static string CurrentUserName { get; set; }

        //当前用户显示名称
        public static string CurrentDispName { get; set; }

        //当前用户角色名称
        public static string CurrentRole { get; set; }
    }
}
