using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /*
     引言
 　　在现实生活中，处处可见观察者模式，例如，微信中的订阅号，订阅博客和QQ微博中关注好友，这些都属于观察者模式的应用

        观察者模式的定义
　　从生活中的例子可以看出，只要对订阅号进行关注的客户端，如果订阅号有什么更新，就会直接推送给订阅了的用户。从中，我们就可以得出观察者模式的定义。
　　观察者模式定义了一种一对多的依赖关系，让多个观察者对象同时监听某一个主题对象，这个主题对象在状态发生变化时，会通知所有观察者对象，使它们能够自动更新自己的行为。

        观察者模式的结构
　　从上面观察者模式的定义和生活中的例子，很容易知道，观察者模式中首先会存在两个对象，一个是观察者对象，另一个就是主题对象，然而，根据面向接口编程的原则，则自然就有抽象主题角色和抽象观察者角色。理清楚了观察者模式中涉及的角色后，接下来就要理清他们之间的关联了，要想主题对象状态发生改变时，能通知到所有观察者角色，则自然主题角色必须所有观察者的引用，这样才能在自己状态改变时，通知到所有观察者。有了上面的分析，下面观察者的结构图也就很容易理解了

        可以看出，在观察者模式的结构图有以下角色：
抽象主题角色（Subject）：抽象主题把所有观察者对象的引用保存在一个列表中，并提供增加和删除观察者对象的操作，抽象主题角色又叫做抽象被观察者角色，一般由抽象类或接口实现。
抽象观察者角色（Observer）：为所有具体观察者定义一个接口，在得到主题通知时更新自己，一般由抽象类或接口实现。
具体主题角色（ConcreteSubject）：实现抽象主题接口，具体主题角色又叫做具体被观察者角色。
具体观察者角色（ConcreteObserver）：实现抽象观察者角色所要求的接口，以便使自身状态与主题的状态相协调。

        观察者模式的适用场景
 　　在下面的情况下可以考虑使用观察者模式：
1当一个抽象模型有两个方面，其中一个方面依赖于另一个方面，将这两者封装在独立的对象中以使它们可以各自独立地改变和复用的情况下。从方面的这个词中可以想到，观察者模式肯定在AOP（面向方面编程）中有所体现，更多内容参考：Observern Pattern in AOP.
2当对一个对象的改变需要同时改变其他对象，而又不知道具体有多少对象有待改变的情况下。
3当一个对象必须通知其他对象，而又不能假定其他对象是谁的情况下。

        观察者模式的优缺点
　　观察者模式有以下几个优点：
观察者模式实现了表示层和数据逻辑层的分离，并定义了稳定的更新消息传递机制，并抽象了更新接口，使得可以有各种各样不同的表示层，即观察者。
观察者模式在被观察者和观察者之间建立了一个抽象的耦合，被观察者并不知道任何一个具体的观察者，只是保存着抽象观察者的列表，每个具体观察者都符合一个抽象观察者的接口。
观察者模式支持广播通信。被观察者会向所有的注册过的观察者发出通知。
　　观察者也存在以下一些缺点：

如果一个被观察者有很多直接和间接的观察者时，将所有的观察者都通知到会花费很多时间。
虽然观察者模式可以随时使观察者知道所观察的对象发送了变化，但是观察者模式没有相应的机制使观察者知道所观察的对象是怎样发生变化的。
如果在被观察者之间有循环依赖的话，被观察者会触发它们之间进行循环调用，导致系统崩溃，在使用观察者模式应特别注意这点。

        总结
　　到这里，观察者模式的分享就介绍了。观察者模式定义了一种一对多的依赖关系，让多个观察者对象可以同时监听某一个主题对象，这个主题对象在发生状态变化时，会通知所有观察者对象，使它们能够自动更新自己，解决的是“当一个对象的改变需要同时改变多个其他对象”的问题。大家可以以微信订阅号的例子来理解观察者模式。
     */

    /// <summary>
    /// 观察者模式
    /// https://www.cnblogs.com/zhili/p/ObserverPattern.html
    /// </summary>
    public class Observer_17
    {
        /// <summary>
        /// 观察者演绎
        /// </summary>
        public void ObserverDeduceInit()
        {
            //实例化订阅者和订阅号对象
            Subscriber learningHardSub = new Subscriber("LearningHard");
            TenxunGame txGame = new TenxunGame();

            txGame.Subscriber = learningHardSub;
            txGame.Symbol = "TenXun Game";
            txGame.Info = "Have a new game published ....";

            txGame.Update();
        }

        /// <summary>
        /// 观察者结果
        /// </summary>
        public void ObserverDeduceResult()
        {
            TenXun tenxun = new TenXunGame2("TenXun Game", "Have a new game published ....");
            tenxun.AddObserver(new ConcreteObserver("Learning Hard"));
            tenxun.AddObserver(new ConcreteObserver("Tom"));

            tenxun.Update();
        }

        /// <summary>
        /// 观察者委托形式
        /// </summary>
        public void ObserverDeduceDelegate()
        {
            TenXunNet tenXunGameNet = new TenXunGameNet("TenXun Game", "Have a new game published ....");

            SubscriberNet lh = new SubscriberNet("Learning Hard");
            SubscriberNet tom = new SubscriberNet("Tom");

            // 添加订阅者
            tenXunGameNet.AddObserver(new NotifyEventHandler(lh.ReceiveAndPrint));
            tenXunGameNet.AddObserver(new NotifyEventHandler(tom.ReceiveAndPrint));
            tenXunGameNet.Update();

            Console.WriteLine("-----------------------------------");
            Console.WriteLine("移除Tom订阅者");
            tenXunGameNet.RemoveObserver(new NotifyEventHandler(tom.ReceiveAndPrint));
            tenXunGameNet.Update();
        }
    }

    #region 耦合高
    /// <summary>
    ///  腾讯游戏订阅号类
    /// </summary>
    public class TenxunGame
    {
        /// <summary>
        /// 订阅者对象
        /// </summary>
        public Subscriber Subscriber { get; set; }

        public string Symbol { get; set; }

        public string Info { get; set; }

        public void Update()
        {
            if (Subscriber != null)
            {
                // 调用订阅者对象来通知订阅者
                Subscriber.ReceiveAndPrintData(this);
            }
        }
    }

    /// <summary>
    /// 订阅号
    /// </summary>
    public class Subscriber
    {
        public string Name { get; set; }

        public Subscriber(string name)
        {
            Name = name;
        }

        public void ReceiveAndPrintData(TenxunGame txGame)
        {
            Console.WriteLine("Notified {0} of {1}'s" + " Info is: {2}", Name, txGame.Symbol, txGame.Info);
        }
    }
    #endregion

    #region 演绎后的

    public interface IObserver
    {
        void ReceiveAndPrint(TenXun tenxun);
    }

    public class ConcreteObserver : IObserver
    {
        public string Name { get; set; }

        public ConcreteObserver(string name)
        {
            this.Name = name;
        }

        public void ReceiveAndPrint(TenXun tenxun)
        {
            Console.WriteLine("Notified {0} of {1}'s" + " Info is: {2}", Name, tenxun.Symbol, tenxun.Info);
        }
    }

    public abstract class TenXun
    {
        private List<IObserver> observers = new List<IObserver>();
        public string Symbol { get; set; }
        public string Info { get; set; }

        public TenXun(string sysbol, string info)
        {
            Symbol = sysbol;
            Info = info;
        }

        public void AddObserver(IObserver ob)
        {
            observers.Add(ob);
        }

        public void RemoveObserver(IObserver ob)
        {
            observers.Remove(ob);
        }

        public void Update()
        {
            foreach (IObserver ob in observers)
            {
                if (ob != null)
                {
                    ob.ReceiveAndPrint(this);
                }
            }
        }
    }

    public class TenXunGame2 : TenXun
    {
        public TenXunGame2(string symbol, string info) : base(symbol, info)
        {

        }
    }
    #endregion

    #region 委托形式
    //委托充当订阅者接口类
    public delegate void NotifyEventHandler(object sender);

    public abstract class TenXunNet
    {
        public NotifyEventHandler NotifyEvent;
        public string Symbol { get; set; }
        public string Info { get; set; }

        public TenXunNet(string symbol, string info)
        {
            this.Symbol = symbol;
            this.Info = info;
        }

        public void Update()
        {
            if (NotifyEvent != null)
            {
                NotifyEvent(this);
            }
        }

        public void AddObserver(NotifyEventHandler ob)
        {
            NotifyEvent += ob;
        }

        public void RemoveObserver(NotifyEventHandler ob)
        {
            NotifyEvent -= ob;
        }
    }

    /// <summary>
    /// 具体订阅号类
    /// </summary>
    public class TenXunGameNet : TenXunNet
    {
        public TenXunGameNet(string symbol, string info) : base(symbol, info)
        {

        }
    }

    public class SubscriberNet
    {
        public string Name { get; set; }

        public SubscriberNet(string name)
        {
            this.Name = name;
        }

        public void ReceiveAndPrint(object obj)
        {
            TenXunGameNet tenxun = obj as TenXunGameNet;
            if (tenxun != null)
            {
                Console.WriteLine("Notified {0} of {1}'s" + " Info is: {2}", Name, tenxun.Symbol, tenxun.Info);
            }
        }
    }
    #endregion
}
