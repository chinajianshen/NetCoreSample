using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /*
     说到简单工厂，自然的第一个疑问当然就是什么是简单工厂模式了？ 在现实生活中工厂是负责生产产品的,同样在设计模式中,简单工厂模式我们也可以理解为负责生产对象的一个类, 我们平常编程中，当使用"new"关键字创建一个对象时，此时该类就依赖与这个对象，也就是他们之间的耦合度高，当需求变化时，我们就不得不去修改此类的源码，此时我们可以运用面向对象（OO）的很重要的原则去解决这一的问题，该原则就是——封装改变，既然要封装改变，自然也就要找到改变的代码，然后把改变的代码用类来封装，这样的一种思路也就是我们简单工厂模式的实现方式了。下面通过一个现实生活中的例子来引出简单工厂模式。
     */

    /// <summary>
    /// 简单工厂模式
    /// </summary>
    public class SimpleFactory_2
    {
        /// <summary>
        /// 客户点餐
        /// </summary>
        public void CustomerOrderDishes()
        {

            Console.WriteLine("简单工厂");
            // 客户想点一个西红柿炒蛋
            Food food1 = FoodSimpleFactory.CreateFood("西红柿炒蛋");
            food1.Print();

            // 客户想点一个土豆肉丝
            Food food2 = FoodSimpleFactory.CreateFood("土豆肉丝");
            food2.Print();
        }
    }
  

    /// <summary>
    /// 自己做饭的情况
    /// 没有简单工厂之前，客户想吃什么菜只能自己炒的 
    /// 
    /// </summary>
     class Customer1
    {
        /// <summary>
        /// 点餐
        /// </summary>
        public void OrderDishes()
        {
            Food food1 = Cook("西红柿炒蛋");
            food1.Print();

            Food food2 = Cook("土豆肉丝");
            food2.Print();

        }       

        public  Food Cook(string type)
        {
            Food food = null;
            // 客户A说：我想吃西红柿炒蛋怎么办？
            // 客户B说：那你就自己烧啊
            // 客户A说： 好吧，那就自己做吧
            if (type.Equals("西红柿炒蛋"))
            {
                food = new TomatoScrambledEggs();
            }
            // 我又想吃土豆肉丝, 这个还是得自己做
            // 我觉得自己做好累哦，如果能有人帮我做就好了？
            else if (type.Equals("土豆肉丝"))
            {
                food = new ShreddedPorkWithPotatoes();
            }
            return food;
        }
       
    }

    /// <summary>
    /// 简单工厂类, 负责 炒菜
    /// </summary>
     class FoodSimpleFactory
    {
        public static Food CreateFood(string type)
        {
            Food food=null;
            if (type.Equals("土豆肉丝"))
            {
                food = new ShreddedPorkWithPotatoes();
            }
            else if (type.Equals("西红柿炒蛋"))
            {
                food = new TomatoScrambledEggs();
            }

            return food;
        }
    }

    // <summary>
    /// 菜抽象类
    /// </summary>
    public abstract class Food
    {
        /// <summary>
        /// 输出点了什么菜
        /// </summary>
        public abstract void Print();
    }

    public class TomatoScrambledEggs : Food
    {
        public override void Print()
        {
            Console.WriteLine("一份西红柿炒蛋！");
        }
    }
    /// <summary>
    /// 土豆肉丝这道菜
    /// </summary>
     class ShreddedPorkWithPotatoes : Food
    {
        public override  void Print()
        {
            Console.WriteLine("一份土豆肉丝");
        }
    }

}
