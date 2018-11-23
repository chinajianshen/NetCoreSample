using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBook.Bee.Entity.AutoMapper
{
    #region 组1
    public class Source
    {
        public int SomeValue { get; set; }
        public int SomeValue2 { get; set; }
        public string AnotherValue { get; set; }

        public string Name { get; set; }
    }
    public class Destination
    {
        public int SomeValue { get; set; }

        public string AnotherValue { get; set; }

        public string Name2 { get; set; }

        public int Total { get; set; }
    }
    #endregion

    #region 组2
    public class Order
    {
        public Customer Customer { get; set; }

        public decimal GetTotal()
        {
            return 100M;
        }
    }

    public class Customer
    {
        public string Name { get; set; }
    }

    public class OrderDto
    {
        public string CustomerName { get; set; }
        public string Total { get; set; }
    }
    #endregion

    #region 组3
    public class CalendarEvent
    {
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public EventCalendar EventCalendar { get; set; }
        public Order Order { get; set; }
    }

    public class CalendarEventForm
    {
        public DateTime EventDate { get; set; }
        public int EventHour { get; set; }
        public int EventMinute { get; set; }
        public string DisplayTitle { get; set; }
        public EventCalendar EventCalendar { get; set; }
        public Order Order { get; set; }
        public string Name { get; set; }
    }

    public enum EventCalendar
    {
        //Default=0,
        First =1,
    }
    #endregion
}
