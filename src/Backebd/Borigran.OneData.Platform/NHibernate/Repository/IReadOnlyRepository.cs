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
        Task<bool> ExistsAsync(DetachedCriteria criteria);
        Task<bool> ExistsAsync(params ICriterion[] criteria);
        Task<long> CountAsync(DetachedCriteria criteria);
        Task<long> CountAsync(string namedQuery, params Parameter[] parameters);
        Task<T> GetAsync(object id);
        IQueryable<T> Query();
        IQueryOver<T> QueryOver();
        IQueryOver<T, T> QueryOver(Expression<Func<T>> alias);
    }
}
