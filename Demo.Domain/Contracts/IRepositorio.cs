using Demo.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Contracts
{
    public interface IRepositorio<T>
    {
        T Get(ulong id);
        T Get(string id);
        Task<T> GetAsync(ulong id);
        IQueryable<T> GetAll();
        Page<T> GetPage(int pageNumber, int pageSize, string sortBy, bool ascending, string searchCriteria);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Func<T, bool> predicate);

    }

}
