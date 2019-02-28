using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Models;
using WebApiContract.Models;
using AutoMapper;

namespace Client
{
    class AutoMapperConfig
    {
        public static void ConfigAutoMappings()
        {
            Mapper.CreateMap<UserInfo_API_Get, UserInfo_VM>();
            Mapper.CreateMap<UserInfo_VM, UserInfo_API_Put>();
            Mapper.CreateMap<UserInfo_VM, UserInfo_API_Post>();


        }
    }
}
