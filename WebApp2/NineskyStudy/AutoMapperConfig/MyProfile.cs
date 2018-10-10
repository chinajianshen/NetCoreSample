using AutoMapper;
using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.AutoMapperConfig
{
    public class MyProfile:Profile,IProfile
    {
        public MyProfile()

        {
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel,Category>();
            //CreateMap<List<Category>, List<CategoryViewModel>>();
        }
    }
}
