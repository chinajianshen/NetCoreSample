using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
 外观和适配器比较：
 适配器模式是将一个对象包装起来以改变其接口，而外观是将一群对象 ”包装“起来以简化其接口，它们的意图是不一样的，适配器是将接口转换为不同接口，而外观模式是提供一个统一的接口来简化接口。
 
 
 引言
在软件开发过程中，客户端程序经常会与复杂系统的内部子系统进行耦合，从而导致客户端程序随着子系统的变化而变化，
然而为了将复杂系统的内部子系统与客户端之间的依赖解耦，从而就有了外观模式，也称作 ”门面“模式。

定义
外观模式提供了一个统一的接口，用来访问子系统中的一群接口。外观定义了一个高层接口，让子系统更容易使用。使用外观模式时，我们创建了一个统一的类，用来包装子系统中一个或多个复杂的类，客户端可以直接通过外观类来调用内部子系统中方法，从而外观模式让客户和子系统之间避免了紧耦合。

    有两个角色：
门面（Facade）角色：客户端调用这个角色的方法。该角色知道相关的一个或多个子系统的功能和责任，该角色会将从客户端发来的请求委派带相应的子系统中去。
子系统（subsystem）角色：可以同时包含一个或多个子系统。每个子系统都不是一个单独的类，而是一个类的集合。每个子系统都可以被客户端直接调用或被门面角色调用。对于子系统而言，门面仅仅是另外一个客户端，子系统并不知道门面的存在。

外观的优缺点
优点：

外观模式对客户屏蔽了子系统组件，从而简化了接口，减少了客户处理的对象数目并使子系统的使用更加简单。
外观模式实现了子系统与客户之间的松耦合关系，而子系统内部的功能组件是紧耦合的。松耦合使得子系统的组件变化不会影响到它的客户。
缺点：

如果增加新的子系统可能需要修改外观类或客户端的源代码，这样就违背了”开——闭原则“（不过这点也是不可避免）。
四、使用场景
 在以下情况下可以考虑使用外观模式：

外一个复杂的子系统提供一个简单的接口
提供子系统的独立性
在层次化结构中，可以使用外观模式定义系统中每一层的入口。其中三层架构就是这样的一个例子。

    总结
到这里外观模式的介绍就结束了，外观模式，为子系统的一组接口提供一个统一的接口，该模式定义了一个高层接口，这一个高层接口使的子系统更加容易使用。并且外观模式可以解决层结构分离、降低系统耦合度和为新旧系统交互提供接口功能。
 */

namespace FclConsoleApp.DesignPattern
{
    /// <summary>
    /// 外观模式 平时开发可以运用
    /// 
    /// https://www.cnblogs.com/zhili/p/FacadePattern.html
    /// </summary>
    public class Facade_11
    {
        /// <summary>
        /// 学生选课系统用外观模式（平时开发中客户端大部分复杂点功能都可以用外观模式）
        /// 下面与学校中一个选课系统为例来解释外观模式，例如在选课系统中，有注册课程子系统和通知子系统，在不使用外观模式的情况下，客户端必须同时保存注册课程子系统和通知子系统两个引用，如果后期这两个子系统发生改变时，此时客户端的调用代码也要随之改变，这样就没有很好的可扩展性
        /// 用了外观模式之后，客户端只依赖与外观类，从而将客户端与子系统的依赖解耦了，如果子系统发生改变，此时客户端的代码并不需要去改变。外观模式的实现核心主要是——由外观类去保存各个子系统的引用，实现由一个统一的外观类去包装多个子系统类，然而客户端只需要引用这个外观类，然后由外观类来调用各个子系统中的方法
        /// </summary>
        public void StudySelectCourseSystem()
        {
            RegistrationFacade facade = new RegistrationFacade();
            if (facade.RgeisterCourse("设计模式", "同学1"))
            {
                Console.WriteLine("选课成功");
            }
            else
            {
                Console.WriteLine("选课失败");
            }

            Console.WriteLine("--------------------------------\n");
            if (facade.RgeisterCourse("设计模式", "同学2"))
            {
                Console.WriteLine("选课成功");
            }
            else
            {
                Console.WriteLine("选课失败");
            }

            Console.WriteLine("--------------------------------\n");
            if (facade.RgeisterCourse("设计模式", "同学3"))
            {
                Console.WriteLine("选课成功");
            }
            else
            {
                Console.WriteLine("选课失败");
            }
        }
    }

    /// <summary>
    /// 学生选课外观类
    /// </summary>
    public class RegistrationFacade
    {
        private RegisterCourse registerCourse;
        private NotifyStudent notifyStudent;

        public RegistrationFacade()
        {
            registerCourse = new RegisterCourse();
            notifyStudent = new NotifyStudent();
        }

        public bool RgeisterCourse(string courseName,string studentName)
        {
            if (!registerCourse.CheckAvailable(courseName))
            {
                return false;
            }
            return notifyStudent.Notify(studentName);
        }
    }

    #region 子系统
    /// <summary>
    /// 子系统A 选课子系统
    /// </summary>
    public class RegisterCourse {
        //课程可选人次
        private static int maxPersons = 2;
        private static int currProsons = 0;

        public bool CheckAvailable(string courseName)
        {
            Console.WriteLine("正在验证课程 {0}是否人数已满", courseName);

            if (currProsons <= maxPersons)
            {
                Interlocked.Decrement(ref maxPersons);
                Interlocked.Increment(ref currProsons);
                return true;
            }    
            else
            {
                return false;
            }           
        }
    }

   
    /// <summary>
    /// 子系统B 通知选课是否成功
    /// </summary>
    public class NotifyStudent
    {
        public bool Notify(string studentName)
        {
            Console.WriteLine("正在向{0}发生通知", studentName);
            return true;
        }
    }
    #endregion
}
