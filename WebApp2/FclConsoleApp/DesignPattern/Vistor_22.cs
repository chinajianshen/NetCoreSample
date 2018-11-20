using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /// <summary>
    /// 访问者模式
    /// https://www.cnblogs.com/zhili/p/VistorPattern.html
    /// </summary>
    public class Vistor_22
    {
        /// <summary>
        /// 遍历每个元素对象，然后调用每个元素对象的Print方法来打印该元素对象的信
        /// </summary>
        public void ErgodicElement()
        {

        }
    }

    /// <summary>
    /// 抽象元素角色
    /// </summary>
    public abstract class Element
    {
        public abstract void Accept(IVistor vistor);
        public abstract void Print();
    }

    /// <summary>
    ///  具体元素A
    /// </summary>
    public class ElementA : Element
    {
        public override void Accept(IVistor vistor)
        {
            //调用访问者visit方法
            vistor.Visit(this);
        }

        public override void Print()
        {
            Console.WriteLine("我是元素A");
        }
    }

    /// <summary>
    ///  具体元素B
    /// </summary>
    public class ElementB : Element
    {
        public override void Accept(IVistor vistor)
        {
            vistor.Visit(this);
        }

        public override void Print()
        {
            Console.WriteLine("我是元素B");
        }
    }

    /// <summary>
    /// 抽象访问者
    /// </summary>
    public interface IVistor
    {
        void Visit(ElementA element);
        void Visit(ElementB element);
    }

    /// <summary>
    /// 具体访问者
    /// </summary>
    public class ConcreteVistor : IVistor
    {
        public void Visit(ElementA element)
        {
            element.Print();
        }

        public void Visit(ElementB element)
        {
            element.Print();
        }
    }
}
