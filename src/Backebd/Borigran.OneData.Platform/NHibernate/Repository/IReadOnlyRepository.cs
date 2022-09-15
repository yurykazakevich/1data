using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    public interface IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// Check if any instance matches the criteria.
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        Task<bool> ExistsAsync(DetachedCriteria criteria);

        /// <summary>
        /// Check if any instance matches the criteria.
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        Task<bool> ExistsAsync(params ICriterion[] criteria);

        /// <summary>
        /// Counts the number of instances matching the criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<long> CountAsync(DetachedCriteria criteria);

        /// <summary>
        /// Execute the named query and return the number of matching criteria
        /// </summary>
        /// <param name="namedQuery">The named query to execute</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <returns>The results of the query</returns>
        Task<long> CountAsync(string namedQuery, params Parameter[] parameters);

        /// <summary>
        /// Get the entity from the persistance store, or return null
        /// if it doesn't exist.
        /// </summary>
        /// <param name="id">The entity's id</param>
        /// <returns>Either the entity that matches the id, or a null</returns>
        Task<T> GetAsync(object id);

        /// <summary>
        /// Provides a linq queryable interface
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query();

        /// <summary>
        /// Provides QueryOver interface 
        /// </summary>
        /// <returns></returns>
        IQueryOver<T> QueryOver();

        /// <summary>
        /// Provides QueryOver interface
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        IQueryOver<T, T> QueryOver(Expression<Func<T>> alias);
    }
}
