using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NineskyStudy.InterfaceBase
{
    /// <summary>
    ///  服务基础接口
    /// </summary>
    public interface InterfaceBaseService<T> where T : class
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>添加的记录数[isSave=true时有效]</returns>
        int Add(T entity, bool isSave = true);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>添加的记录数[isSave=true时有效]</returns>
        Task<int> AddAsync(T entity, bool isSave = true);

        /// <summary>
        /// 添加[批量]
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>添加的记录数</returns>
        int AddRange(T[] entities, bool isSave = true);
        /// <summary>
        /// 添加[批量]
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>添加的记录数[isSave=true时有效]</returns>
        Task<int> AddRangeAsync(T[] entities, bool isSave = true);

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>记录数</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询记录数
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>记录数</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>是否存在</returns>
        bool Exists(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>是否存在</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns></returns>
        T Find(int Id);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="Id">主键</param>
        /// <returns></returns>
        Task<T> FindAsync(int Id);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        T Find(object[] keyValues);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="keyValues">主键</param>
        /// <returns></returns>
        Task<T> FindAsync(object[] keyValues);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        T Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>实体列表</returns>
        IQueryable<T> FindList();

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns>实体列表</returns>
        Task<IQueryable<T>> FindListAsync();

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        IQueryable<T> FindList(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        Task<IQueryable<T>> FindListAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="number">返回记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        IQueryable<T> FindList(int number, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="number">返回记录数</param>
        /// <param name="predicate">查询条件</param>
        /// <returns>实体列表</returns>
        Task<IQueryable<T>> FindListAsync(int number, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="number">显示数量[小于等于0-不启用]</param>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序</param>
        /// <param name="isAsc">正序</param>
        /// <returns></returns>
        IQueryable<T> FindList<TKey>(int number, Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="number">显示数量[小于等于0-不启用]</param>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序</param>
        /// <param name="isAsc">正序</param>
        /// <returns></returns>
        Task<IQueryable<T>> FindListAsync<TKey>(int number, Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc);

        /// <summary>
        /// 查询[分页]
        /// </summary>
        /// <typeparam name="TKey">排序属性</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序</param>
        /// <param name="isAsc">是否正序</param>
        /// <param name="paging">分页数据</param>
        /// <returns></returns>
        Paging<T> FindList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc, Paging<T> paging);

        /// <summary>
        /// 查询[分页]
        /// </summary>
        /// <typeparam name="TKey">排序属性</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序</param>
        /// <param name="isAsc">是否正序</param>
        /// <param name="paging">分页数据</param>
        /// <returns></returns>
        Task<Paging<T>> FindListAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc, Paging<T> paging);

        /// <summary>
        /// 查询[分页]
        /// </summary>
        /// <typeparam name="TKey">排序属性</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序</param>
        /// <param name="isAsc">是否正序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        Paging<T> FindList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc, int pageIndex, int pageSize);

        /// <summary>
        /// 查询[分页]
        /// </summary>
        /// <typeparam name="TKey">排序属性</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">排序</param>
        /// <param name="isAsc">是否正序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录数</param>
        /// <returns></returns>
        Task<Paging<T>> FindListAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc, int pageIndex, int pageSize);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>是否删除成功[isSave=true时有效]</returns>
        bool Remove(T entity, bool isSave = true);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>是否删除成功[isSave=true时有效]</returns>
        Task<bool> RemoveAsync(T entity, bool isSave = true);

        /// <summary>
        /// 删除[批量]
        /// </summary>
        /// <param name="entities">实体数组</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>成功删除的记录数[isSave=true时有效]</returns>
        int RemoveRange(T[] entities, bool isSave = true);

        /// <summary>
        /// 删除[批量]
        /// </summary>
        /// <param name="entities">实体数组</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>成功删除的记录数[isSave=true时有效]</returns>
        Task<int> RemoveRangeAsync(T[] entities, bool isSave = true);

        /// <summary>
        ///  保存数据
        /// </summary>
        /// <returns>更改的记录数</returns>
        int SaveChanges();

        /// <summary>
        ///  保存数据
        /// </summary>
        /// <returns>更改的记录数</returns>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>是否保存成功[isSave=true时有效]</returns>
        bool Update(T entity, bool isSave = true);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>是否保存成功[isSave=true时有效]</returns>
        Task<bool> UpdateAsync(T entity, bool isSave = true);

        /// <summary>
        /// 更新[批量]
        /// </summary>
        /// <param name="entities">实体数组</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>更新成功的记录数[isSave=true时有效]</returns>
        int UpdateRange(T[] entities, bool isSave = true);

        /// <summary>
        /// 更新[批量]
        /// </summary>
        /// <param name="entities">实体数组</param>
        /// <param name="isSave">是否立即保存</param>
        /// <returns>更新成功的记录数[isSave=true时有效]</returns>
        Task<int> UpdateRangeAsync(T[] entities, bool isSave = true);

    }
}
