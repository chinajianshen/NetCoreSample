using AutoMapper;
using OpenBook.Bee.Entity.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Test
{
    public class MapperTest
    {
        /// <summary>
        /// 直接使用
        /// </summary>
        public void Test1()
        {
            Mapper.Initialize(config => config.CreateMap<Source, Destination>()
            .ForMember(dto => dto.Name2, opt => //属性名不一致时
            {
                opt.MapFrom(s => s.Name);
            }));
            Source src = new Source() { SomeValue = 1, AnotherValue = "2", Name = "张三" };
            Destination dest = Mapper.Map<Destination>(src);

        }

        /// <summary>
        /// 调用配置文件
        /// </summary>
        public void Test2()
        {
            Source src = new Source() { SomeValue = 1, AnotherValue = "2", Name = "张三" };
            Destination dest = Mapper.Map<Destination>(src);
        }

        public void Test3()
        {
            Customer customer = new Customer() { Name = "Tom" };
            Order order = new Order() { Customer = customer };
            OrderDto orderDto = Mapper.Map<OrderDto>(order);

            CalendarEvent calendarEvent = new CalendarEvent()
            {
                Date = DateTime.Now,
                Title = "Demo Event",
                Order = order,
                EventCalendar = EventCalendar.First
            };
            CalendarEventForm calendarEventForm = Mapper.Map<CalendarEventForm>(calendarEvent);
            int i = (int)calendarEventForm.EventCalendar;

        }


    }
}
