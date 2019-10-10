using Demo.Domain.Entities;
using Demo.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Service.Contracts
{
    public interface IServicio<T> where T : BaseEntity
    {
        /// <summary>
        /// Obtiene una entidad en base a su id
        /// </summary>
        /// <param name="id">Id de la entidad</param>
        /// <returns></returns>
        T Get(ulong id);
        /// <summary>
        /// Obtiene una entidad en base a su id
        /// </summary>
        /// <param name="id">Id de la entidad</param>
        /// <returns></returns>
        Task<T> GetAsync(ulong id);
        /// <summary>
        /// Obtiene todas las entidades, genera un log de advertencia en caso de superar los 10.000 resultados
        /// </summary>
        /// <returns>Lista de entidad</returns>
        IList<T> GetAll();
        /// <summary>
        /// Obtiene todas las entidades, genera un log de advertencia en caso de superar los 10.000 resultados
        /// </summary>
        /// <returns>Lista de entidad</returns>
        Task<IList<T>> GetAllAsync();
        /// <summary>
        /// Devuelve todas las entidades paginadas
        /// </summary>
        /// <param name="page">Numero de pagina</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="orderBy">Campo a ordenar</param>
        /// <param name="ascending">True = ascendente - False = descendente</param>
        /// <typeparam name="TKey">Valor del campo a ordenar</typeparam>
        /// <returns>Devuelve la pagina de la entidad</returns>
        Page<T> GetAll<TKey>(int page, int pageSize, Expression<Func<T, TKey>> orderBy, bool ascending = true);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">Numero de pagina</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="orderBy">Campo a ordenar</param>
        /// <param name="thenBy">campo a ordenar luego del campo principal</param>
        /// <param name="ascending">True = ascendente - False = descendente</param>
        /// <typeparam name="TKey">Valor del campo a ordenar</typeparam>
        /// <typeparam name="TSec">Valor del campo secundario a ordenar</typeparam>
        /// <returns>Devuelve la pagina de la entidad</returns>
        Page<T> GetAll<TKey, TSec>(int page, int pageSize, Expression<Func<T, TKey>> orderBy, Expression<Func<T, TSec>> thenBy, bool ascending = true);
        /// <summary>
        /// Devuelve una lista de entidades ordenadas de manera ascendente, genera un log de advertencia si la query supera las 10.000 entidades
        /// </summary>
        /// <param name="keySelector">Campo por el cual se va a ordenar</param>
        /// <typeparam name="TKey">Valor del campo de ordenamiento</typeparam>
        /// <returns>Listado de entidades</returns>
        IList<T> GetAllOrderedBy<TKey>(Expression<Func<T, TKey>> keySelector);
        /// <summary>
        /// Devuelve una lista de entidades ordenadas de manera ascendente, genera un log de advertencia si la query supera las 10.000 entidades
        /// </summary>
        /// <param name="keySelector">Campo por el cual se va a ordenar</param>
        /// <typeparam name="TKey">Valor del campo de ordenamiento</typeparam>
        /// <returns>Listado de entidades</returns>
        Task<IList<T>> GetAllOrderedByAsync<TKey>(Expression<Func<T, TKey>> keySelector);
        /// <summary>
        /// Crea una entidad
        /// </summary>
        /// <param name="item">Entidad</param>
        void Create(T item);
        /// <summary>
        /// Crea una entidad
        /// </summary>
        /// <param name="item">Entidad</param>
        Task CreateAsync(T item);
        /// <summary>
        /// Actualiza una entidad
        /// </summary>
        /// <param name="item">Entidad</param>
        void Update(T item);
        /// <summary>
        /// Actualiza una entidad
        /// </summary>
        /// <param name="item">Entidad</param>
        Task UpdateAsync(T item);
        /// <summary>
        /// Elimina (Soft) una entidad
        /// </summary>
        /// <param name="id">id de la entidad</param>
        void Delete(ulong id);
        /// <summary>
        /// Elimina (Soft) una entidad
        /// </summary>
        /// <param name="id">id de la entidad</param>
        Task DeleteAsync(ulong id);
        /// <summary>
        /// Devuelve todas las entidades que correspondan a la query en el parametro "predicate" paginadas
        /// </summary>
        /// <param name="pageNumber">Numero de pagina</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="orderBy">Campo a ordenar</param>
        /// <param name="ascending">True = ascendente - False = descendente</param>
        /// <param name="predicate">Query</param>
        /// <typeparam name="TKey">Valor del campo a ordenar</typeparam>
        /// <returns>Entidades paginadas</returns>
        Page<T> GetPage<TKey>(int pageNumber,
            int pageSize,
            Expression<Func<T, TKey>> orderBy,
            bool ascending = true,
            Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// Devuelve todas las entidades que correspondan a la query en el parametro "predicate" paginadas
        /// </summary>
        /// <param name="pageNumber">Numero de pagina</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="orderBy">Campo a ordenar</param>
        /// <param name="thenBy">Campo a ordenar luego del campo primario</param>
        /// <param name="ascending">True = ascendente - False = descendente</param>
        /// <param name="predicate">Query</param>
        /// <typeparam name="TKey">Valor del campo a ordenar</typeparam>
        /// <typeparam name="TSec">Valor del campo secundario a ordenar</typeparam>
        /// <returns>Entidades paginadas</returns>
        Page<T> GetPage<TKey, TSec>(int pageNumber,
            int pageSize,
            Expression<Func<T, TKey>> orderBy,
            Expression<Func<T, TSec>> thenBy,
            bool ascending = true,
            Expression<Func<T, bool>> predicate = null);
        /// <summary>
        /// Devuelve todas las entidades que correspondan a la query en el parametro "query" paginadas
        /// </summary>
        /// <param name="pageNumber">Numero de pagina</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="query">Query</param>
        /// <param name="keySelector">Campo a ordenar</param>
        /// <param name="ascending">True = ascendente - False = descendente</param>
        /// <typeparam name="TKey">Valor del campo a ordenar</typeparam>
        /// <returns>Entidades paginadas</returns>
        Page<T> GetPage<TKey>(int pageNumber,
            int pageSize,
            IQueryable<T> query,
            Expression<Func<T, TKey>> keySelector,
            bool ascending = true);

        /// <summary>
        /// Devuelve todas las entidades que correspondan a la query en el parametro "query" paginadas
        /// </summary>
        /// <param name="pageNumber">Numero de pagina</param>
        /// <param name="pageSize">Tamaño de la pagina</param>
        /// <param name="query">Query</param>
        /// <param name="orderBy">Campo a ordenar</param>
        /// <param name="thenBy">Campo a ordenar luego del campo primario</param>
        /// <param name="ascending">True = ascendente - False = descendente</param>
        /// <typeparam name="TKey">Valor del campo a ordenar</typeparam>
        /// <typeparam name="TSec">Valor del campo secundario a ordenar</typeparam>
        /// <returns>Entidades paginadas</returns>
        Page<T> GetPage<TKey, TSec>(int pageNumber,
            int pageSize,
            IQueryable<T> query,
            Expression<Func<T, TKey>> orderBy,
            Expression<Func<T, TSec>> thenBy,
            bool ascending = true);
    }

}
