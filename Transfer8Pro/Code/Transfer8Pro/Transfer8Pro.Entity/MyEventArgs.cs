using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Entity
{
    public class MyEventArgs<T1> : EventArgs
    {
        public T1 Value1 { get; set; }
    }

    public class MyEventArgs<T1, T2> : MyEventArgs<T1>
    {
        public T2 Value2 { get; set; }
    }

    public class MyEventArgs<T1, T2, T3> : MyEventArgs<T1, T2>
    {
        public T3 Value3 { get; set; }
    }

    public class MyEventArgs<T1, T2, T3, T4> : MyEventArgs<T1, T2, T3>
    {
        public T4 Value4 { get; set; }
    }
}
