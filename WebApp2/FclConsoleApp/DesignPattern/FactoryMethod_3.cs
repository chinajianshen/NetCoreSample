using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 在工厂方法模式中，工厂类与具体产品类具有平行的等级结构，它们之间是一一对应的。针对UML图的解释如下：
Creator类：充当抽象工厂角色，任何具体工厂都必须继承该抽象类
TomatoScrambledEggsFactory和ShreddedPorkWithPotatoesFactory类：充当具体工厂角色，用来创建具体产品
Food类：充当抽象产品角色，具体产品的抽象类。任何具体产品都应该继承该类
TomatoScrambledEggs和ShreddedPorkWithPotatoes类：充当具体产品角色，实现抽象产品类对定义的抽象方法，由具体工厂类创建，它们之间有一一对应的关系。
 */

namespace FclConsoleApp.DesignPattern
{
    /// <summary>
    /// 工厂方法模式
    /// http://www.cnblogs.com/zhili/p/FactoryMethod.html
    /// </summary>
    public class FactoryMethod_3
    {
        /*
         　工厂方法模式之所以可以解决简单工厂的模式，是因为它的实现把具体产品的创建推迟到子类中，此时工厂类不再负责所有产品的创建，
          而只是给出具体工厂必须实现的接口，这样工厂方法模式就可以允许系统不修改工厂类逻辑的情况下来添加新产品，这样也就克服了简单工厂模式中缺点。
          下面看下工厂模式的具体实现代码（这里还是以简单工厂模式中点菜的例子来实现
        */
        public void ClientOrderDishes()
        {
            Console.WriteLine("工厂方法");

            // 初始化做菜的两个工厂（）
            AbstractCreator shreddedPorkWithPotatoesFactory = new ShreddedPorkWithPotatoesFactory();
            AbstractCreator tomatoScrambledEggsFactory = new TomatoScrambledEggsFactory();

            // 开始做西红柿炒蛋
            Food tomatoScrambleEggs = tomatoScrambledEggsFactory.CreateFoodFactory();
            tomatoScrambleEggs.Print();

            //开始做土豆肉丝
            Food shreddedPorkWithPotatoes = shreddedPorkWithPotatoesFactory.CreateFoodFactory();
            shreddedPorkWithPotatoes.Print();
        }
    }

    /// <summary>
    /// 抽象工厂类
    /// </summary>
    public abstract class AbstractCreator
    {
        /// <summary>
        /// 工厂方法
        /// </summary>
        /// <returns></returns>
        public abstract Food CreateFoodFactory();
    }

    public class TomatoScrambledEggsFactory : AbstractCreator
    {
        /// <summary>
        /// 负责创建西红柿炒蛋这道菜
        /// </summary>
        /// <returns></returns>
        public override Food CreateFoodFactory()
        {
            return new TomatoScrambledEggs();
        }
    }

    /// <summary>
    /// 土豆肉丝工厂类
    /// </summary>
    class ShreddedPorkWithPotatoesFactory : AbstractCreator
    {
        /// <summary>
        /// 负责创建土豆肉丝这道菜
        /// </summary>
        /// <returns></returns>
        public override Food CreateFoodFactory()
        {
            return new ShreddedPorkWithPotatoes();
        }
    }
}
