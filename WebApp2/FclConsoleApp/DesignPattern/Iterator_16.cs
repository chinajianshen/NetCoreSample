using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /*
    迭代器模式的介绍
　　迭代器是针对集合对象而生的，对于集合对象而言，必然涉及到集合元素的添加删除操作，同时也肯定支持遍历集合元素的操作，我们此时可以把遍历操作也放在集合对象中，但这样的话，集合对象就承担太多的责任了，面向对象设计原则中有一条是单一职责原则，所以我们要尽可能地分离这些职责，用不同的类去承担不同的职责。迭代器模式就是用迭代器类来承担遍历集合元素的职责。 

     迭代器模式的定义
　　迭代器模式提供了一种方法顺序访问一个聚合对象（理解为集合对象）中各个元素，而又无需暴露该对象的内部表示，这样既可以做到不暴露集合的内部结构，又可让外部代码透明地访问集合内部的数据。
    
     迭代器模式的结构
　　既然，迭代器模式承担了遍历集合对象的职责，则该模式自然存在2个类，一个是聚合类，一个是迭代器类。在面向对象涉及原则中还有一条是针对接口编程，所以，在迭代器模式中，抽象了2个接口，一个是聚合接口，另一个是迭代器接口，这样迭代器模式中就四个角色了，具体的类图如下所示：
   　　从上图可以看出，迭代器模式由以下角色组成：
迭代器角色（Iterator）：迭代器角色负责定义访问和遍历元素的接口
具体迭代器角色（Concrete Iteraror）：具体迭代器角色实现了迭代器接口，并需要记录遍历中的当前位置。
聚合角色（Aggregate）：聚合角色负责定义获得迭代器角色的接口
具体聚合角色（Concrete Aggregate）：具体聚合角色实现聚合角色接口。

        .NET中迭代器模式的应用
　　在.NET下，迭代器模式中的聚集接口和迭代器接口都已经存在了，其中IEnumerator接口扮演的就是迭代器角色，IEnumberable接口则扮演的就是抽象聚集的角色，只有一个GetEnumerator()方法，关于这两个接口的定义可以自行参考MSDN
   
        迭代器模式的适用场景
　　在下面的情况下可以考虑使用迭代器模式：
1系统需要访问一个聚合对象的内容而无需暴露它的内部表示。
2系统需要支持对聚合对象的多种遍历。
3系统需要为不同的聚合结构提供一个统一的接口。

        迭代器模式的优缺点
　　由于迭代器承担了遍历集合的职责，从而有以下的优点：
1迭代器模式使得访问一个聚合对象的内容而无需暴露它的内部表示，即迭代抽象。
2迭代器模式为遍历不同的集合结构提供了一个统一的接口，从而支持同样的算法在不同的集合结构上进行操作
　　迭代器模式存在的缺陷：
迭代器模式在遍历的同时更改迭代器所在的集合结构会导致出现异常。所以使用foreach语句只能在对集合进行遍历，不能在遍历的同时更改集合中的元素。

        总结
　　到这里，本博文的内容就介绍结束了，迭代器模式就是抽象一个迭代器类来分离了集合对象的遍历行为，这样既可以做到不暴露集合的内部结构，又可让外部代码透明地访问集合内部的数据
     */

    /// <summary>
    /// 迭代器模式
    /// </summary>
    public class Iterator_16
    {
        public void IteratorClient()
        {
            Iterator iterator;
            IListCollection list = new ConcreteList();
            iterator = list.GetIterator();

            while (iterator.MoveNext())
            {
                int i = (int)iterator.GetCurrent();
                Console.WriteLine(i.ToString());
                iterator.Next();
            }
        }
    }

    /// <summary>
    /// 抽象聚合 接口
    /// </summary>
    public interface IListCollection
    {
        Iterator GetIterator();
    }   

    public class ConcreteList : IListCollection
    {
        int[] collection;

        public ConcreteList()
        {
            collection = new int[] { 2,4,6,8};
        }

        public Iterator GetIterator()
        {
            return new ConcreteIterator(this);
        }

        public int length
        {
            get
            {
                return collection.Length;
            }
        }

        public int GetElement(int index)
        {
            return collection[index];
        }

    }

    /// <summary>
    /// 迭代器接口
    /// </summary>
    public interface Iterator
    {
        bool MoveNext();
        Object GetCurrent();
        void Next();
        void Reset();
    }

    public class ConcreteIterator : Iterator
    {
        private ConcreteList _list;
        private int _index;

        public ConcreteIterator(ConcreteList list)
        {
            _list = list;
            _index = 0;
        }

        public object GetCurrent()
        {
            return _list.GetElement(_index);
        }

        public bool MoveNext()
        {
            if (_index < _list.length)
            {
                return true;
            }
            return false;
        }

        public void Next()
        {
            if (_index < _list.length)
            {
                _index++;
            }
        }

        public void Reset()
        {
            _index = 0;
        }
    }
}
