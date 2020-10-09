using FinPos.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinPos.DAL.Interfaces
{
    public interface IEntityProvider<T> where T : BaseEntity
    {
        IQueryable<T> Get();

        List<TResult> GetAs<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> filter = null, string includeProperties = "", bool ignoreDeleted = false) where TResult : class;
        Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "", bool ignoreDeleted = false);
        Task<List<TResult>> GetAsAsync<TResult>(Expression<Func<T, TResult>> select, int limit, int skip, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool ignoreDeleted = false) where TResult : class;

        Task<List<T>> GetAsync(int limit, int skip, Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool ignoreDeleted = false
          );

        //T Get(Int64 id);
        int GetCount(Expression<Func<T, bool>> filter = null, bool ignoreDeleted = false);
        T GetById(object id);
        TResult GetSingleAs<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> filter);
        Task<TResult> GetSingleAsAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> filter);
        T GetSingle(Expression<Func<T, bool>> filter);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> filter);
        int Insert(T entity);

        void InsertAll(List<T> entities);

        void DeleteAll(List<T> entities);

        void UpdateAll(List<T> entities);

        void Delete(object id);

        void Delete(T entityToDelete);

        int Update(T entityToUpdate);
        bool Any(Expression<Func<T, bool>> filter);

    }
}
