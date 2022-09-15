using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    public abstract class RepositoryBase<T> where T : class
    {
        private static readonly Order[] NullOrderArray = null;

        protected abstract DisposableAction<ISession> ActionToBePerformedOnSessionUsedForDBFetches
        {
            get;
        }

        protected abstract ISessionFactory SessionFactory { get; }

        public DetachedCriteria CreateDetachedCriteria()
        {
            return DetachedCriteria.For<T>();
        }

        public DetachedCriteria CreateDetachedCriteria(string alias)
        {
            return DetachedCriteria.For<T>(alias);
        }

        public async Task<ICollection<T>> FindAllAsync(params ICriterion[] criteria)
        {
            return await FindAllAsync(NullOrderArray, criteria);
        }

        public async Task<ICollection<T>> FindAllAsync(Order order, params ICriterion[] criteria)
        {
            return await FindAllAsync(new Order[] { order }, criteria);
        }

        public async Task<ICollection<T>> FindAllAsync(Order[] orders, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, orders);
                return await crit.ListAsync<T>();
            }
        }

        public async Task<ICollection<T>> FindAllAsync(DetachedCriteria[] criteria)
        {
            return await FindAllAsync(ResultSetOptions.None, criteria);
        }
        public async Task<ICollection<T>> FindAllAsync(ResultSetOptions options, params DetachedCriteria[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value, criteria);
                IEnumerable<T> result = new List<T>();
                bool first = true;
                foreach (IList resultItem in await crit.ListAsync())
                {
                    IEnumerable<T> mappedResultItem = resultItem.Cast<T>();

                    if (first) result = new List<T>(mappedResultItem);

                    switch (options)
                    {
                        case ResultSetOptions.Union:
                            result = result.Union(mappedResultItem);
                            break;
                        case ResultSetOptions.Intersect:
                            result = result.Intersect(mappedResultItem);
                            break;
                        default:
                            if (!first) (result as List<T>).AddRange(mappedResultItem);
                            break;
                    }
                    first = false;
                }

                return result.ToList();
            }
        }

        public async Task<ICollection<T>> FindAllAsync(DetachedCriteria criteria, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, orders);
                return await crit.ListAsync<T>();
            }
        }

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="criteria">the criteria to look for</param>
        /// <param name="orders"> the order to load the entities</param>
        /// <param name="firstResult">the first result to load</param>
        /// <param name="maxResults">the number of result to load</param>
        /// <returns>All the entities that match the criteria</returns>
        public async Task<ICollection<T>> FindAllAsync(DetachedCriteria criteria, int firstResult, int maxResults, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, orders);
                crit.SetFirstResult(firstResult)
                    .SetMaxResults(maxResults);
                return await crit.ListAsync<T>();
            }
        }

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="criteria">the criteria to look for</param>
        /// <param name="orders"> the order to load the entities</param>
        /// <param name="pagingInfo">the paging information</param>
        /// <returns>All the entities that match the criteria</returns>
        public async Task<ICollection<T>> FindAllAsync(DetachedCriteria criteria, PagingInfo pagingInfo, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                var countCriteria = CriteriaTransformer.Clone(criteria);

                RepositoryHelper<T>.AddOrders(criteria, orders);

                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value,
                    new[]{
                        countCriteria.SetProjection(Projections.RowCount()),
                        criteria.SetFirstResult(pagingInfo.FirstResult).SetMaxResults(pagingInfo.MaxResults)
                        });

                return await DoMultiCriteriaPaging<T>(crit, pagingInfo);
            }
        }

        /// <summary>
        /// Loads all the entities that match the criteria, and allow paging.
        /// </summary>
        /// <param name="firstResult">The first result to load</param>
        /// <param name="numberOfResults">Total number of results to load</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        public async Task<ICollection<T>> FindAllAsync(int firstResult, int numberOfResults, params ICriterion[] criteria)
        {
            return await FindAllAsync(firstResult, numberOfResults, NullOrderArray, criteria);
        }

        /// <summary>
        /// Loads all the entities that match the criteria, and allow paging.
        /// </summary>
        /// <param name="pagingInfo">The paging information</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        public async Task<ICollection<T>> FindAllAsync(PagingInfo pagingInfo, params ICriterion[] criteria)
        {
            return await FindAllAsync(pagingInfo, NullOrderArray, criteria);
        }

        /// <summary>
        /// Loads all the entities that match the criteria, with paging 
        /// and orderring by a single field.
        /// <param name="firstResult">The first result to load</param>
        /// <param name="numberOfResults">Total number of results to load</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// <param name="selectionOrder">The field the repository should order by</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// </summary>
        public async Task<ICollection<T>> FindAllAsync(int firstResult, int numberOfResults, Order selectionOrder, params ICriterion[] criteria)
        {
            return await FindAllAsync(firstResult, numberOfResults, new Order[] { selectionOrder }, criteria);
        }

        /// <summary>
        /// Loads all the entities that match the criteria, with paging 
        /// and orderring by a single field.
        /// <param name="pagingInfo">The paging information</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// <param name="selectionOrder">The field the repository should order by</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// </summary>
        public async Task<ICollection<T>> FindAllAsync(PagingInfo pagingInfo, Order selectionOrder, params ICriterion[] criteria)
        {
            return await FindAllAsync(pagingInfo, new Order[] { selectionOrder }, criteria);
        }

        /// <summary>
        /// Loads all the entities that match the criteria, with paging 
        /// and orderring by a multiply fields.
        /// </summary>
        /// <param name="firstResult">The first result to load</param>
        /// <param name="numberOfResults">Total number of results to load</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// <param name="selectionOrder">The fields the repository should order by</param>
        public async Task<ICollection<T>> FindAllAsync(int firstResult, int numberOfResults, Order[] selectionOrder, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, selectionOrder);
                crit.SetFirstResult(firstResult)
                    .SetMaxResults(numberOfResults);
                return await crit.ListAsync<T>();
            }
        }

        /// <summary>
        /// Loads all the entities that match the criteria, with paging 
        /// and orderring by a multiply fields.
        /// </summary>
        /// <param name="pagingInfo">The paging information</param>
        /// <param name="criterion">the criterion to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// <param name="selectionOrder">The fields the repository should order by</param>
        public async Task<ICollection<T>> FindAllAsync(PagingInfo pagingInfo, Order[] selectionOrder, params ICriterion[] criterion)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria criteria = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criterion, selectionOrder);
                ICriteria countCriteria = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criterion, new Order[0]);

                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value,
                   new[]{
                       countCriteria.SetProjection(Projections.RowCount()),
                        criteria.SetFirstResult(pagingInfo.FirstResult).SetMaxResults(pagingInfo.MaxResults)});

                return await DoMultiCriteriaPaging<T>(crit, pagingInfo);
            }
        }

        /// <summary>
        /// Execute the named query and return all the results
        /// </summary>
        /// <param name="namedQuery">The named query to execute</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <returns>The results of the query</returns>
        public async Task<ICollection<T>> FindAllAsync(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                return await query.ListAsync<T>();
            }
        }

        /// <summary>
        /// Execute the named query and return paged results
        /// </summary>
        /// <param name="parameters">Parameters for the query</param>
        /// <param name="namedQuery">the query to execute</param>
        /// <param name="firstResult">The first result to return</param>
        /// <param name="numberOfResults">number of records to return</param>
        /// <returns>Paged results of the query</returns>
        public async Task<ICollection<T>> FindAllAsync(int firstResult, int numberOfResults, string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                query.SetFirstResult(firstResult)
                    .SetMaxResults(numberOfResults);
                return await query.ListAsync<T>();
            }
        }

        /// <summary>
        /// Find a single entity based on a criteria.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <returns>The entity or null</returns>
        public async Task<T> FindOneAsync(params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit =
                    RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, null);
                return await crit.UniqueResultAsync<T>();
            }
        }

        /// <summary>
        /// Find a single entity based on multi criteria.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <returns>The entity or null</returns>
        public async Task<T> FindOneAsync(params DetachedCriteria[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value, criteria);
                var result = (IList)(await crit.ListAsync())[0];

                //if(result.Count > 1)
                //    throw new NonUniqueResultException(result.Count);

                return (T)result[0];
            }
        }

        /// <summary>
        /// Find a single entity based on a criteria.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <returns>The entity or null</returns>
        public async Task<T> FindOneAsync(DetachedCriteria criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit =
                    RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                return await crit.UniqueResultAsync<T>();
            }
        }

        /// <summary>
        /// Find a single entity based on a named query.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="parameters">parameters for the query</param>
        /// <param name="namedQuery">the query to executre</param>
        /// <returns>The entity or null</returns>
        public async Task<T> FindOneAsync(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                return await query.UniqueResultAsync<T>();
            }
        }

        /// <summary>
        /// Counts the number of instances matching the criteria.
        /// </summary>
        public async Task<long> CountAsync(DetachedCriteria criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                crit.SetProjection(Projections.RowCount());
                object countMayBe_Int32_Or_Int64_DependingOnDatabase = await crit.UniqueResultAsync();
                return Convert.ToInt64(countMayBe_Int32_Or_Int64_DependingOnDatabase, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Find a single entity based on a named query.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="parameters">parameters for the query</param>
        /// <param name="namedQuery">the query to executre</param>
        /// <returns>The entity or null</returns>
        public async Task<long> CountAsync(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                return (await query.ListAsync()).Count;
            }
        }

        /// <summary>
        /// Counts the overall number of instances.
        /// </summary>
        public async Task<long> CountAsync()
        {
            DetachedCriteria criteria = null;
            return await CountAsync(criteria);
        }

        /// <summary>
        /// Check if any instance matches the criteria.
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsAsync(DetachedCriteria criteria)
        {
            return 0 != await CountAsync(criteria);
        }

        /// <summary>
        /// Check if any instance of the type exists
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsAsync()
        {
            return await ExistsAsync(null as DetachedCriteria);
        }

        /// <summary>
        /// Check if any instance matches the criteria.
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        public async Task<bool> ExistsAsync(params ICriterion[] criteria)
        {
            var detached = DetachedCriteria.For<T>();
            foreach (var crit in criteria)
                detached.Add(crit);

            return await CountAsync(detached) != 0;
        }
        private static ICollection<TProj> DoReportAll<TProj>(ICriteria criteria, ProjectionList projectionList, IResultTransformer resultTransformer)
        {
            BuildProjectionCriteria<TProj>(criteria, projectionList, true, resultTransformer);
            return criteria.List<TProj>();
        }

        private static ICollection<TProj> DoReportAll<TProj>(ICriteria criteria, ProjectionList projectionList)
        {
            return DoReportAll<TProj>(criteria, projectionList, false);
        }

        private static ICollection<TProj> DoReportAll<TProj>(ICriteria criteria, ProjectionList projectionList, bool distinctResults)
        {
            BuildProjectionCriteria<TProj>(criteria, projectionList, distinctResults, new TypedResultTransformer<TProj>());
            return criteria.List<TProj>();
        }

        private static ICriteria BuildProjectionCriteria<TProj>(ICriteria criteria, IProjection projectionList, bool distinctResults, IResultTransformer resultTransformer)
        {
            if (distinctResults)
                criteria.SetProjection(Projections.Distinct(projectionList));
            else
                criteria.SetProjection(projectionList);

            if (typeof(TProj) != typeof(object[]))
            //we are not returning a tuple, so we need the result transformer
            {
                criteria.SetResultTransformer(resultTransformer);
            }

            return criteria;
        }
        public IQueryOver<T> QueryOver()
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                return action.Value.QueryOver<T>();
            }
        }

        public IQueryOver<T, T> QueryOver(Expression<Func<T>> alias)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                return action.Value.QueryOver(alias);
            }
        }

        public IQueryable<T> Query()
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                return action.Value.Query<T>();
            }
        }

        private async Task<ICollection<TColItem>> DoMultiCriteriaPaging<TColItem>(IMultiCriteria criteria, PagingInfo pagingInfo)
        {
            IList results = await criteria.ListAsync();

            PaginatedList<TColItem> returnList = new PaginatedList<TColItem>();

            foreach (var t in (IList)results[1])
                returnList.Add((TColItem)t);

            pagingInfo.Total = (int)((IList)results[0])[0];

            returnList.SetPagingInfo(pagingInfo);

            return returnList;
        }

    }
}


