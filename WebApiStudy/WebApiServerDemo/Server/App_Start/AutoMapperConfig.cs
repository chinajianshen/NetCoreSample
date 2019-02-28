using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using BLL;
using WebApiContract.Models;

namespace Server
{
    public class AutoMapperConfig
    {
        public static void ConfigAutoMappings()
        {
            //例如：Mapper.CreateMap<OrderMgr.Order_BL, Models.Order_UI>();
            Mapper.CreateMap<UserInfo_BLL, UserInfo_API_Get>();
            Mapper.CreateMap<UserInfo_API_Put, UserInfo_BLL>();
            Mapper.CreateMap<UserInfo_API_Post, UserInfo_BLL>();
        }
    }
}
