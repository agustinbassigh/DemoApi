using Demo.Domain.Constants;
using Demo.Domain.Contracts;
using Demo.Domain.Entities;
using Demo.Domain.Extensions;
using Demo.Service.Contracts;
using Demo.Service.Exceptions;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Service.Implmentation
{
    public class Servicio<T> : IServicio<T> where T : BaseEntity
    {
        #region Propiedades

        protected readonly IRepositorio<T> Repositorio;
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly ILogger<Servicio<T>> Logger;
        protected readonly IHttpContextAccessor HttpContextAccessor;
        protected readonly Guid InstanceGuid;
        private readonly string _tipo;
        private readonly JsonSerializerSettings _jsonOptions;

        #endregion

        #region constructor

        public Servicio(IRepositorio<T> repositorio, IUnitOfWork unitOfWork, ILogger<Servicio<T>> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            Repositorio = repositorio;
            UnitOfWork = unitOfWork;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            InstanceGuid = Guid.NewGuid();
            _tipo = typeof(T).ToString();
            _jsonOptions = new JsonSerializerSettings();
            _jsonOptions.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

        #endregion

        #region Metodos Publicos

        public T Get(ulong id)
        {
            Logger.LogInformation(LogEvents.Service,
                "Buscando entidad del tipo {_tipo} con id:{id}.", _tipo, id);

            var entity = Repositorio.Get(id);


            var entidad = JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.Indented, _jsonOptions);

            Logger.LogInformation(LogEvents.Service,
                "Entidad obtenida: {entidad}.", entidad);

            return entity;
        }

        public async Task<T> GetAsync(ulong id)
        {
            Logger.LogInformation(LogEvents.Service,
                "Buscando entidad del tipo {_tipo} con id:{id}.", _tipo, id);

            var entity = await Repositorio.GetAsync(id);

            var entidad = JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.Indented, _jsonOptions);

            Logger.LogInformation(LogEvents.Service,
                "Entidad obtenida: {entidad}.", entidad);

            return entity;
        }

        protected T Get(Expression<Func<T, bool>> predicate)
        {
            Logger.LogInformation(LogEvents.Service,
                "Buscando entidad del tipo {_tipo} con predicado:{predicate}.", _tipo, predicate);

            var entity = GetAllItems().SingleOrDefault(predicate);

            var entidad = JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.Indented, _jsonOptions);

            Logger.LogInformation(LogEvents.Service,
                "Entidad obtenida: {entidad}.", entidad);

            return entity;
        }

        protected async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            Logger.LogInformation(LogEvents.Service,
                "Buscando entidad del tipo {_tipo} con predicado:{predicate}.", _tipo, predicate);

            var entity = await GetAllItems().SingleOrDefaultAsync(predicate);

            var entidad = JsonConvert.SerializeObject(entity, Newtonsoft.Json.Formatting.Indented, _jsonOptions);

            Logger.LogInformation(LogEvents.Service,
                "Entidad obtenida: {entidad}.", entidad);

            return entity;
        }

        protected IQueryable<T> GetAllItems()
        {
            return GetAllItems(null);
        }

        protected IQueryable<T> GetAllItems(Expression<Func<T, bool>> predicate)
        {
            var query = Repositorio.GetAll();

            if (predicate != null)
            {
                Logger.LogInformation(LogEvents.Service,
                    "Buscando entidades del tipo {_tipo} con predicado:{predicate}.", _tipo, predicate);
                return query.Where(predicate);
            }

            Logger.LogInformation(LogEvents.Service,
                "Buscando entidades del tipo {_tipo} sin ningun tipo de predicado. Si la lista supera los 10.000 resultados se emitira una advertencia",
                _tipo);
            return query;
        }

        public IList<T> GetAll()
        {
            Logger.LogInformation(LogEvents.Service,
                "Buscando entidades del tipo {_tipo} sin ningun tipo de predicado. Si la lista supera los 10.000 resultados se emitira una advertencia",
                _tipo);
            var items = GetAllItems().ToList();

            if (items.Count > 10000)
                Logger.LogInformation(LogEvents.Service,
                    "Se buscaron mas de 10.000 entidades del tipo {_tipo} sin ningun tipo de predicado.", _tipo);

            return items;
        }

        public async Task<IList<T>> GetAllAsync()
        {
            Logger.LogInformation(LogEvents.Service,
                "Buscando entidades del tipo {_tipo} sin ningun tipo de predicado. Si la lista supera los 10.000 resultados se emitira una advertencia",
                _tipo);
            var items = await GetAllItems().ToListAsync();

            if (items.Count > 10000)
                Logger.LogInformation(LogEvents.Service,
                    "Se buscaron mas de 10.000 entidades del tipo {_tipo} sin ningun tipo de predicado.", _tipo);

            return items;
        }

        public IList<T> GetAllOrderedBy<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            Logger.LogInformation(LogEvents.Service,
                "Buscando entidades del tipo {_tipo} sin ningun tipo de predicado y ordenadas por:{keySelector}. Si la lista supera los 10.000 resultados se emitira una advertencia",
                _tipo, keySelector);
            var items = GetAllItems().OrderBy(keySelector).ToList();

            if (items.Count > 10000)
                Logger.LogInformation(LogEvents.Service,
                    "Se buscaron mas de 10.000 entidades del tipo {_tipo} sin ningun tipo de predicado y ordenadas por:{keySelector}.",
                    _tipo, keySelector);

            return items;
        }

        public async Task<IList<T>> GetAllOrderedByAsync<TKey>(Expression<Func<T, TKey>> keySelector)
        {
            Logger.LogInformation(LogEvents.Service,
                "Buscando entidades del tipo {_tipo} sin ningun tipo de predicado y ordenadas por:{keySelector}. Si la lista supera los 10.000 resultados se emitira una advertencia",
                _tipo, keySelector);
            var items = await GetAllItems().OrderBy(keySelector).ToListAsync();

            if (items.Count > 10000)
                Logger.LogInformation(LogEvents.Service,
                    "Se buscaron mas de 10.000 entidades del tipo {_tipo} sin ningun tipo de predicado y ordenadas por:{keySelector}.",
                    _tipo, keySelector);

            return items;
        }

        public virtual Page<T> GetAll<TKey>(int page, int pageSize, Expression<Func<T, TKey>> orderBy,
            bool ascending = true)
        {
            return GetPage(page, pageSize, orderBy, ascending);
        }

        public virtual Page<T> GetAll<TKey, TSec>(int page, int pageSize, Expression<Func<T, TKey>> orderBy,
            Expression<Func<T, TSec>> thenBy, bool ascending = true)
        {
            return GetPage(page, pageSize, orderBy, thenBy, ascending);
        }

        public virtual void Create(T item)
        {
            try
            {
                Repositorio.Add(item);
                Logger.LogInformation(LogEvents.Service,
                    "Se agrego un item del tipo {_tipo} al repositorio, valores:{item}", _tipo, item);
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                var exception = new ServiceException("Ocurrio un error al crear el item", ex);
                Logger.LogError(LogEvents.Service, exception, "Ocurrio un error creando el item:{item}", item);
                throw exception;
            }
        }

        public virtual async Task CreateAsync(T item)
        {
            try
            {
                Repositorio.Add(item);
                Logger.LogInformation(LogEvents.Service,
                    "Se agrego un item del tipo {_tipo} al repositorio, valores:{item}", _tipo, item);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var exception = new ServiceException("Ocurrio un error al crear el item", ex);
                Logger.LogError(LogEvents.Service, exception, "Ocurrio un error creando el item:{item}", item);
                throw exception;
            }
        }

        public virtual void Update(T item)
        {
            try
            {
                Logger.LogInformation(LogEvents.Service,
                    "Se solicito actualizar un item del tipo {_tipo}, valores:{item}", _tipo,
                    item);
                Repositorio.Update(item);
                Logger.LogInformation(LogEvents.Service,
                    "Se actualizo un item del tipo {_tipo} en el repositorio, valores:{item}", _tipo, item);
                UnitOfWork.SaveChanges();
                Logger.LogInformation(LogEvents.Service, "Item actualizado del tipo {_tipo}, valores:{item}", _tipo,
                    item);
            }
            catch (Exception ex)
            {
                var exception = new ServiceException("Ocurrio un error al editar el item", ex);
                Logger.LogError(LogEvents.Service, exception, "Ocurrio un error actualizando el item:{item}", item);
                throw exception;
            }
        }

        public virtual async Task UpdateAsync(T item)
        {
            try
            {
                Logger.LogInformation(LogEvents.Service,
                    "Se solicito actualizar un item del tipo {_tipo}, valores:{item}", _tipo,
                    item);
                Repositorio.Update(item);
                Logger.LogInformation(LogEvents.Service,
                    "Se actualizo un item del tipo {_tipo} en el repositorio, valores:{item}", _tipo, item);
                await UnitOfWork.SaveChangesAsync();
                Logger.LogInformation(LogEvents.Service, "Item actualizado del tipo {_tipo}, valores:{item}", _tipo,
                    item);
            }
            catch (Exception ex)
            {
                var exception = new ServiceException("Ocurrio un error al editar el item", ex);
                Logger.LogError(LogEvents.Service, exception, "Ocurrio un error actualizando el item:{item}", item);
                throw exception;
            }
        }

        public virtual void Delete(ulong id)
        {
            try
            {
                var item = Repositorio.Get(id);
                Logger.LogInformation(LogEvents.Service,
                    "Se solicito eliminar un item del tipo {_tipo}, valores:{item}", _tipo,
                    item);
                Repositorio.Delete(item);
                Logger.LogInformation(LogEvents.Service,
                    "Se elimino un item del tipo {_tipo} en el repositorio, valores:{item}", _tipo, item);
                UnitOfWork.SaveChanges();
                Logger.LogInformation(LogEvents.Service, "Item eliminado del tipo {_tipo}, valores:{item}", _tipo,
                    item);
            }
            catch (Exception ex)
            {
                var exception = new ServiceException("Ocurrio un error al eliminar el item", ex);
                Logger.LogError(LogEvents.Service, exception, "Ocurrio un error elimando el item con id:{id}", id);
                throw exception;
            }
        }

        public virtual async Task DeleteAsync(ulong id)
        {
            try
            {
                var item = await Repositorio.GetAsync(id);
                Logger.LogInformation(LogEvents.Service,
                    "Se solicito eliminar un item del tipo {_tipo}, valores:{item}", _tipo,
                    item);
                Repositorio.Delete(item);
                Logger.LogInformation(LogEvents.Service,
                    "Se elimino un item del tipo {_tipo} en el repositorio, valores:{item}", _tipo, item);
                await UnitOfWork.SaveChangesAsync();
                Logger.LogInformation(LogEvents.Service, "Item eliminado del tipo {_tipo}, valores:{item}", _tipo,
                    item);
            }
            catch (Exception ex)
            {
                var exception = new ServiceException("Ocurrio un error al eliminar el item", ex);
                Logger.LogError(LogEvents.Service, exception, "Ocurrio un error elimando el item con id:{id}", id);
                throw exception;
            }
        }

        /// <summary>
        /// Función para obtener una página a partir del número y tamaño de la página deseada
        /// </summary>
        /// <param name="pageNumber">Número de página a devolver</param>
        /// <param name="pageSize">Tamaño deseado de la página</param>
        /// <param name="orderBy">Selector del campo de ordenamiento. Necesario debido al uso de la función Skip que requiere elementos ordenados.</param>
        /// <param name="ascending">Indicador para describir si el ordenamiento es ascendente o descendente</param>
        /// <param name="predicate">Predicado para realizar el filtrado de los elementos a devolver</param>
        /// <returns>Devuelve un objeto de tipo Page</returns>
        public Page<T> GetPage<TKey>(int pageNumber,
            int pageSize,
            Expression<Func<T, TKey>> orderBy,
            bool ascending = true,
            Expression<Func<T, bool>> predicate = null)
        {
            var query = GetAllItems(predicate).AsExpandable();

            return GetPage(pageNumber, pageSize, query, orderBy, ascending);
        }

        /// <summary>
        /// Función para obtener una página a partir del número y tamaño de la página deseada
        /// </summary>
        /// <param name="pageNumber">Número de página a devolver</param>
        /// <param name="pageSize">Tamaño deseado de la página</param>
        /// <param name="orderBy">Selector del campo de ordenamiento. Necesario debido al uso de la función Skip que requiere elementos ordenados.</param>
        /// <param name="thenBy">Selector secundario del campo de ordenamiento. Opcional.</param>
        /// <param name="ascending">Indicador para describir si el ordenamiento es ascendente o descendente</param>
        /// <param name="predicate">Predicado para realizar el filtrado de los elementos a devolver</param>
        /// <returns>Devuelve un objeto de tipo Page</returns>
        public Page<T> GetPage<TKey, TSec>(int pageNumber,
            int pageSize,
            Expression<Func<T, TKey>> orderBy,
            Expression<Func<T, TSec>> thenBy = null,
            bool ascending = true,
            Expression<Func<T, bool>> predicate = null)
        {
            var query = GetAllItems(predicate).AsExpandable();

            return GetPage(pageNumber, pageSize, query, orderBy, thenBy, ascending);
        }

        /// <summary>
        /// Función para obtener una página a partir del número y tamaño de la página deseada
        /// </summary>
        /// <param name="pageNumber">Número de página a devolver</param>
        /// <param name="pageSize">Tamaño deseado de la página</param>
        /// <param name="orderBy">Selector del campo de ordenamiento. Necesario debido al uso de la función Skip que requiere elementos ordenados.</param>
        /// <param name="ascending">Indicador para describir si el ordenamiento es ascendente o descendente</param>
        /// <param name="query">Query para usar en lugar del repositorio por defecto</param>
        /// <returns>Devuelve un objeto de tipo Page</returns>
        public Page<T> GetPage<TKey>(int pageNumber,
            int pageSize,
            IQueryable<T> query,
            Expression<Func<T, TKey>> orderBy,
            bool ascending = true)
        {
            query = ascending
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);

            Logger.LogInformation(LogEvents.Service,
                "Paginando entidades del tipo {_tipo} con query: {query},ordenada por {orderBy}.",
                _tipo, query, orderBy);

            return query.ToPage(pageNumber, pageSize);
        }

        /// <summary>
        /// Función para obtener una página a partir del número y tamaño de la página deseada
        /// </summary>
        /// <param name="pageNumber">Número de página a devolver</param>
        /// <param name="pageSize">Tamaño deseado de la página</param>
        /// <param name="orderBy">Selector del campo de ordenamiento. Necesario debido al uso de la función Skip que requiere elementos ordenados.</param>
        /// <param name="thenBy">Selector secundario del campo de ordenamiento. Opcional.</param>
        /// <param name="ascending">Indicador para describir si el ordenamiento es ascendente o descendente</param>
        /// <param name="query">Query para usar en lugar del repositorio por defecto</param>
        /// <returns>Devuelve un objeto de tipo Page</returns>
        public Page<T> GetPage<TKey, TSec>(int pageNumber,
            int pageSize,
            IQueryable<T> query,
            Expression<Func<T, TKey>> orderBy,
            Expression<Func<T, TSec>> thenBy,
            bool ascending = true)
        {
            query = ascending
                ? query.OrderBy(orderBy).ThenBy(thenBy)
                : query.OrderByDescending(orderBy).ThenByDescending(thenBy);

            Logger.LogInformation(LogEvents.Service,
                "Paginando entidades del tipo {_tipo} con query: {query},ordenada por {orderBy} y luego por {thenBy}.",
                _tipo, query, orderBy, thenBy);

            return query.ToPage(pageNumber, pageSize);
        }

        #endregion
    }

}
