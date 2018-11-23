using AutoMapper;
using OpenBook.Bee.Entity.AutoMapper;
using OpenBook.Bee.Test.AutoMapperTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Test
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            EntityAutoMapperProfile autoMapperProfile = new EntityAutoMapperProfile();
            Mapper.Initialize(x => x.AddProfile<EntityAutoMapperProfile>());
        }
    }
}
