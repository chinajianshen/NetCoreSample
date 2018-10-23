using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FclConsoleApp.MultiThread.ParallelThread
{
    /// <summary>
    /// 并行集合
    /// 并行算法的出现，随之而产生的也就有了并行集合，及线程安全集合；微软向的也算周到，没有忘记linq，也推出了linq的并行版本，plinq - Parallel Linq.
    /// http://www.cnblogs.com/yunfeifei/p/3998783.html
    /// </summary>
    public class ParallelEnumerableStudy
    {
        /// <summary>
        /// 普通List集 并行计算 出现安全数据问题
        /// 因为List<T>是非线程安全集合，意思就是说所有的线程都可以修改他的值
        /// </summary>
        public void ListWithParallel_UnSafety()
        {
            List<int> list = new List<int>(); //经测试有时会不正确有时少于10000条
            //List<string> list = new List<string>(); //经测试字符串会不正确有时少于10000条
            Parallel.For(0, 10000, item =>
              {
                  list.Add(item);
                  //list.Add(item.ToString());
              });
            Console.WriteLine($"List's count is {list.Count}");
        }

        /// <summary>
        /// ConcurrentBag中的数据并不是按照顺序排列的，顺序是乱的，随机的。我们平时使用的Max、First、Last等linq方法都还有
        /// </summary>
        public void ConcurrentBagWithPalle()
        {         
            ConcurrentBag<int> list = new ConcurrentBag<int>();
            //list.TryPeek();
            //list.TryTake();
            Parallel.For(0, 10000, item =>
              {
                  list.Add(item);
              });
            Console.WriteLine("ConcurrentBag's count is {0}", list.Count());
        }

        /// <summary>
        /// 并行队列 ConcurrentQueue 是完全无锁的，能够支持并发的添加元素，先进先出
        /// </summary>
        public void ConcurrentQueueSample()
        {
            ConcurrentQueue<Custom> customQueue = new ConcurrentQueue<Custom>();
            customQueue.Enqueue(new Custom { Name="1",Age=21 }); //添加到结尾
            customQueue.Enqueue(new Custom { Name = "2", Age = 22 }); //添加到结尾
            customQueue.Enqueue(new Custom { Name = "3", Age = 23 }); //添加到结尾

            Custom custom = new Custom();
            customQueue.TryPeek(out custom);//尝尝试返回队列开头处的对象但不将其移除。
            customQueue.TryDequeue(out custom);// 尝试移除并返回并发队列开头处的对象
        }

        /// <summary>
        /// 并列堆栈 是完全无锁的，能够支持并发的添加元素，后进先出
        /// </summary>
        public void ConcurrentStackSample()
        {
            ConcurrentStack<Custom> customStack = new ConcurrentStack<Custom>();
            customStack.Push(new Custom { Name = "1", Age = 21 }); //顶部插入一个元素
            customStack.PushRange((new List<Custom>() { new Custom { Name = "2", Age = 22 }, new Custom { Name = "3", Age = 23 } }).ToArray());//顶部插入多个元素
            customStack.Push(new Custom { Name = "4", Age = 24 });

            Custom custom = new Custom();
            customStack.TryPeek(out custom);//从顶部返回一个对象而无需移除它

            customStack.TryPop(out custom);//从顶部返回一个对象而并移除它

            Custom[] customArray = new Custom[2];
            customStack.TryPopRange(customArray); //从顶部返回2个对象
        }

        /// <summary>
        /// 对于读操作是完全无锁的，当很多线程要修改数据时，它会使用细粒度的锁。
        /// https://blog.csdn.net/wangzhiyu1980/article/details/45497907
        /// </summary>
        public void ConcurrentDictionaryWithPalle()
        {
            /*
AddOrUpdate：如果键不存在，方法会在容器中添加新的键和值，如果存在，则更新现有的键和值。
GetOrAdd：如果键不存在，方法会向容器中添加新的键和值，如果存在则返回现有的值，并不添加新值。
TryAdd：尝试在容器中添加新的键和值。
TryGetValue：尝试根据指定的键获得值。
TryRemove：尝试删除指定的键。
TryUpdate：有条件的更新当前键所对应的值。
GetEnumerator：返回一个能够遍历整个容器的枚举器。
             */
            ConcurrentDictionary<int, UserEntity> dic = new ConcurrentDictionary<int, UserEntity>();
            bool b1 = dic.TryAdd(1, new UserEntity { UserID = 1, UserName = "11" }); //添加
            bool b11 = dic.TryAdd(1, new UserEntity { UserID = 1, UserName = "11" }); //重键添加失败 不报异常
            bool b2 = dic.TryAdd(2, new UserEntity { UserID = 2, UserName = "22" }); 
            bool b3 = dic.TryAdd(3, new UserEntity { UserID = 3, UserName = "33" });
            UserEntity getUser = dic.GetOrAdd(6, new UserEntity { UserID=6, UserName="66" }); //如果该键不存在，则将键/值对添加到字典中；如果该键已经存在，则通过使用指定的函数更新
            UserEntity getUser2 = dic.GetOrAdd(7, (k) => { return new UserEntity { UserID = 7 , UserName = "77", Flag=k  }; });
            UserEntity getUser3 = dic.GetOrAdd(7, (k) => { return new UserEntity { UserID = 77, UserName = "7777", Flag = k }; });

            UserEntity userEntity = new UserEntity();                     
            bool b5 = dic.TryGetValue(1, out userEntity);// 尝试从字典 获取与指定的键关联的值 
            bool b4 = dic.TryRemove(1, out userEntity);//尝试从字典 中移除并返回具有指定键的值 

            //如果键不存在，方法会在容器中添加新的键和值，如果存在，则更新现有的键和值。
            //ConcurrentDictionary<string, int> allc = new ConcurrentDictionary<string, int>();
            //allc.TryAdd("duoduo", 2);
            //allc.AddOrUpdate("duoduo", 1, (x, y) => 1);

            UserEntity userEntity2 = new UserEntity() { UserID=8, UserName="88"};
            UserEntity ue = dic.AddOrUpdate(8, userEntity2, (key, oldValue) => userEntity2);
            UserEntity userEntity3 = new UserEntity() { UserID = 88, UserName = "8888" };
            UserEntity ue2 =  dic.AddOrUpdate(8, userEntity3, (key, oldValue) => userEntity3);

            //没用对
            //bool b7= dic.TryUpdate(3, new UserEntity { UserID = 3, UserName = "33" }, new UserEntity { UserID = 3, UserName = "我是33" } );
           

        }

        /// <summary>
        /// 与经典的阻塞队列数据结构类似，能够适用于多个任务添加和删除数据，提供阻塞和限界能力。
        /// </summary>
        public void BlockingCollectionSample()
        {
            BlockingCollection<UserEntity> blockList = new BlockingCollection<UserEntity>();
            blockList.Add(new UserEntity { UserID = 1, UserName = "11" });
            blockList.Add(new UserEntity { UserID = 2, UserName = "22" });

            bool status0 = blockList.TryAdd(new UserEntity { UserID = 2, UserName = "22" });
            bool status = blockList.TryAdd(new UserEntity { UserID = 3, UserName = "33" });
            var takes =  blockList.Take(2); //从序列开头返回连续项

            UserEntity userEntity = new UserEntity();
            bool status2 = blockList.TryTake(out userEntity);

        }

        /// <summary>
        /// 并行Linq的AsParallel方法 （测试大部分时间并行Linq还没有普通的快）
        /// 时间相差了一倍，不过有时候不会相差这么多，要看系统当前的资源利用率。
        /// </summary>
        public void AsParallelPLinq()
        {
            Stopwatch sw = new Stopwatch();
            List<Custom> customs = new List<Custom>();
            for (int i = 0; i < 2000000; i++)
            {
                customs.Add(new Custom() { Name = "Jack", Age = 21, Address = "NewYork" });
                customs.Add(new Custom() { Name = "Jime", Age = 26, Address = "China" });
                customs.Add(new Custom() { Name = "Tina", Age = 29, Address = "ShangHai" });
                customs.Add(new Custom() { Name = "Luo", Age = 30, Address = "Beijing" });
                customs.Add(new Custom() { Name = "Wang", Age = 60, Address = "Guangdong" });
                customs.Add(new Custom() { Name = "Feng", Age = 25, Address = "YunNan" });
            }
            
            sw.Reset();
            sw.Start();
            var result2 = customs.AsParallel().Where<Custom>(c => c.Age > 26).ToList(); //测试大部分时间并行Linq还没有普通的快
            sw.Stop();
            Console.WriteLine("Parallel Linq time is {0}.", sw.ElapsedMilliseconds);

            sw.Reset();
            sw.Start();
            var result = customs.Where<Custom>(c => c.Age > 26).ToList();
            sw.Stop();
            Console.WriteLine("Linq time is {0}.", sw.ElapsedMilliseconds);           
        }

    }

     class Custom
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }

    class UserEntity
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public int Flag { get; set; }
    }
}
