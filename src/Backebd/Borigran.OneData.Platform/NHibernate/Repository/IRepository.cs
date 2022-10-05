using NHibernate.Criterion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : class
    {
        Task DeleteAsync(T entity);
        Task<T> SaveAsync(T entity);
        Task<T> SaveOrUpdateAsync(T entity);
        Task UpdateAsync(T entity);
        Task EvictAsync (T entity);
        Task<ICollection<T>> FindAllAsync(Order order, params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(DetachedCriteria criteria, params Order[] orders);
        Task<ICollection<T>> FindAllAsync(ResultSetOptions options, params DetachedCriteria[] criteria);
        Task<ICollection<T>> FindAllAsync(DetachedCriteria[] criteria);
        Task<ICollection<T>> FindAllAsync(DetachedCriteria criteria,
                               int firstResult, int maxResults,
                               params Order[] orders);
        Task<ICollection<T>> FindAllAsync(DetachedCriteria criteria,
                               PagingInfo pagingInfo,
                               params Order[] orders);
        Task<ICollection<T>> FindAllAsync(Order[] orders, params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(int firstResult, int numberOfResults, params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(PagingInfo pagingInfo, params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(int firstResult, int numberOfResults,
                               Order selectionOrder,
                               params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(PagingInfo pagingInfo,
                               Order selectionOrder,
                               params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(int firstResult, int numberOfResults,
                               Order[] selectionOrder,
                               params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(PagingInfo pagingInfo,
                               Order[] selectionOrder,
                               params ICriterion[] criteria);
        Task<ICollection<T>> FindAllAsync(string namedQuery, params Parameter[] parameters);
        Task<ICollection<T>> FindAllAsync(int firstResult,
            int numberOfResults, string namedQuery,
            params Parameter[] parameters);
        Task<T> FindOneAsync(params ICriterion[] criteria);
        Task<T> FindOneAsync(params DetachedCriteria[] criteria);
        Task<T> FindOneAsync(DetachedCriteria criteria);
        Task<T> FindOneAsync(string namedQuery, params Parameter[] parameters);
        Task<bool> ExistsAsync();
        Task<long> CountAsync();
        DetachedCriteria CreateDetachedCriteria();
        DetachedCriteria CreateDetachedCriteria(string aliasName);
    }
}
