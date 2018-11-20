using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 命令模式的定义
     命令模式属于对象的行为型模式。命令模式是把一个操作或者行为抽象为一个对象中，通过对命令的抽象化来使得发出命令的责任和执行命令的责任分隔开。命令模式的实现可以提供命令的撤销和恢复功能。

    命令模式的定义
命令模式属于对象的行为型模式。命令模式是把一个操作或者行为抽象为一个对象中，通过对命令的抽象化来使得发出命令的责任和执行命令的责任分隔开。命令模式的实现可以提供命令的撤销和恢复功能。

    命令模式的优缺点
 　　命令模式使得命令发出的一个和接收的一方实现低耦合，从而有以下的优点：
1命令模式使得新的命令很容易被加入到系统里。
2可以设计一个命令队列来实现对请求的Undo和Redo操作。
3可以较容易地将命令写入日志。
4可以把命令对象聚合在一起，合成为合成命令。合成命令式合成模式的应用。
　　命令模式的缺点：
使用命令模式可能会导致系统有过多的具体命令类。这会使得命令模式在这样的系统里变得不实际。

    .NET中命令模式的应用(引用TerryLee)
 在ASP.NET的MVC模式中，有一种叫Front Controller的模式，它分为Handler和Command树两个部分，Handler处理所有公共的逻辑，接收HTTP Post或Get请求以及相关的参数并根据输入的参数选择正确的命令对象，然后将控制权传递到Command对象，由其完成后面的操作，这里面其实就是用到了Command模式。
 */

namespace FclConsoleApp.DesignPattern
{
    /// <summary>
    /// 命令模式
    /// http://www.cnblogs.com/zhili/p/CommandPattern.html
    /// </summary>
    public class Command_15
    {
        /// <summary>
        /// 校长发布学生跑步1000米命令
        /// </summary>
        public void LeaderCommand()
        {
            // 初始化Receiver、Invoke和Command
            Receiver r = new Concrete_Run1000_MeterReceiver();
            Command c = new ConcreteCommand(r);
            Invoke i = new Invoke(c);

            //院领导发出命令
            i.ExecuteCommand();
        }
    }

    /// <summary>
    /// 教官 负责调用命令对象执行命令
    /// </summary>
    public class Invoke
    {
        public Command _command;

        public Invoke(Command command)
        {
            this._command = command;
        }

        public void ExecuteCommand()
        {
            _command.Action();
        }

    }

    
    /// <summary>
    /// 命令抽象类
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        ///  命令应该知道接收者是谁，所以有Receiver这个成员变量
        /// </summary>
        protected Receiver _receiver;

        public Command(Receiver receiver)
        {
            this._receiver = receiver;
        }

        /// <summary>
        /// 命令执行方法
        /// </summary>
        public abstract void Action();
    }

    /// <summary>
    /// 命令实现类
    /// </summary>
    public class ConcreteCommand : Command
    {
        public ConcreteCommand(Receiver receiver) : base(receiver)
        {

        }

        public override void Action()
        {
            _receiver.Action();
        }
    }

    /// <summary>
    /// 命令接收者-学生
    /// </summary>
    public abstract class Receiver
    {
        public abstract void Action();
    }

    public class Concrete_Run1000_MeterReceiver : Receiver
    {
        public override void Action()
        {           
            Console.WriteLine("跑1000米");           
        }
    }
}
