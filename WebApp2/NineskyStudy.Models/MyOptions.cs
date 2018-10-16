using System;
using System.Collections.Generic;
using System.Text;

namespace NineskyStudy.Models
{
    public class MyOptions
    {
        public MyOptions()
        {
            Option1 = "Option1初始化值";
        }

        public string Option1 { get; set; }

        public int Option2 { get; set; } = 5;
    }

    public class MyOptionsWithDelegateConfig
    {
        public MyOptionsWithDelegateConfig()
        {
            Option1 = "Option1初始化值";
        }

        public string Option1 { get; set; }

        public int Option2 { get; set; } = 5;
    }
}
