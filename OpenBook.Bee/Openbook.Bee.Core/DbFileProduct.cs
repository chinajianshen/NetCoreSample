using OpenBook.Bee.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openbook.Bee.Core
{
    /// <summary>
    /// 数据库文件产品
    /// </summary>
    public class DbFileProduct
    {
        private Dictionary<string, Func<T8TaskEntity, T8TaskEntity>> productParts;
        public DbFileProduct()
        {
            productParts = new Dictionary<string, Func<T8TaskEntity, T8TaskEntity>>();
        }

        public void AddPart(string key, Func<T8TaskEntity, T8TaskEntity> func)
        {
            if (!productParts.Keys.Contains(key))
            {
                productParts.Add(key, func);
            }
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="t8Task"></param>
        public void Execute(T8TaskEntity t8Task)
        {
            foreach (KeyValuePair<string,Func<T8TaskEntity, T8TaskEntity>> item in productParts)
            {                
                t8Task = item.Value(t8Task);
            }
        }
    }

    /// <summary>
    /// 数据库文件产品抽象构造者类
    /// </summary>
    public abstract class ADbFileProductBuilder
    {
        /// <summary>
        /// 新建数据库文件及数据
        /// </summary>
        /// <param name="t8Task"></param>
        public abstract T8TaskEntity BuildDbFile(T8TaskEntity t8Task);

        /// <summary>
        /// 创建数据库压缩文件
        /// </summary>
        /// <param name="t8Task"></param>
        /// <returns></returns>
        public abstract T8TaskEntity BuildCompressFile(T8TaskEntity t8Task);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="t8Task"></param>
        /// <returns></returns>
        public abstract T8TaskEntity BuildUploadFile(T8TaskEntity t8Task);

        /// <summary>
        /// 备份数据库文件
        /// </summary>
        /// <param name="t8Task"></param>
        /// <returns></returns>
        public abstract T8TaskEntity BackupDbFile(T8TaskEntity t8Task);
    }

    /// <summary>
    /// 数据库文件产品实现类
    /// </summary>
    public class DbFileProductBuilder : ADbFileProductBuilder
    {
        public override T8TaskEntity BackupDbFile(T8TaskEntity t8Task)
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
            throw new NotFiniteNumberException();
        }

        public override T8TaskEntity BuildCompressFile(T8TaskEntity t8Task)
        {
            throw new NotImplementedException();
        }

        public override T8TaskEntity BuildDbFile(T8TaskEntity t8Task)
        {
            throw new NotImplementedException();
        }

        public override T8TaskEntity BuildUploadFile(T8TaskEntity t8Task)
        {
            throw new NotImplementedException();
        }
    }
}
