using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.DesignPattern
{
    /*
    引言
在软件开发中，我们经常想要对一类对象添加不同的功能，例如要给手机添加贴膜，手机挂件，手机外壳等，如果此时利用继承来实现的话，就需要定义无数的类，
如StickerPhone（贴膜是手机类）、AccessoriesPhone（挂件手机类）等，这样就会导致 ”子类爆炸“问题，
为了解决这个问题，我们可以使用装饰者模式来动态地给一个对象添加额外的职责。

   定义
装饰者模式以对客户透明的方式动态地给一个对象附加上(嵌套)更多的责任，装饰者模式相比生成子类可以更灵活地增加功能。

      在装饰者模式中各个角色有：
1抽象构件（Phone）角色：给出一个抽象接口，以规范准备接受附加责任的对象。
2具体构件（AppPhone）角色：定义一个将要接收附加责任的类。
3装饰（Dicorator）角色：持有一个构件（Component）对象的实例，并定义一个与抽象构件接口一致的接口。 (装饰抽象构件一定要继承抽象构件)
4具体装饰（Sticker和Accessories）角色：负责给构件对象 ”贴上“附加的责任。

        装饰者模式的优缺点
优点：
1装饰这模式和继承的目的都是扩展对象的功能，但装饰者模式比继承更灵活
2通过使用不同的具体装饰类以及这些类的排列组合，设计师可以创造出很多不同行为的组合
3装饰者模式有很好地可扩展性
缺点：装饰者模式会导致设计中出现许多小对象，如果过度使用，会让程序变的更复杂。并且更多的对象会是的差错变得困难，特别是这些对象看上去都很像。

    .NET中装饰者模式的实现
 BufferedStream、CryptoStream和GZipStream其实就是两个具体装饰类，这里的装饰者模式省略了抽象装饰角色（Decorator）。下面演示下客户端如何动态地为MemoryStream动态增加功能的。

            MemoryStream memoryStream = new MemoryStream(new byte[] {95,96,97,98,99});
            // 扩展缓冲的功能
            BufferedStream buffStream = new BufferedStream(memoryStream);

            // 添加加密的功能
            CryptoStream cryptoStream = new CryptoStream(memoryStream,new AesManaged().CreateEncryptor(),CryptoStreamMode.Write);
            // 添加压缩功能
            GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true);

   总结
到这里，装饰者模式的介绍就结束了，装饰者模式采用对象组合而非继承的方式实现了再运行时动态地扩展对象功能的能力，而且可以根据需要扩展多个功能，避免了单独使用继承带来的 ”灵活性差“和”多子类衍生问题“。同时它很好地符合面向对象设计原则中 ”优先使用对象组合而非继承“和”开放-封闭“原则。
    */

    /// <summary>
    /// 装饰者模式
    /// https://www.cnblogs.com/zhili/p/DecoratorPattern.html
    /// </summary>
    public class Decorator_9
    {
        /// <summary>
        /// 以手机和手机配件的例子来演示装饰者模式的实现
        /// </summary>
        public void DecoratorPhone()
        {
            //我买了个苹果手机
            Phone phone = new ApplePhone();

            // 现在想贴膜了 (开始装饰了)
            Decorator applePhoneWithSticker = new PhoneSticker(phone); //想让手机贴膜，得给贴膜人，所以要把苹果手机传给贴膜人
            //扩展贴膜行为
            applePhoneWithSticker.Print();
            Console.WriteLine("----------------------\n");

            // 现在我想有挂件了
            Decorator applePhoneAccesstories = new PhoneAccessories(phone);
            applePhoneAccesstories.Print();
            Console.WriteLine("----------------------\n");

            // 现在我同时有贴膜和手机挂件了
            PhoneSticker sticker = new PhoneSticker(phone);
            PhoneAccessories accessory = new PhoneAccessories(sticker);
            accessory.Print();
            Console.WriteLine("----------------------\n");

            // 现在我同时有贴膜和手机挂件了 另一组合方法
            Decorator sticker2 = new PhoneSticker(phone);
            Decorator accessory2 = new PhoneAccessories(sticker2);
            accessory2.Print();
        }

        /// <summary>
        ///  手机抽象类，即装饰者模式中的抽象组件类
        /// </summary>
        public abstract class Phone
        {
            /// <summary>
            /// 执行功能
            /// </summary>
            public abstract void Print();
        }

        /// <summary>
        /// 苹果手机，即装饰着模式中的具体组件类
        /// </summary>
        public class ApplePhone : Phone
        {
            public override void Print()
            {
                Console.WriteLine("开始执行具体的对象——苹果手机");
            }
        }

        /// <summary>
        /// 装饰抽象类 要让装饰完全取代抽象组件，所以必须继承自抽象组件类（Phone）
        /// </summary>
        public abstract class Decorator : Phone
        {
            private Phone phone;
            public Decorator(Phone phone)
            {
                this.phone = phone;
            }

            /// <summary>
            /// 抽象装饰类实现一个基本方法  继续装饰抽象类具体重写并增加功能（一层一层包裹着
            /// ）
            /// </summary>
            public override void Print()
            {
               if (phone != null)
                {
                    phone.Print();
                }
            }
        }

        /// <summary>
        /// 手机贴膜 具体装饰者
        /// </summary>
        public class PhoneSticker : Decorator
        {
            public PhoneSticker(Phone phone) : base(phone) {
                int i = 0;
            }

            /// <summary>
            /// 具体装饰者重写 具体组件方法，用于在调用具体组件方法（不影响具体组件形为）基础上添加自己的特有形为
            /// </summary>
            public override void Print()
            {
                base.Print();

                //添加自己特有的新形为
                AddSticker();
            }

            private void AddSticker()
            {
                Console.WriteLine("现在苹果手机有贴膜了");
            }
        }

        /// <summary>
        /// 手机挂件 具体装饰者
        /// </summary>
        public class PhoneAccessories : Decorator
        {
            public PhoneAccessories(Phone phone) : base(phone)
            {
                int l = 1;
            }

            public override void Print()
            {
                base.Print();

                // 添加新的行为
                AddAccessories();
            }

            private void AddAccessories()
            {
                Console.WriteLine("现在苹果手机有漂亮的挂件了");
            }
        }
    }
}
