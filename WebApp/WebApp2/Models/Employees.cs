using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp2.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int Flag { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
    }

   
}
