using System;
using System.Collections.Generic;
using System.Text;

namespace NineskyStudy.Base
{
    public delegate string TestDelegate(string value);
    public class TestClass
    {
        private string _value;

        public string CustomeField;

        public TestClass(string value)
        {
            _value = value;
        }

        public TestClass()
        {

        }

        [CustomTestAttribue(CustomID = 25)]
        public string GetTestValue()
        {
            return _value;
        }

        public string GetValue(string prefix)
        {
            if (_value == null)
                return "NULL";
            else
                return prefix + "  :  " + _value;
        }
        public string GetValue2(string prefix)
        {
            return prefix;
        }

        public string Value
        {
            set
            {
                _value = value;
            }
            get
            {
                if (_value == null)
                    return "NULL";
                else
                    return _value;
            }
        }
    }

    public class CustomTestAttribue : Attribute
    {
        public CustomTestAttribue()
        {

        }

        public CustomTestAttribue(int cid)
        {
            CustomID = cid;
        }

        public int CustomID { get; set; }
    }

    public class MyGeneric<T>
    {
        public string GetName(T name)
        {
            return "Generic Name：" + name.ToString();
        }

        public string GetName2<T1>(T name,T1 name2)
        {
            return "Generic Name:" + name.ToString() + " T1:" + name2;
        }
    }

    public class Class1<T>
    {
        public void Test(T t)
        {
            Console.WriteLine(t);
        }
    }
}
