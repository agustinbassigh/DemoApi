using Demo.Domain.Contracts;
using Demo.Domain.Entities;
using Demo.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Implementations
{
    public class Repositorio<T> : IRepositorio<T> where T : BaseEntity
    {
        protected readonly DbContext DbContext;

        public Repositorio(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        public virtual T Get(ulong id)
        {
            return DbContext.Set<T>().Find(id);
        }
        public virtual async Task<T> GetAsync(ulong id)
        {
            return await DbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public virtual T Get(string id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public IQueryable<T> GetAll()
        {
            return DbContext.Set<T>().Select(x => x);
        }

        public virtual void Add(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbContext.Update<T>(entity);
        }

        public virtual void Delete(T entity)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
                DbContext.Set<T>().Attach(entity);

            DbContext.Set<T>().Remove(entity);
        }

        public virtual void Delete(Func<T, bool> predicate)
        {
            var records = DbContext.Set<T>().Where(predicate);

            foreach (var record in records)
            {
                DbContext.Set<T>().Remove(record);
            }

        }

        public Page<T> GetPage(int pageNumber, int pageSize, string sortBy, bool ascending, string searchCriteria)
        {
            var query = DbContext.Set<T>().Select(x => x);
            return query.ToPage(pageNumber, pageSize);
        }

    }

}
