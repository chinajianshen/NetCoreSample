using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp
{
    /// <summary>
    /// http://www.cnblogs.com/kissdodog/archive/2013/03/26/2983755.html
    /// </summary>
    public class ThreadParamterStudy
    {
        public void Process()
        {
           
            for (int i = 0; i < 5; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(Run));
                t.Start(i);
            }
            Console.WriteLine("主线程执行完毕！");
            Thread.Sleep(3000);

            //自定义类型作为参数
            Person p1 = new Person("关羽", 22);
            Person p2 = new Person("张飞", 21);
            Thread t1 = new Thread(new ParameterizedThreadStart(RunP));
            t1.Start(p1);

            Thread t2 = new Thread(new ParameterizedThreadStart(RunP));
            t2.Start(p2);
        }

        void Run(object i)
        {
            Thread.Sleep(50);
            Console.WriteLine("线程传进来的参数是：" + i.ToString());
        }
        void RunP(object o)
        {
            Thread.Sleep(50);
            Person p = o as Person;
            Console.WriteLine(p.Name + p.Age);
        }
    }

    class Person
    {
        public Person(string name, int age) { this.Name = name; this.Age = age; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
