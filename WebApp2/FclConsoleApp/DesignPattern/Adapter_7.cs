using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 在实际的开发过程中，由于应用环境的变化（例如使用语言的变化），我们需要的实现在新的环境中没有现存对象可以满足，但是其他环境却存在这样现存的对象。那么如果将“将现存的对象”在新的环境中进行调用呢？
 解决这个问题的办法就是我们本文要介绍的适配器模式——使得新环境中不需要去重复实现已经存在了的实现而很好地把现有对象（指原来环境中的现有对象）加入到新环境来使用。

 定义:适配器模式——把一个类的接口变换成客户端所期待的另一种接口，从而使原本接口不匹配而无法一起工作的两个类能够在一起工作。
   适配器模式有 类的适配器模式 和 对象的适配器模式 两种形式，
 */

namespace FclConsoleApp.DesignPattern
{
    /// <summary>
    /// 适配器模式
    /// 
    /// </summary>
    public class Adapter_7
    {
        /// <summary>
        /// 类适配器例子
        /// </summary>
        public void ClassAdapterSample(){
            // 现在客户端可以通过电适配要使用2个孔的插头了
            IThreeHole threeHole = new PowerClassAdapter();
            threeHole.Request();
        }

        /// <summary>
        /// 对象适配器
        /// </summary>
        public void ObjectAdapterSample()
        {
            // 现在客户端可以通过电适配要使用2个孔的插头了
            ThreeHoleObject threehole = new PowerObjectAdapter();
            threehole.Request();
        }
    }


    #region 类适配器
    /// <summary>
    /// 适配器类，接口要放在类的后面
    /// 适配器类提供了三个孔插头的行为，但其本质是调用两个孔插头的方法
    /// </summary>
    public class PowerClassAdapter : TwoHole, IThreeHole
    {
        /// <summary>
        /// 实现三个孔插头接口方法
        /// </summary>
        public void Request()
        {
            // 调用两个孔插头方法
            base.SpecificRequest();
        }
    }

    /// <summary>
    /// 三个孔的插头，也就是适配器模式中的目标角色
    /// </summary>
    public interface IThreeHole
    {
        void Request();
    }

    public abstract class TwoHole
    {
        /// <summary>
        /// 特殊请求
        /// </summary>
        public void SpecificRequest()
        {
            Console.WriteLine("我是两个孔的插头");
        }
    }
    #endregion

    #region 对象适配器
    /// <summary>
    /// 对象适配器类，这里适配器类没有TwoHole类，
    /// 而是引用了TwoHole对象，所以是对象的适配器模式的实现
    /// </summary>
    public class PowerObjectAdapter : ThreeHoleObject
    {
        /*
         既然现在适配器类不能继承TwoHole抽象类了（因为用继承就属于类的适配器了），但是适配器类无论如何都要实现客户端期待的方法的，即Request方法，所以一定是要继承ThreeHole抽象类或IThreeHole接口的，然而适配器类的Request方法又必须调用TwoHole的SpecificRequest方法，又不能用继承，这时候就想，不能继承，但是我们可以在适配器类中创建TwoHole对象，然后在Requst中使用TwoHole的方法了。
        */
        // 引用两个孔插头的实例,从而将客户端与TwoHole联系起来
        public TwoHoleObject twoholeAdaptee = new TwoHoleObject();

        /// <summary>
        /// 实现三个孔插头接口方法
        /// </summary>
        public override void Request()
        {
            twoholeAdaptee.SpecificRequest();
        }
    }

    public class ThreeHoleObject
    {
        /// <summary>
        /// 客户端需要的方法
        /// </summary>
        public virtual void Request()
        {
            // 可以把一般实现放在这里
        }
    }

    /// <summary>
    /// 两个孔的插头，源角色——需要适配的类
    /// </summary>
    public class TwoHoleObject
    {
        public void SpecificRequest()
        {
            Console.WriteLine("我是两个孔的插头");
        }
    }
    #endregion

}