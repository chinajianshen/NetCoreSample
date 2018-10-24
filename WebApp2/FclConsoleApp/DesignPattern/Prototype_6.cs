using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 引言
在软件系统中，当创建一个类的实例的过程很昂贵或很复杂，并且我们需要创建多个这样类的实例时，如果我们用new操作符去创建这样的类实例，这未免会增加创建类的复杂度和耗费更多的内存空间，因为这样在内存中分配了多个一样的类实例对象，然后如果采用工厂模式来创建这样的系统的话，随着产品类的不断增加，导致子类的数量不断增多，反而增加了系统复杂程度，所以在这里使用工厂模式来封装类创建过程并不合适，然而原型模式可以很好地解决这个问题，因为每个类实例都是相同的，当我们需要多个相同的类实例时，没必要每次都使用new运算符去创建相同的类实例对象，此时我们一般思路就是想——只创建一个类实例对象，如果后面需要更多这样的实例，可以通过对原来对象拷贝一份来完成创建，这样在内存中不需要创建多个相同的类实例，从而减少内存的消耗和达到类实例的复用。 然而这个思路正是原型模式的实现方式。下面就具体介绍下设计模式中的原型设计模式。

    原型模式的优点有：
1原型模式向客户隐藏了创建新实例的复杂性
2原型模式允许动态增加或较少产品类。
3原型模式简化了实例的创建结构，工厂方法模式需要有一个与产品类等级结构相同的等级结构，而原型模式不需要这样。
4产品类不需要事先确定产品的等级结构，因为原型模式适用于任何的等级结构

     原型模式的缺点有：
1每个类必须配备一个克隆方法
2配备克隆方法需要对类的功能进行通盘考虑，这对于全新的类不是很难，但对于已有的类不一定很容易，特别当一个类引用不支持串行化的间接对象，或者引用含有循环结构的时候。

 总结
 原型模式用一个原型对象来指明所要创建的对象类型，然后用复制这个原型对象的方法来创建出更多的同类型对象，它与工厂方法模式的实现非常相似，其中原型模式中的Clone方法就类似工厂方法模式中的工厂方法，只是工厂方法模式的工厂方法是通过new运算符重新创建一个新的对象（相当于原型模式的深拷贝实现），而原型模式是通过调用MemberwiseClone方法来对原来对象进行拷贝，也就是复制，同时在原型模式优点中也介绍了与工厂方法的区别（第三点）。
 */
namespace FclConsoleApp.DesignPattern
{
    /// <summary>
    /// 原型模式
    /// 在现实生活中，也有很多原型设计模式的例子，例如，细胞分裂的过程，一个细胞的有丝分裂产生两个相同的细胞；还有西游记中孙悟空变出后孙的本领和火影忍者中鸣人的隐分身忍术等。
    /// 下面就以孙悟空为例子来演示下原型模式的实现
    /// </summary>
    public class Prototype_6
    {
        /// <summary>
        /// 
        /// </summary>
        public void MonkeyKingPrototype()
        {
            //孙悟空 原型
            MonkeyKingPortotype monkeyKingPortotype = new ConcreteMonkeyKingPrototyp("MonkeyKing");

            // 变一个
            MonkeyKingPortotype monkeyKingPortotype2 = monkeyKingPortotype.Clone() as ConcreteMonkeyKingPrototyp;
            Console.WriteLine("Cloned1:\t" + monkeyKingPortotype2.Id);

            // 变两个
            MonkeyKingPortotype cloneMonkeyKing2 = monkeyKingPortotype.Clone() as ConcreteMonkeyKingPrototyp;
            Console.WriteLine("Cloned2:\t" + cloneMonkeyKing2.Id);
        }
    }

    /// <summary>
    /// 孙悟空原型抽象类
    /// </summary>
    public abstract class MonkeyKingPortotype
    {
        public string Id { get; set; }

        public MonkeyKingPortotype(string id)
        {
            this.Id = id;
        }

        public virtual void Method()
        {

        }

        /// <summary>
        /// 克隆方法，即孙大圣说“变”
        /// </summary>
        /// <returns></returns>
        public abstract MonkeyKingPortotype Clone();      
    }

    public class ConcreteMonkeyKingPrototyp : MonkeyKingPortotype
    {
        public ConcreteMonkeyKingPrototyp(string id) : base(id) {
            string a = base.Id + "a";
        }

        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public override MonkeyKingPortotype Clone()
        {
            return (MonkeyKingPortotype)(this.MemberwiseClone());
        }
    }
}
