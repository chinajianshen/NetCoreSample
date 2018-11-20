using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /*
     引言
　　在上一篇文章介绍到可以使用状态者模式和观察者模式来解决中介者模式存在的问题，在本文中将首先通过一个银行账户的例子来解释状态者模式，通过这个例子使大家可以对状态者模式有一个清楚的认识，接着，再使用状态者模式来解决上一篇文章中提出的问题。

    状态者模式的介绍
　　每个对象都有其对应的状态，而每个状态又对应一些相应的行为，如果某个对象有多个状态时，那么就会对应很多的行为。那么对这些状态的判断和根据状态完成的行为，就会导致多重条件语句，并且如果添加一种新的状态时，需要更改之前现有的代码。这样的设计显然违背了开闭原则。状态模式正是用来解决这样的问题的。状态模式将每种状态对应的行为抽象出来成为单独新的对象，这样状态的变化不再依赖于对象内部的行为。

    状态者模式的定义
　　上面对状态模式做了一个简单的介绍，这里给出状态模式的定义。
　　状态模式——允许一个对象在其内部状态改变时自动改变其行为，对象看起来就像是改变了它的类。

    状态者模式的结构
　　既然状态者模式是对已有对象的状态进行抽象，则自然就有抽象状态者类和具体状态者类，而原来已有对象需要保存抽象状态者类的引用，通过调用抽象状态者的行为来改变已有对象的行为。经过上面的分析，状态者模式的结构图也就很容易理解了
    状态者模式涉及以下三个角色：
Account类：维护一个State类的一个实例，该实例标识着当前对象的状态。
State类：抽象状态类，定义了一个具体状态类需要实现的行为约定。
SilveStater、GoldState和RedState类：具体状态类，实现抽象状态类的每个行为。
    
    */

    /// <summary>
    /// 状态者模式
    /// https://www.cnblogs.com/zhili/p/StatePattern.html
    /// </summary>
    public class State_19
    {
        /// <summary>
        /// 银行账户的状态来实现下状态者模式
        /// 。银行账户根据余额可分为RedState、SilverState和GoldState。这些状态分别代表透支账号，新开账户和标准账户。账号余额在【-100.0，0.0】范围表示处于RedState状态，账号余额在【0.0 ， 1000.0】范围表示处于SilverState，账号在【1000.0， 100000.0】范围表示处于GoldState状态。
        /// </summary>
        public void BankAccountState()
        {
            //开一个新的账户
            Account account = new Account("zhang san");

            //进行交易
            //存钱 
            account.Deposit(1000.0);
            account.Deposit(200.00);
            account.Deposit(600.00);

            //付利息
            account.PayInterest();

            //取钱
            account.Withdraw(2000.00);
            account.Withdraw(500.00);
        }
    }

    /// <summary>
    /// 账户类
    /// </summary>
    public class Account
    {
        public State State { get; set; }

        public string Owner { get; set; }

        public Account(string owner)
        {
            this.Owner = owner;
            this.State = new SilverState(0.0, this);
        }

        public double Balance { get { return State.Balance; } }

        /// <summary>
        /// 存钱
        /// </summary>
        /// <param name="amount"></param>
        public void Deposit(double amount)
        {
            State.Deposit(amount);
            Console.WriteLine("存款金额为 {0:C}——", amount);
            Console.WriteLine("账户余额为 =:{0:C}", this.Balance);
            Console.WriteLine("账户状态为: {0}", this.State.GetType().Name);
            Console.WriteLine();
        }

        /// <summary>
        /// 取钱
        /// </summary>
        /// <param name="amount"></param>
        public void Withdraw(double amount)
        {
            State.Withdraw(amount);
            Console.WriteLine("取款金额为 {0:C}——", amount);
            Console.WriteLine("账户余额为 =:{0:C}", this.Balance);
            Console.WriteLine("账户状态为: {0}", this.State.GetType().Name);
            Console.WriteLine();
        }

        /// <summary>
        /// 获得利息
        /// </summary>
        public void PayInterest()
        {
            State.PayInterest();
            Console.WriteLine("Interest Paid --- ");
            Console.WriteLine("账户余额为 =:{0:C}", this.Balance);
            Console.WriteLine("账户状态为: {0}", this.State.GetType().Name);
            Console.WriteLine();
        }
    }

    /// <summary>
    ///  抽象状态类
    /// </summary>
    public abstract class State
    {
        public Account Account { get; set; }

        /// <summary>
        /// 余额
        /// </summary>
        public double Balance { get; set; }

        //利率
        public double Interest { get; set; }

        //下限
        public double LowerLimit { get; set; }

        //上限
        public double UpperLimit { get; set; }

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="amount"></param>
        public abstract void Deposit(double amount);

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="amount"></param>
        public abstract void Withdraw(double amount);

        /// <summary>
        /// 获得的利息
        /// </summary>
        public abstract void PayInterest();
    }

    /// <summary>
    /// 透支状态
    /// </summary>
    public class RedState : State
    {
        public RedState(State state)
        {
            this.Balance = state.Balance;
            this.Account = state.Account;
            this.Interest = 0.00;
            LowerLimit = -100.00;
            UpperLimit = 0.00;
        }

        public override void Deposit(double amount)
        {
            this.Balance += amount;
            StateChangeCheck();
        }

        public override void PayInterest()
        {
            Console.WriteLine("没有利息！");
        }

        public override void Withdraw(double amount)
        {
            Console.WriteLine("没有钱可以取了！");
        }

        private void StateChangeCheck()
        {
            if (this.Balance > this.UpperLimit)
            {
                base.Account.State = new SilverState(this);
            }
        }
    }

    /// <summary>
    /// 新开账户状态
    /// </summary>
    public class SilverState : State
    {
        public SilverState(State state) : this(state.Balance, state.Account)
        {

        }

        public SilverState(double balance, Account account)
        {
            this.Balance = balance;
            this.Account = account;
            this.Interest = 0.00;
            LowerLimit = 0.00;
            UpperLimit = 1000.00;
        }

        public override void Deposit(double amount)
        {
            this.Balance += amount;
            StateChangeCheck();
        }

        public override void PayInterest()
        {
            this.Balance += this.Interest * Balance;
            StateChangeCheck();
        }

        public override void Withdraw(double amount)
        {
            this.Balance -= amount;
            StateChangeCheck();

        }

        private void StateChangeCheck()
        {
            if (Balance < LowerLimit)
            {
                Account.State = new RedState(this);
            }
            else if (Balance > UpperLimit)
            {
                Account.State = new GoldState(this);
            }
        }
    }

    /// <summary>
    /// 标准账户状态
    /// </summary>
    public class GoldState : State
    {
        public GoldState(State state)
        {
            this.Balance = state.Balance;
            this.Account = state.Account;
            Interest = 0.05;
            LowerLimit = 1000.00;
            UpperLimit = 1000000.00;
        }

        public override void Deposit(double amount)
        {
            this.Balance += amount;
            StateChangeCheck();
        }

        public override void PayInterest()
        {
            this.Balance += this.Balance * this.Interest;
            StateChangeCheck();
        }

        public override void Withdraw(double amount)
        {
            this.Balance -= amount;
            StateChangeCheck();
        }

        private void StateChangeCheck()
        {
            if (Balance < 0.0)
            {
                Account.State = new RedState(this);
            }
            else if (Balance < LowerLimit)
            {
                Account.State = new SilverState(this);
            }
        }
    }
}
