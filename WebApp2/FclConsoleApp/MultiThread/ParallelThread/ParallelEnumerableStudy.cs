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
            //BlockingCollection<int> block = new BlockingCollection<int>();
            //System.Collections.Concurrent.ConcurrentDictionary
            //System.Collections.Concurrent.ConcurrentQueue
            //System.Collections.Concurrent.ConcurrentStack
            //System.Collections.Concurrent.OrderablePartitioner
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
            bool b2 = dic.TryAdd(2, new UserEntity { UserID = 2, UserName = "22" }); //添加
            bool b3 = dic.TryAdd(3, new UserEntity { UserID = 3, UserName = "33" }); //添加

            UserEntity userEntity = new UserEntity();
            bool b4 = dic.TryRemove(1, out userEntity);//移除           
            bool b5 = dic.TryGetValue(2, out userEntity);//获取

            //如果键不存在，方法会在容器中添加新的键和值，如果存在，则更新现有的键和值。
            UserEntity userEntity5 = new UserEntity { UserID = 1, UserName = "55" };
            //bool b8 = dic.AddOrUpdate(5,userEntity5,(key,entity) => entity);

            UserEntity getUser = dic.GetOrAdd(6, new UserEntity { UserID=6, UserName="666" });
            UserEntity getUser2 = dic.GetOrAdd(7, (k) => { return new UserEntity { UserID = 7, UserName = "77" }; });

            //没用对
            //bool b7= dic.TryUpdate(3, new UserEntity { UserID = 3, UserName = "我是33" }, new UserEntity { UserID = 3, UserName = "33" });


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

    public class Custom
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
    }

    class UserEntity
    {
        public int UserID { get; set; }

        public string UserName { get; set; }
    }
}
