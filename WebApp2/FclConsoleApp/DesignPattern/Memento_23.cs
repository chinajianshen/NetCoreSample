using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /// <summary>
    /// 备忘录模式
    /// https://www.cnblogs.com/zhili/p/MementoPattern.html
    /// </summary>
    public class Memento_23
    {
        /*
         一、引言
 　　在上一篇博文分享了访问者模式，访问者模式的实现是把作用于某种数据结构上的操作封装到访问者中，使得操作和数据结构隔离。而今天要介绍的备忘者模式与命令模式有点相似，不同的是，命令模式保存的是发起人的具体命令（命令对应的是行为），而备忘录模式保存的是发起人的状态（而状态对应的数据结构，如属性）。下面具体来看看备忘录模式。
   二、备忘录模式介绍
2.1 备忘录模式的定义
 　　从字面意思就可以明白，备忘录模式就是对某个类的状态进行保存下来，等到需要恢复的时候，可以从备忘录中进行恢复。生活中这样的例子经常看到，如备忘电话通讯录，备份操作操作系统，备份数据库等。

　　备忘录模式的具体定义是：在不破坏封装的前提下，捕获一个对象的内部状态，并在该对象之外保存这个状态，这样以后就可以把该对象恢复到原先的状态。

2.2 备忘录模式的结构图
 　　介绍完备忘录模式的定义之后，下面具体看看备忘录模式的结构图：
   备忘录模式中主要有三类角色：

发起人角色：记录当前时刻的内部状态，负责创建和恢复备忘录数据。
备忘录角色：负责存储发起人对象的内部状态，在进行恢复时提供给发起人需要的状态。
管理者角色：负责保存备忘录对象。
三、备忘录模式的适用场景
 　　在以下情况下可以考虑使用备忘录模式：

如果系统需要提供回滚操作时，使用备忘录模式非常合适。例如文本编辑器的Ctrl+Z撤销操作的实现，数据库中事务操作。
四、备忘录模式的优缺点
 　　备忘录模式具有以下优点：

如果某个操作错误地破坏了数据的完整性，此时可以使用备忘录模式将数据恢复成原来正确的数据。
备份的状态数据保存在发起人角色之外，这样发起人就不需要对各个备份的状态进行管理。而是由备忘录角色进行管理，而备忘录角色又是由管理者角色管理，符合单一职责原则。
　　当然，备忘录模式也存在一定的缺点：

在实际的系统中，可能需要维护多个备份，需要额外的资源，这样对资源的消耗比较严重。
五、总结
　　备忘录模式主要思想是——利用备忘录对象来对保存发起人的内部状态，当发起人需要恢复原来状态时，再从备忘录对象中进行获取，在实际开发过程也应用到这点，例如数据库中的事务处理。
         */

        /// <summary>
        /// 备忘录 备份手机联系人
        /// </summary>
        public void BackupMobileContact()
        {
            List<ContactPerson> persons = new List<ContactPerson>()
            {
                new ContactPerson() { Name= "Learning Hard", MobileNum = "123445"},
                new ContactPerson() { Name = "Tony", MobileNum = "234565"},
                new ContactPerson() { Name = "Jock", MobileNum = "231455"}
            };

            MobileOwner mobileOwner = new MobileOwner(persons);
            mobileOwner.Show();

            // 创建备忘录并保存备忘录对象
            Caretaker caretaker = new Caretaker();
            caretaker.ContactMementoDic[DateTime.Now.ToString()] = mobileOwner.CreateMemento();

            // 更改发起人联系人列表
            Console.WriteLine("----移除最后一个联系人--------");
            mobileOwner.ContactPersons.RemoveAt(2);
            mobileOwner.Show();

            //创建第二个备份
            Thread.Sleep(1000);
            caretaker.ContactMementoDic.Add(DateTime.Now.ToString(), mobileOwner.CreateMemento());

            //恢复到原始状态
            Console.WriteLine("-------恢复联系人列表,请从以下列表选择恢复的日期------");
            var keyCollection = caretaker.ContactMementoDic.Keys;
            foreach (string k in keyCollection)
            {
                Console.WriteLine("Key={0}", k);
            }

            int counter = 0;
            Console.WriteLine();
            while (true)
            {
                if (counter>2)
                {
                    break;
                }
                Console.Write("请输入数字,按窗口的关闭键退出:");
                int index = -1;
                try
                {
                    index = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("输入的格式错误");
                    continue;
                }

                ContactMemento contactMemento = null;
                if (index < keyCollection.Count && caretaker.ContactMementoDic.TryGetValue(keyCollection.ElementAt(index), out contactMemento))
                {
                    mobileOwner.RestoreMemento(contactMemento);
                    mobileOwner.Show();
                }
                else
                {
                    Console.WriteLine("输入的索引大于集合长度！");
                }

                counter++;
            }
        }
    }

    /// <summary>
    ///  联系人类
    /// </summary>
    public class ContactPerson
    {
        public string Name { get; set; }

        public string MobileNum { get; set; }
    }

    /// <summary>
    /// 备忘录类
    /// </summary>
    public class ContactMemento
    {
        public List<ContactPerson> contactPersonBack;

        public ContactMemento(List<ContactPerson> persons)
        {
            this.contactPersonBack = persons;
        }
    }

    /// <summary>
    ///  管理角色
    /// </summary>
    public class Caretaker
    {
        //使用多个备忘录来存储多个备份点
        public Dictionary<string, ContactMemento> ContactMementoDic { get; set; }

        public Caretaker()
        {
            ContactMementoDic = new Dictionary<string, ContactMemento>();
        }
       
    }

    /// <summary>
    /// 发起人
    /// </summary>
    public class MobileOwner
    {
        // 发起人需要保存的内部状态
        public List<ContactPerson> ContactPersons { get; set; }

        public MobileOwner(List<ContactPerson> persons)
        {
            this.ContactPersons = persons;
        }

        /// <summary>
        /// 创建备忘录，将当期要保存的联系人列表导入到备忘录中 
        /// </summary>
        /// <returns></returns>
        public ContactMemento CreateMemento()
        {
            return new ContactMemento(new List<ContactPerson>(this.ContactPersons));
        }

        /// <summary>
        ///  将备忘录中的数据备份导入到联系人列表中
        /// </summary>
        /// <param name="memento"></param>
        public void RestoreMemento(ContactMemento memento)
        {
            this.ContactPersons = memento.contactPersonBack;
        }

        public void Show()
        {
            Console.WriteLine("联系人列表中有{0}个人，他们是:", ContactPersons.Count);
            foreach (ContactPerson p in ContactPersons)
            {
                Console.WriteLine("姓名: {0} 号码为: {1}", p.Name, p.MobileNum);
            }
        }
    }
}
