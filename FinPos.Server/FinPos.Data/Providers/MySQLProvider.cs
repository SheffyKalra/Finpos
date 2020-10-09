using FinPos.DAL.Entities;
using FinPos.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinPos.Data;
using System.Data.Entity;
using System.Linq.Expressions;

namespace FinPos.DAL.Providers
{
    public class MySQLProvider<T> : IEntityProvider<T> where T : BaseEntity
    {
        internal FinPosDbContext context;
        internal DbSet<T> dbSet;
        public MySQLProvider(FinPosDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public IQueryable<T> Get()
        {
            return this.dbSet.AsQueryable();
        }

        


        public virtual List<TResult> GetAs<TResult>(Expression<Func<T, TResult>> select, Expression<Func<T, bool>> filter = null,
            string includeProperties = "", bool ignoreDeleted = false) where TResult : class
        {
            var query = GetQuery(filter, includeProperties);
            return query.Select(select).ToList();
        }
        public virtual Task<List<TResult>> GetAsAsync<TResult>(Expression<Func<T, TResult>> select, int limit, int skip, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool ignoreDeleted = false) where TResult : class
        {
            var query = GetQuery(filter, includeProperties, ignoreDeleted);
            if (orderBy != null)
                return orderBy(query).Skip(skip).Take(limit).Select(select).ToListAsync();
            return query.Select(select).ToListAsync();
        }

        public virtual Task<List<T>> GetAsync(Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool ignoreDeleted = false)
        {

            var query = GetQuery(filter, includeProperties, ignoreDeleted);

            if (orderBy != null)
                return orderBy(query).ToListAsync();

            return query.ToListAsync();
        }

        public virtual Task<List<T>> GetAsync(int limit, int skip, Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool ignoreDeleted = false
          )
        {

            var query = GetQuery(filter, includeProperties, ignoreDeleted);

            if (orderBy != null)
                return orderBy(query).Skip(skip).Take(limit).ToListAsync();

            return query.Skip(skip).Take(limit).ToListAsync();
        }
        public int GetCount(Expression<Func<T, bool>> filter = null, bool ignoreDeleted = false)
        {
            var query = GetQuery(filter, ignoreDeleted: ignoreDeleted);
            var count = query.Count();
            return count;
        }
        private IQueryable<T> GetQuery(Expression<Func<T, bool>> filter = null, string includeProperties = "", bool ignoreDeleted = false)
        {
            IQueryable<T> query = dbSet;

            Type[] types = typeof(T).GetInterfaces();


            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }
        public virtual int Insert(T entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
            return entity.Id.Value;
        }

        public virtual T GetById(object id)
        {
            return dbSet.Find(id);
        }
        public virtual TResult GetSingleAs<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> filter)
        {
            return dbSet.Select(select).FirstOrDefault(filter);
        }
        public virtual Task<TResult> GetSingleAsAsync<TResult>(Expression<Func<T, TResult>> select, Expression<Func<TResult, bool>> filter)
        {
            return dbSet.Select(select).FirstOrDefaultAsync(filter);
        }
        public virtual T GetSingle(Expression<Func<T, bool>> filter)
        {
            return dbSet.FirstOrDefault(filter);
        }
        public virtual Task<T> GetSingleAsync(Expression<Func<T, bool>> filter)
        {
            return dbSet.FirstOrDefaultAsync(filter);
        }

        public virtual void InsertAll(List<T> entities)
        {
            foreach (var entity in entities)
            {
                dbSet.Add(entity);
            }
            context.SaveChanges();
        }

        public virtual void DeleteAll(List<T> entities)
        {
            foreach (var entityToDelete in entities)
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                else
                {
                    dbSet.Remove(entityToDelete);
                }
            }
            context.SaveChanges();

        }

        public virtual void UpdateAll(List<T> entities)
        {
            var ss = entities.GetType().Name;
            foreach (var entityToUpdate in entities)
            {
                //// if (context.Entry(entityToUpdate).State == EntityState.Detached)
                ////  dbSet.Attach(entityToUpdate);     

              
              //  var  ss= context(entities.GetType().Name).Find(entityToUpdate.Id);
                context.Entry(entityToUpdate).State = EntityState.Modified;
            }
                context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
           
        }

        public virtual void Delete(T entityToDelete)
        {

            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            else
            {
                dbSet.Remove(entityToDelete);
            }
            context.SaveChanges();

        }
        public virtual int Update(T entityToUpdate)
        {

            //if (context.Entry(entityToUpdate).State == EntityState.Detached)
            //{
            //    dbSet.Attach(entityToUpdate);
            //}
            context.Entry(entityToUpdate).State = EntityState.Modified;
            context.SaveChanges();
            return entityToUpdate.Id.Value;
        }
        public bool Any(Expression<Func<T, bool>> filter)
        {
            return dbSet.Any(filter);
        }


    }
}
