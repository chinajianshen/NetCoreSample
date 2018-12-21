using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Transfer8Pro.Entity
{
    public class KVEntity
    {
        public string K { get; set; }
        public string V { get; set; }
    }

    public class KVEntity<D> : KVEntity
    {
        public D T1 { get; set; }
    }

    public class KVEntity<D1, D2> : KVEntity<D1>
    {
        public D2 T2 { get; set; }
    }

    public class KVEntity<D1, D2,D3> : KVEntity<D1,D2>
    {
        public D3 T3 { get; set; }
    }
        
    public class KVEntity<D1, D2, D3,D4> : KVEntity<D1, D2,D3>
    {
        public D4 T4 { get; set; }
    }

}
