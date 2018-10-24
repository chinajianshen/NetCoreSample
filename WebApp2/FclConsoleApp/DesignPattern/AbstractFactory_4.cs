using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 知道了抽象工厂的优缺点之后，也就能很好地把握什么情况下考虑使用抽象工厂模式了，下面就具体看看使用抽象工厂模式的系统应该符合那几个前提：
1一个系统不要求依赖产品类实例如何被创建、组合和表达的表达，这点也是所有工厂模式应用的前提。
2这个系统有多个系列产品，而系统中只消费其中某一系列产品
3系统要求提供一个产品类的库，所有产品以同样的接口出现，客户端不需要依赖具体实现。
*/

namespace FclConsoleApp.DesignPattern
{
    /*
     工厂方法模式是为了克服简单工厂模式的缺点而设计出来的,简单工厂模式的工厂类随着产品类的增加需要增加额外的代码），而工厂方法模式每个具体工厂类只完成单个实例的创建,所以它具有很好的可扩展性。但是在现实生活中，一个工厂只创建单个产品这样的例子很少，因为现在的工厂都多元化了，一个工厂创建一系列的产品，如果我们要设计这样的系统时，工厂方法模式显然在这里不适用，然后抽象工厂模式却可以很好地解决一系列产品创建的问题,这是本专题所要介绍的内容。
    */

    /// <summary>
    /// 抽象工厂模式
    /// http://www.cnblogs.com/zhili/p/AbstractFactory.html
    /// </summary>
    public class AbstractFactory_4
    {
        /*
         下面就以生活中 “绝味” 连锁店的例子来实现一个抽象工厂模式。例如，绝味鸭脖想在江西南昌和上海开分店，但是由于当地人的口味不一样，在南昌的所有绝味的东西会做的辣一点，而上海不喜欢吃辣的，所以上海的所有绝味的东西都不会做的像南昌的那样辣，然而这点不同导致南昌绝味工厂和上海的绝味工厂生成所有绝味的产品都不同，也就是某个具体工厂需要负责一系列产品(指的是绝味所有食物)的创建工作，下面就具体看看如何使用抽象工厂模式来实现这种情况。

            抽象工厂模式很难支持新种类产品的变化。这是因为抽象工厂接口中已经确定了可以被创建的产品集合，如果需要添加新产品，此时就必须去修改抽象工厂的接口，这样就涉及到抽象工厂类的以及所有子类的改变，这样也就违背了“开发——封闭”原则。
        */

        /// <summary>
        /// ，抽象工厂对于系列产品的变化支持 “开放——封闭”原则（指的是要求系统对扩展开放，对修改封闭），扩展起来非常简便，但是，抽象工厂对于添加新产品这种情况就不支持”开放——封闭 “原则，这也是抽象工厂的缺点所在
        /// </summary>
        public void AastracetFactoryClient()
        {
            // 南昌工厂制作南昌的鸭脖和鸭架
            JiuJiuYaAbstractFactory nanChangFactory = new NanChangFactory();
            YaBo nanChangYaBo = nanChangFactory.CreateYaBo();
            nanChangYaBo.Print();
            YaJia nanChangYaJia = nanChangFactory.CreateYaJia();
            nanChangYaJia.Print();

            // 上海工厂制作南昌的鸭脖和鸭架
            JiuJiuYaAbstractFactory shangHaiFactory = new ShangHaiFactory();
            YaBo shangHaiYaBo = shangHaiFactory.CreateYaBo();
            shangHaiYaBo.Print();
            YaJia shangHaiYaJia = shangHaiFactory.CreateYaJia();
            shangHaiYaJia.Print();
        }
    }

    #region  抽象工厂类，提供创建两个不同地方的鸭架和鸭脖的接口
    public abstract class JiuJiuYaAbstractFactory
    {
        // 抽象工厂提供创建一系列产品的接口，这里作为例子，只给出了绝味中鸭脖和鸭架的创建接口
        public abstract YaBo CreateYaBo();
        public abstract YaJia CreateYaJia();
    }
    #endregion

    #region  南昌工厂实现抽象工厂
    public class NanChangFactory : JiuJiuYaAbstractFactory
    {
        // 制作南昌鸭脖
        public override YaBo CreateYaBo()
        {
            return new NanChangYaBo();
        }

        // 制作南昌鸭架
        public override YaJia CreateYaJia()
        {
            return new NanChangYaJia();
        }
    }
    #endregion

    #region  上海工厂实现抽象工厂
    public class ShangHaiFactory : JiuJiuYaAbstractFactory
    {
        public override YaBo CreateYaBo()
        {
            return new ShangHaiYaBo();
        }

        public override YaJia CreateYaJia()
        {
            return new ShangHaiYaJia();
        }
    }
    #endregion

    #region 鸭脖和鸭架抽象类
    /// <summary>
    /// 鸭脖抽象类，供每个地方的鸭脖类继承
    /// </summary>
    public abstract class YaBo
    {
        /// <summary>
        /// 打印方法，用于输出信息
        /// </summary>
        public abstract void Print();
    }
    /// <summary>
    /// 鸭架抽象类，供每个地方的鸭架类继承
    /// </summary>
    public abstract class YaJia
    {
        /// <summary>
        /// 打印方法，用于输出信息
        /// </summary>
        public abstract void Print();
    }
    #endregion

    #region 南昌鸭脖和鸭架实现抽象类 （做的较辣点）
    /// <summary>
    /// 南昌的鸭脖类，因为江西人喜欢吃辣的，所以南昌的鸭脖稍微会比上海做的辣
    /// </summary>
    public class NanChangYaBo : YaBo
    {
        public override void Print()
        {
            Console.WriteLine("南昌的鸭脖比较辣");
        }
    }

    public class NanChangYaJia : YaJia
    {
        public override void Print()
        {
            Console.WriteLine("南昌的鸭架比较辣");
        }
    }
    #endregion

    #region  上海鸭脖和鸭架实现抽象类 （做的不辣）
    // <summary>
    /// 上海的鸭脖没有南昌的鸭脖做的辣
    /// </summary>
    public class ShangHaiYaBo : YaBo
    {
        public override void Print()
        {
            Console.WriteLine("上海的鸭脖不辣");
        }
    }

    public class ShangHaiYaJia : YaJia
    {
        public override void Print()
        {
            Console.WriteLine("上海的鸭架不辣");
        }
    }
    #endregion

}
