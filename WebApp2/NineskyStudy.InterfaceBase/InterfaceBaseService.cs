using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NineskyStudy.InterfaceBase
{
    /// <summary>
    /// 仓储基类接口 所有业务接口都要调用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface InterfaceBaseService<T> where T:class
    {
        /// <summary>
        /// 查询[不含导航属性]
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="includeParams">导航属性</param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        T Find(string[] includeParams, Expression<Func<T, bool>> predicate);
    }
}
