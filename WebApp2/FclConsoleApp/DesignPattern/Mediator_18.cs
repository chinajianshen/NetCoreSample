using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /*
     引言
　　在现实生活中，有很多中介者模式的身影，例如QQ游戏平台，聊天室、QQ群和短信平台，这些都是中介者模式在现实生活中的应用，下面就具体分享下我对中介者模式的理解。

    中介者模式的定义
　　从生活中的例子可以看出，不论是QQ游戏还是QQ群，它们都是充当一个中间平台，QQ用户可以登录这个中间平台与其他QQ用户进行交流，如果没有这些中间平台，我们如果想与朋友进行聊天的话，可能就需要当面才可以了。电话、短信也同样是一个中间平台，有了这个中间平台，每个用户都不要直接依赖与其他用户，只需要依赖这个中间平台就可以了，一切操作都由中间平台去分发。了解完中介模式在生活中的模型后，下面给出中介模式的正式定义。
　　中介者模式，定义了一个中介对象来封装一系列对象之间的交互关系。中介者使各个对象之间不需要显式地相互引用，从而使耦合性降低，而且可以独立地改变它们之间的交互行为。

    中介者模式的结构
　　从生活中例子自然知道，中介者模式设计两个具体对象，一个是用户类，另一个是中介者类，根据针对接口编程原则，则需要把这两类角色进行抽象，所以中介者模式中就有了4类角色，它们分别是：抽象中介者角色，具体中介者角色、抽象同事类和具体同事类。中介者类是起到协调各个对象的作用，则抽象中介者角色中则需要保存各个对象的引用

    为什么要使用中介者模式
　　在现实生活中，中介者的存在是不可缺少的，如果没有了中介者，我们就不能与远方的朋友进行交流了。而在软件设计领域，为什么要使用中介者模式呢？如果不使用中介者模式的话，各个同事对象将会相互进行引用，如果每个对象都与多个对象进行交互时，将会形成如下图所示的网状结构。

        中介者模式的适用场景
 　　一般在以下情况下可以考虑使用中介者模式：
一组定义良好的对象，现在要进行复杂的相互通信。
想通过一个中间类来封装多个类中的行为，而又不想生成太多的子类。

        中介者模式的优缺点
　　中介者模式具有以下几点优点：
简化了对象之间的关系，将系统的各个对象之间的相互关系进行封装，将各个同事类解耦，使得系统变为松耦合。
提供系统的灵活性，使得各个同事对象独立而易于复用。
　　然而，中介者模式也存在对应的缺点：

中介者模式中，中介者角色承担了较多的责任，所以一旦这个中介者对象出现了问题，整个系统将会受到重大的影响。例如，QQ游戏中计算欢乐豆的程序出错了，这样会造成重大的影响。
新增加一个同事类时，不得不去修改抽象中介者类和具体中介者类，此时可以使用观察者模式和状态模式来解决这个问题。

         总结
　　中介者模式，定义了一个中介对象来封装系列对象之间的交互。中介者使各个对象不需要显式地相互引用，从而使其耦合性降低，而且可以独立地改变它们之间的交互。中介者模式一般应用于一组定义良好的对象之间需要进行通信的场合以及想定制一个分布在多个类中的行为，而又不想生成太多的子类的情形下。
    */

    /// <summary>
    /// 中介者模式
    /// https://www.cnblogs.com/zhili/p/MediatorPattern.html
    /// </summary>
    public class Mediator_18
    {
        /// <summary>
        /// 两人打牌
        /// </summary>
        public void Two_CardPartner()
        {
            ACardPartner A = new ParterA();
            A.MoneyCount = 20;

            ACardPartner B = new ParterB();
            B.MoneyCount = 20;

            // A 赢了则B的钱就减少
            A.ChangeCount(5, B);
            Console.WriteLine("A 现在的钱是：{0}", A.MoneyCount);// 应该是25
            Console.WriteLine("B 现在的钱是：{0}", B.MoneyCount); // 应该是15

            // B赢了A的钱也减少
            B.ChangeCount(10, A);
            Console.WriteLine("A 现在的钱是：{0}", A.MoneyCount); // 应该是15
            Console.WriteLine("B 现在的钱是：{0}", B.MoneyCount); // 应该是25
        }

        /// <summary>
        /// 中介者 两人打牌
        /// 抽象中介者类保存了两个抽象牌友类，如果新添加一个牌友类似时，此时就不得不去更改这个抽象中介者类。可以结合观察者模式来解决这个问题，即抽象中介者对象保存抽象牌友的类别，然后添加Register和UnRegister方法来对该列表进行管理，然后在具体中介者类中修改AWin和BWin方法，遍历列表，改变自己和其他牌友的钱数。这样的设计还是存在一个问题——即增加一个新牌友时，此时虽然解决了抽象中介者类不需要修改的问题，但此时还是不得不去修改具体中介者类，即添加CWin方法，我们可以采用状态模式来解决这个问题，关于状态模式的介绍将会在下一专题进行分享。
        /// </summary>
        public void Mediator_CardPartner()
        {
            AbstractCardPartner A = new ConcreteParterA();
            AbstractCardPartner B = new ConcreteParterB();

            //初始钱
            A.MoneyCount = 20;
            B.MoneyCount = 20;

            AbstractMediator mediator = new ConcreteMediatorPater(A, B);

            // A赢了
            A.ChangeCount(5, mediator);
            Console.WriteLine("A 现在的钱是：{0}", A.MoneyCount);// 应该是25
            Console.WriteLine("B 现在的钱是：{0}", B.MoneyCount); // 应该是15

            // B 赢了
            B.ChangeCount(10, mediator);
            Console.WriteLine("A 现在的钱是：{0}", A.MoneyCount);// 应该是15
            Console.WriteLine("B 现在的钱是：{0}", B.MoneyCount); // 应该是25
        }

        /// <summary>
        /// 中介者模式和状态者模式组合运用
        /// </summary>
        public void Mediator_State_CardPartner()
        {
            AStateCardPartner A = new StateParterA();
            AStateCardPartner B = new StateParterB();

            // 初始钱
            A.MoneyCount = 20;
            B.MoneyCount = 20;

            AStateMediator mediator = new MediatorPater(new InitState());

            // A,B玩家进入平台进行游戏
            mediator.Enter(A);
            mediator.Enter(B);

            // A赢了
            mediator.State = new AwinState(mediator);
            mediator.ChangeCount(5);

            Console.WriteLine("A 现在的钱是：{0}", A.MoneyCount);// 应该是25
            Console.WriteLine("B 现在的钱是：{0}", B.MoneyCount); // 应该是15

            // B 赢了
            mediator.State = new BwinSate(mediator);
            mediator.ChangeCount(10);
            Console.WriteLine("A 现在的钱是：{0}", A.MoneyCount);// 应该是15
            Console.WriteLine("B 现在的钱是：{0}", B.MoneyCount); // 应该是25
        }
    }

    #region 两人打牌 高耦合
    /// <summary>
    /// 抽象牌友类
    /// </summary>
    public abstract class ACardPartner
    {
        public int MoneyCount { get; set; }

        public ACardPartner()
        {
            MoneyCount = 0;
        }

        public abstract void ChangeCount(int count, ACardPartner other);
    }

    /// <summary>
    ///  牌友A类
    /// </summary>
    public class ParterA : ACardPartner
    {
        public override void ChangeCount(int count, ACardPartner other)
        {
            this.MoneyCount += count;
            other.MoneyCount -= count;
        }
    }
    /// <summary>
    ///  牌友B类
    /// </summary>
    public class ParterB : ACardPartner
    {
        public override void ChangeCount(int count, ACardPartner other)
        {
            this.MoneyCount += count;
            other.MoneyCount -= count;
        }
    }
    #endregion

    #region 两人打牌 中介者模式


    /// <summary>
    /// 抽象牌友类
    /// </summary>
    public abstract class AbstractCardPartner
    {
        public int MoneyCount { get; set; }

        public AbstractCardPartner()
        {
            this.MoneyCount = 0;
        }

        public abstract void ChangeCount(int count, AbstractMediator mediator);
    }

    /// <summary>
    /// 牌友A类
    /// </summary>
    public class ConcreteParterA : AbstractCardPartner
    {
        public override void ChangeCount(int count, AbstractMediator mediator)
        {
            mediator.AWin(count);
        }
    }

    /// <summary>
    /// 牌友B类
    /// </summary>
    public class ConcreteParterB : AbstractCardPartner
    {
        public override void ChangeCount(int count, AbstractMediator mediator)
        {
            mediator.AWin(count);
        }
    }

    /// <summary>
    /// 抽象中介者类
    /// </summary>
    public abstract class AbstractMediator
    {
        protected AbstractCardPartner A;
        protected AbstractCardPartner B;
        public AbstractMediator(AbstractCardPartner a, AbstractCardPartner b)
        {
            A = a;
            B = b;
        }

        public abstract void AWin(int count);
        public abstract void BWin(int count);
    }

    public class ConcreteMediatorPater : AbstractMediator
    {
        public ConcreteMediatorPater(AbstractCardPartner a, AbstractCardPartner b) : base(a, b)
        {
        }

        public override void AWin(int count)
        {
            A.MoneyCount += count;
            B.MoneyCount -= count;
        }

        public override void BWin(int count)
        {
            A.MoneyCount -= count;
            B.MoneyCount += count;
        }
    }


    #endregion

    #region 两人打牌 通过状态者模式优化中介者模式
    /// <summary>
    /// 抽象中介者类
    /// </summary>
    public abstract class AStateMediator
    {
        public List<AStateCardPartner> list = new List<AStateCardPartner>();

        public AState State { get; set; }

        public AStateMediator(AState state)
        {
            this.State = state;
        }

        public void Enter(AStateCardPartner partner)
        {
            list.Add(partner);
        }

        public void Exit(AStateCardPartner partner)
        {
            list.Remove(partner);
        }

        public void ChangeCount(int count)
        {
            State.ChangeCount(count);
        }
    }

    /// <summary>
    /// 具体中介者类
    /// </summary>
    public class MediatorPater : AStateMediator
    {
        public MediatorPater(AState state) : base(state)
        {
        }
    }

    /// <summary>
    /// 抽象牌友类
    /// </summary>
    public abstract class AStateCardPartner
    {
        public int MoneyCount { get; set; }

        public AStateCardPartner()
        {
            this.MoneyCount = 0;
        }

        public abstract void ChangeCount(int count, AStateMediator mediator);
    }

    /// <summary>
    /// 牌友A类
    /// </summary>
    public class StateParterA : AStateCardPartner
    {
        public override void ChangeCount(int count, AStateMediator mediator)
        {
            mediator.ChangeCount(count);
        }
    }

    /// <summary>
    ///  牌友B类
    /// </summary>
    public class StateParterB : AStateCardPartner
    {
        public override void ChangeCount(int count, AStateMediator mediator)
        {
            mediator.ChangeCount(count);
        }
    }

    /// <summary>
    /// 抽象状态类
    /// </summary>
    public abstract class AState
    {
        protected AStateMediator mediator;
        public abstract void ChangeCount(int count);
    }

    public class InitState : AState
    {
        public InitState()
        {
            Console.WriteLine("游戏才刚刚开始,暂时还有玩家胜出");
        }

        public override void ChangeCount(int count)
        {
            return;
        }
    }

    /// <summary>
    /// A赢状态
    /// </summary>
    public class AwinState : AState
    {
        public AwinState(AStateMediator mediator)
        {
            base.mediator = mediator;
        }

        public override void ChangeCount(int count)
        {
            foreach (AStateCardPartner p in mediator.list)
            {
                StateParterA a = p as StateParterA;
                if (a != null)
                {
                    a.MoneyCount += count;
                }
                else
                {
                    p.MoneyCount -= count;
                }
            }
        }
    }

    public class BwinSate : AState
    {
        public BwinSate(AStateMediator mediator)
        {
            base.mediator = mediator;
        }

        public override void ChangeCount(int count)
        {
            foreach (AStateCardPartner p in mediator.list)
            {
                StateParterB b = p as StateParterB;
                if (b != null)
                {
                    b.MoneyCount += count;
                }
                else
                {
                    p.MoneyCount -= count;
                }
            }
        }
    }
    #endregion

}
