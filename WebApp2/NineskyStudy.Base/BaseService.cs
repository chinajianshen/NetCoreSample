using Microsoft.EntityFrameworkCore;
using NineskyStudy.InterfaceBase;
using NineskyStudy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NineskyStudy.Base
{
    public class BaseService<T> : InterfaceBaseService<T> where T : class
    {
        protected DbContext _dbContext;

        public BaseService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual int Add(T entity, bool isSave = true)
        {
            _dbContext.Set<T>().Add(entity);
            if (isSave) return _dbContext.SaveChanges();
            else return 0;
        }

        public virtual async Task<int> AddAsync(T entity, bool isSave = true)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            if (isSave) return await _dbContext.SaveChangesAsync();
            else return 0;
        }

        public virtual int AddRange(T[] entities, bool isSave = true)
        {
            _dbContext.Set<T>().AddRange(entities);
            if (isSave) return _dbContext.SaveChanges();
            else return 0;
        }

        public virtual async Task<int> AddRangeAsync(T[] entities, bool isSave = true)
        {
            await _dbContext.Set<T>().AddRangeAsync();
            if (isSave) return await _dbContext.SaveChangesAsync();
            else return 0;
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Count(predicate);
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().CountAsync(predicate);
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return Count(predicate) > 0;
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await CountAsync(predicate)>0;
        }

        public virtual T Find(int Id)
        {
            return _dbContext.Set<T>().Find(Id);
        }

        public virtual T Find(object[] keyValues)
        {
            return _dbContext.Set<T>().Find(keyValues);
        }

        public virtual T Find(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().SingleOrDefault(predicate);
        }

        public virtual async Task<T> FindAsync(int Id)
        {
            return await _dbContext.Set<T>().FindAsync(Id);
        }

        public virtual async Task<T> FindAsync(object[] keyValues)
        {
            return await _dbContext.Set<T>().FindAsync(keyValues);
        }

        public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public virtual IQueryable<T> FindList<TKey>(int number, Expression<Func<T, bool>> predicate)
        {
            var entityList = _dbContext.Set<T>().Where(predicate);
            if (number > 0) return entityList.Take(number);
            else return entityList;
        }

        public virtual IQueryable<T> FindList<TKey>(int number, Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc)
        {
            var entityList = _dbContext.Set<T>().Where(predicate);
            if (isAsc) entityList = entityList.OrderBy(keySelector);
            else entityList.OrderByDescending(keySelector);

            if (number > 0) return entityList.Take(number);
            else return entityList;

        }

        public virtual Paging<T> FindList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc, Paging<T> paging)
        {
            var entityList = _dbContext.Set<T>().Where(predicate);
            paging.Total = entityList.Count();
            if (isAsc) entityList = entityList.OrderBy(keySelector);
            else entityList.OrderByDescending(keySelector);
            paging.Entities = entityList.Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize).ToList();
            return paging;
        }

        public virtual Paging<T> FindList<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc, int pageIndex, int pageSize)
        {
            Paging<T> paging = new Paging<T> { PageIndex=pageIndex, PageSize=pageSize };
            return FindList(predicate, keySelector, isAsc, paging);
        }

        public virtual IQueryable<T> FindList()
        {
            return _dbContext.Set<T>();
        }

        public virtual IQueryable<T> FindList(Expression<Func<T, bool>> predicate)
        {
            return _dbContext.Set<T>().Where(predicate);
        }

        public virtual IQueryable<T> FindList(int number, Expression<Func<T, bool>> predicate)
        {
            var entityList = _dbContext.Set<T>().Where(predicate);
            if (number > 0) return entityList.Take(number);
            else return entityList;
        }

        public virtual async Task<IQueryable<T>> FindListAsync()
        {
            IQueryable<T> result = _dbContext.Set<T>();

            //异步中用到同步
            return await Task.FromResult(result);
        }

        public virtual async Task<IQueryable<T>> FindListAsync(Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(FindList(predicate));
        }

        public virtual async Task<IQueryable<T>> FindListAsync(int number, Expression<Func<T, bool>> predicate)
        {
            return await Task.FromResult(FindList(number, predicate));
        }

        public virtual async Task<IQueryable<T>> FindListAsync<TKey>(int number, Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc)
        {
            var entityList = _dbContext.Set<T>().Where(predicate);
            if (isAsc) entityList = entityList.OrderBy(keySelector);
            else entityList.OrderByDescending(keySelector);

            if (number > 0) entityList = entityList.Take(number);
            return await Task.FromResult(entityList);
        }

        public virtual async Task<Paging<T>> FindListAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc, Paging<T> paging)
        {
            var entityList = _dbContext.Set<T>().Where(predicate);
            paging.Total = await entityList.CountAsync();
            if (isAsc) entityList = entityList.OrderBy(keySelector);
            else entityList.OrderByDescending(keySelector);

            paging.Entities = await entityList.Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize).ToListAsync();

            return paging;           
        }

        public virtual async Task<Paging<T>> FindListAsync<TKey>(Expression<Func<T, bool>> predicate, Expression<Func<T, TKey>> keySelector, bool isAsc, int pageIndex, int pageSize)
        {
            Paging<T> paging = new Paging<T> { PageIndex = pageIndex, PageSize = pageSize };
            return await FindListAsync(predicate,keySelector,isAsc,paging);
        }

        public virtual bool Remove(T entity, bool isSave = true)
        {
            _dbContext.Set<T>().Remove(entity);
            if (isSave) return _dbContext.SaveChanges() > 0;
            else return false;
        }

        public virtual async Task<bool> RemoveAsync(T entity, bool isSave = true)
        {
            _dbContext.Set<T>().Remove(entity);
            if (isSave) return await _dbContext.SaveChangesAsync() > 0;
            else return false;
        }

        public virtual int RemoveRange(T[] entities, bool isSave = true)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            if (isSave) return _dbContext.SaveChanges();
            else return 0;
        }

        public virtual async Task<int> RemoveRangeAsync(T[] entities, bool isSave = true)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            if (isSave) return await _dbContext.SaveChangesAsync();
            else return 0;
        }

        public virtual int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public bool Update(T entity, bool isSave = true)
        {
            _dbContext.Set<T>().Update(entity);
            if (isSave) return _dbContext.SaveChanges()>0;
            else return false;
        }

        public virtual async Task<bool> UpdateAsync(T entity, bool isSave = true)
        {
            _dbContext.Set<T>().Update(entity);
            if (isSave) return await _dbContext.SaveChangesAsync() > 0;
            else return false;
        }

        public int UpdateRange(T[] entities, bool isSave = true)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            if (isSave) return _dbContext.SaveChanges();
            else return 0;
        }

        public virtual async Task<int> UpdateRangeAsync(T[] entities, bool isSave = true)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            if (isSave) return await _dbContext.SaveChangesAsync();
            else return 0;
        }
    }
}
