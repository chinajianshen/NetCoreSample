using System;
using System.Collections.Generic;
using System.Text;

namespace NineskyStudy.Models
{
   public class Paging<T> where T:class
    {
        public Paging()
        {
            PageIndex = 1;
            PageSize = 20;
        }
        public List<T> Entities { get; set; }

        public int Total { get; set; }

        public int  PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
