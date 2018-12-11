using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity.AutoMapper
{
    public class EntityAutoMapperProfile : Profile
    {
        public EntityAutoMapperProfile()
        {            
            InitMapperEntities();
        }

        private void InitMapperEntities()
        {
            //Mapper.AssertConfigurationIsValid();
            //自定义解析器 opt.ResolveUsing<CustomResolver>();
            //自定义类型转换器 .ConvertUsing<CustomConverter>()

            #region 测试用例
            //属性名不一致时
            CreateMap<Source, Destination>()
                .ForMember(dest => dest.Name2, opt => { opt.MapFrom(s => s.Name); }); //opt.Ignore();忽略
            //来判断Destination类中的所有属性是否都被映射，如果存在未被映射的属性，则抛出异常          

            //默认源Customer.Name 可以映射 CustomerName  GetTotal()可以映射Total 
            CreateMap<Order, OrderDto>();

            //属性值可以分拆并映射
            CreateMap<CalendarEvent, CalendarEventForm>()
                 .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Date))
                 .ForMember(dest => dest.EventHour, opt => opt.MapFrom(src => src.Date.Hour))
                 .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.Date))
                 .ForMember(dest => dest.EventMinute, opt => opt.MapFrom(src => src.Date.Minute))
                 .ForMember(dest => dest.DisplayTitle, opt => opt.MapFrom(src => src.Title + src.Date.Minute.ToString()));
            #endregion

            //映射T8配置类到数据文件类
            CreateMap<T8ConfigEntity, T8FileEntity>();         
            CreateMap<T8ConfigItemEntity, T8FileEntity>();            
        }
    }


    #region 自定义方法
    //自定义解析器
    //public interface IValueResolver
    //{
    //    Source Resolve(Source source);
    //}

    //public class ValueResolver : IValueResolver
    //{
    //    public Source Resolve(Source source)
    //    {
    //        return source;
    //    }
    //}


    //自定义类型转换器
    //public class CustomConverter : ITypeConverter<Source, Destination>
    //{
    //    public Destination Convert(ResolutionContext context)
    //    {
    //        Source src = context.SourceValue as Source;
    //        Destination dest = new Destination();
    //        dest = System.Convert.ToInt32(src.Value1);

    //        return dest;
    //    }
    //}
    #endregion
}
