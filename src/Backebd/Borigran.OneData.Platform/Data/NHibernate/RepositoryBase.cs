using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Borigran.OneData.Platform.Data.NHibernate
{
    public class RepositoryBase<T> where T : class
    {
        private static readonly Order[] NullOrderArray = null;

        /// <summary>
        /// Creates a <see cref="DetachedCriteria"/> compatible with this Repository
        /// </summary>
        /// <returns>The <see cref="DetachedCriteria"/></returns>
        public DetachedCriteria CreateDetachedCriteria()
        {
            return DetachedCriteria.For<T>();
        }


        /// <summary>
        /// Creates an aliases <see cref="DetachedCriteria"/> compatible with this Repository
        /// </summary>
        /// <param name="alias">the alias</param>
        /// <returns>The <see cref="DetachedCriteria"/></returns>
        public DetachedCriteria CreateDetachedCriteria(string alias)
        {
            return DetachedCriteria.For<T>(alias);
        }

        /// <summary>
        /// Loads all the entities that match the criteria
        /// </summary>
        /// <param name="criteria">the criteria to look for</param>
        /// <returns>All the entities that match the criteria</returns>
        public ICollection<T> FindAll(params ICriterion[] criteria)
        {
            return FindAll(NullOrderArray, criteria);
        }

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="order">the order in which to bring the data</param>
        /// <param name="criteria">the criteria to look for</param>
        /// <returns>All the entities that match the criteria</returns>
        public ICollection<T> FindAll(Order order, params ICriterion[] criteria)
        {
            return FindAll(new Order[] { order }, criteria);
        }

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="orders">the order in which to bring the data</param>
        /// <param name="criteria">the criteria to look for</param>
        /// <returns>All the entities that match the criteria</returns>
        public ICollection<T> FindAll(Order[] orders, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, orders);
                return crit.List<T>();
            }
        }

        public ICollection<T> FindAll(DetachedCriteria[] criteria)
        {
            return FindAll(ResultSetOptions.None, criteria);
        }

        public ICollection<T> FindAll(ResultSetOptions options, params DetachedCriteria[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value, criteria);
                IEnumerable<T> result = new List<T>();
                bool first = true;
                foreach (IList resultItem in crit.List())
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

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="criteria">the criteria to look for</param>
        /// <param name="orders"> the order to load the entities</param>
        /// <returns>All the entities that match the criteria</returns>
        public ICollection<T> FindAll(DetachedCriteria criteria, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, orders);
                return crit.List<T>();
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
        public ICollection<T> FindAll(DetachedCriteria criteria, int firstResult, int maxResults, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, orders);
                crit.SetFirstResult(firstResult)
                    .SetMaxResults(maxResults);
                return crit.List<T>();
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
        public ICollection<T> FindAll(DetachedCriteria criteria, IPagingInfo pagingInfo, params Order[] orders)
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

                return DoMultiCriteriaPaging<T>(crit, pagingInfo);
            }
        }

        /// <summary>
        /// Loads all the entities that match the criteria, and allow paging.
        /// </summary>
        /// <param name="firstResult">The first result to load</param>
        /// <param name="numberOfResults">Total number of results to load</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        public ICollection<T> FindAll(int firstResult, int numberOfResults, params ICriterion[] criteria)
        {
            return FindAll(firstResult, numberOfResults, NullOrderArray, criteria);
        }

        /// <summary>
        /// Loads all the entities that match the criteria, and allow paging.
        /// </summary>
        /// <param name="pagingInfo">The paging information</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        public IPaginatedCollection<T> FindAll(IPagingInfo pagingInfo, params ICriterion[] criteria)
        {
            return FindAll(pagingInfo, NullOrderArray, criteria);
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
        public ICollection<T> FindAll(int firstResult, int numberOfResults, Order selectionOrder, params ICriterion[] criteria)
        {
            return FindAll(firstResult, numberOfResults, new Order[] { selectionOrder }, criteria);
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
        public IPaginatedCollection<T> FindAll(IPagingInfo pagingInfo, Order selectionOrder, params ICriterion[] criteria)
        {
            return FindAll(pagingInfo, new Order[] { selectionOrder }, criteria);
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
        public ICollection<T> FindAll(int firstResult, int numberOfResults, Order[] selectionOrder, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, selectionOrder);
                crit.SetFirstResult(firstResult)
                    .SetMaxResults(numberOfResults);
                return crit.List<T>();
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
        public IPaginatedCollection<T> FindAll(IPagingInfo pagingInfo, Order[] selectionOrder, params ICriterion[] criterion)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria criteria = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criterion, selectionOrder);
                ICriteria countCriteria = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criterion, new Order[0]);

                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value,
                   new[]{
                       countCriteria.SetProjection(Projections.RowCount()),
                        criteria.SetFirstResult(pagingInfo.FirstResult).SetMaxResults(pagingInfo.MaxResults)});

                return DoMultiCriteriaPaging<T>(crit, pagingInfo);
            }
        }

        /// <summary>
        /// Execute the named query and return all the results
        /// </summary>
        /// <param name="namedQuery">The named query to execute</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <returns>The results of the query</returns>
        public ICollection<T> FindAll(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                return query.List<T>();
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
        public ICollection<T> FindAll(int firstResult, int numberOfResults, string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                query.SetFirstResult(firstResult)
                    .SetMaxResults(numberOfResults);
                return query.List<T>();
            }
        }

        /// <summary>
        /// Find a single entity based on a criteria.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <returns>The entity or null</returns>
        public T FindOne(params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit =
                    RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, null);
                return crit.UniqueResult<T>();
            }
        }

        /// <summary>
        /// Find a single entity based on multi criteria.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <returns>The entity or null</returns>
        public T FindOne(params DetachedCriteria[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value, criteria);
                var result = (IList)crit.List()[0];

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
        public T FindOne(DetachedCriteria criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit =
                    RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                return crit.UniqueResult<T>();
            }
        }

        /// <summary>
        /// Find a single entity based on a named query.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="parameters">parameters for the query</param>
        /// <param name="namedQuery">the query to executre</param>
        /// <returns>The entity or null</returns>
        public T FindOne(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                return query.UniqueResult<T>();
            }
        }

        /// <summary>
        /// Find the first entity of type
        /// </summary>
        /// <param name="orders">Optional ordering</param>
        /// <returns>The entity or null</returns>
        public T FindFirst(params Order[] orders)
        {
            return FindFirst(null, orders);
        }

        /// <summary>
        /// Find the entity based on a criteria.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <param name="orders">Optional orderring</param>
        /// <returns>The entity or null</returns>
        public T FindFirst(DetachedCriteria criteria, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, orders);
                crit.SetFirstResult(0);
                crit.SetMaxResults(1);
                return (T)crit.UniqueResult();
            }
        }

        public TProj ReportOne<TProj>(DetachedCriteria criteria, ProjectionList projectionList, IResultTransformer transformer)
        {
            using (var action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                var crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                return DoReportOne<TProj>(crit, projectionList, transformer);
            }
        }

        public TProj ReportOne<TProj>(DetachedCriteria criteria, ProjectionList projectionList)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                return DoReportOne<TProj>(crit, projectionList);
            }
        }

        public TProj ReportOne<TProj>(ProjectionList projectionList, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, null);
                return DoReportOne<TProj>(crit, projectionList);
            }
        }

        public TProj ReportOne<TProj>(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                if (typeof(TProj) != typeof(object[]))
                {
                    query.SetResultTransformer(new TypedResultTransformer<TProj>());
                }

                query.SetFetchSize(1);

                return query.UniqueResult<TProj>();
            }
        }

        private static TProj DoReportOne<TProj>(ICriteria criteria, ProjectionList projectionList, IResultTransformer resultTransformer)
        {
            BuildProjectionCriteria<TProj>(criteria, projectionList, true, resultTransformer);
            return criteria.UniqueResult<TProj>();
        }

        private static TProj DoReportOne<TProj>(ICriteria criteria, ProjectionList projectionList)
        {
            return DoReportOne<TProj>(criteria, projectionList, new TypedResultTransformer<TProj>());
        }

        public ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, null, null);
                return DoReportAll<TProj>(crit, projectionList);
            }

        }

        public ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, null, orders);
                return DoReportAll<TProj>(crit, projectionList);
            }
        }

        public ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList, bool distinctResults)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, null, null);
                return DoReportAll<TProj>(crit, projectionList, distinctResults);
            }
        }

        public ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, IResultTransformer transformer)
        {
            using (var action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                var crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                return DoReportAll<TProj>(crit, projectionList, transformer);
            }
        }

        public ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList)
        {
            return ReportAll<TProj>(criteria, projectionList, false);
        }

        public ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, bool distinctResults)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                return DoReportAll<TProj>(crit, projectionList, distinctResults);
            }
        }

        public ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, int firstResult, int numberOfResults)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                crit.SetFirstResult(firstResult)
                    .SetMaxResults(numberOfResults);
                return DoReportAll<TProj>(crit, projectionList);
            }
        }

        public IPaginatedCollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, IPagingInfo pagingInfo)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                var countCriteria = CriteriaTransformer.Clone(criteria).SetProjection(Projections.RowCount());

                var projCriteria = BuildProjectionCriteria<TProj>(criteria, projectionList, false)
                    .SetFirstResult(pagingInfo.FirstResult).SetMaxResults(pagingInfo.MaxResults);

                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value, new[] { countCriteria, projCriteria });

                return DoMultiCriteriaPaging<TProj>(crit, pagingInfo);

            }
        }

        public ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, int firstResult, int numberOfResults, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, orders);
                crit.SetFirstResult(firstResult)
                    .SetMaxResults(numberOfResults);
                return DoReportAll<TProj>(crit, projectionList);
            }
        }

        public IPaginatedCollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, IPagingInfo pagingInfo, params Order[] orders)
        {
            return ReportAll<TProj>(criteria, projectionList, pagingInfo, false, orders);
        }

        public IPaginatedCollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, IPagingInfo pagingInfo, bool distinct, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IProjection countProjection = distinct ? Projections.CountDistinct("Id") : Projections.RowCount();

                var countCriteria = CriteriaTransformer.Clone(criteria).SetProjection(countProjection);

                var projCriteria = BuildProjectionCriteria<TProj>(criteria, projectionList, distinct)
                    .SetFirstResult(pagingInfo.FirstResult).SetMaxResults(pagingInfo.MaxResults);

                RepositoryHelper<T>.AddOrders(projCriteria, orders);

                IMultiCriteria crit = RepositoryHelper<T>.GetExecutableMultiCriteria(action.Value, new[] { countCriteria, projCriteria });

                return DoMultiCriteriaPaging<TProj>(crit, pagingInfo);
            }
        }

        protected abstract DisposableAction<ISession> ActionToBePerformedOnSessionUsedForDBFetches
        {
            get;
        }

        protected abstract ISessionFactory SessionFactory { get; }

        public ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, params Order[] orders)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, orders);
                return DoReportAll<TProj>(crit, projectionList);
            }
        }

        public ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, null);
                return DoReportAll<TProj>(crit, projectionList);
            }
        }

        public ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList, Order[] orders, params ICriterion[] criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.CreateCriteriaFromArray(action.Value, criteria, orders);
                return DoReportAll<TProj>(crit, projectionList);
            }
        }


        public ICollection<TProj> ReportAll<TProj>(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                if (typeof(TProj) != typeof(object[]))
                {
                    query.SetResultTransformer(new TypedResultTransformer<TProj>());
                }
                return query.List<TProj>();
            }
        }

        /// <summary>
        /// Counts the number of instances matching the criteria.
        /// </summary>
        public long Count(DetachedCriteria criteria)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                ICriteria crit = RepositoryHelper<T>.GetExecutableCriteria(action.Value, criteria, null);
                crit.SetProjection(Projections.RowCount());
                object countMayBe_Int32_Or_Int64_DependingOnDatabase = crit.UniqueResult();
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
        public long Count(string namedQuery, params Parameter[] parameters)
        {
            using (DisposableAction<ISession> action = ActionToBePerformedOnSessionUsedForDBFetches)
            {
                IQuery query = RepositoryHelper<T>.CreateQuery(action.Value, namedQuery, parameters);
                return query.List().Count;
            }
        }

        /// <summary>
        /// Counts the overall number of instances.
        /// </summary>
        public long Count()
        {
            DetachedCriteria criteria = null;
            return Count(criteria);
        }

        /// <summary>
        /// Check if any instance matches the criteria.
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        public bool Exists(DetachedCriteria criteria)
        {
            return 0 != Count(criteria);
        }

        /// <summary>
        /// Check if any instance of the type exists
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        public bool Exists()
        {
            return Exists(null as DetachedCriteria);
        }

        /// <summary>
        /// Check if any instance matches the criteria.
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        public bool Exists(params ICriterion[] criteria)
        {
            var detached = DetachedCriteria.For<T>();
            foreach (var crit in criteria)
                detached.Add(crit);

            return Count(detached) != 0;
        }

        private static IPaginatedCollection<TColItem> DoMultiCriteriaPaging<TColItem>(IMultiCriteria criteria, IPagingInfo pagingInfo)
        {
            IList results = (IList)criteria.List();

            PaginatedList<TColItem> returnList = new PaginatedList<TColItem>();

            foreach (var t in (IList)results[1])
                returnList.Add((TColItem)t);

            pagingInfo.Total = (int)((IList)results[0])[0];

            returnList.SetPagingInfo(pagingInfo);

            return returnList;
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

        private static DetachedCriteria BuildProjectionCriteria<TProj>(DetachedCriteria criteria, IProjection projectionList, bool distinctResults)
        {
            if (distinctResults)
                criteria.SetProjection(Projections.Distinct(projectionList));
            else
                criteria.SetProjection(projectionList);

            if (typeof(TProj) != typeof(object[]))
            //we are not returning a tuple, so we need the result transformer
            {
                criteria.SetResultTransformer(new TypedResultTransformer<TProj>());
            }

            return criteria;
        }

        public object ExecuteStoredProcedure(string storedProcName, params Parameter[] parameters)
        {
            IDbConnection connection = ((ISessionFactoryImplementor)SessionFactory).ConnectionProvider.GetConnection();
            try
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = storedProcName;
                    command.CommandType = CommandType.StoredProcedure;

                    RepositoryHelper<T>.CreateDbDataParameters(command, parameters);

                    return command.ExecuteScalar();
                }
            }
            finally
            {
                ((ISessionFactoryImplementor)SessionFactory).ConnectionProvider.CloseConnection(connection);
            }
        }


        public ICollection<T2> ExecuteStoredProcedure<T2>(string storedProcName, params Parameter[] parameters)
        {
            return ExecuteStoredProcedure<T2>(StoredProcedureResultMapper.Map<T2>, storedProcName, parameters);
        }

        /// <summary>
        /// Execute the specified stored procedure with the given parameters and then converts
        /// the results using the supplied delegate.
        /// </summary>
        /// <typeparam name="T2">The collection type to return.</typeparam>
        /// <param name="converter">The delegate which converts the raw results.</param>
        /// <param name="storedProcName">The name of the stored procedure.</param>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns></returns>
        public ICollection<T2> ExecuteStoredProcedure<T2>(Converter<IDataReader, T2> converter, string storedProcName,
                                                          params Parameter[] parameters)
        {
            IDbConnection connection = ((ISessionFactoryImplementor)SessionFactory).ConnectionProvider.GetConnection();

            try
            {
                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = storedProcName;
                    command.CommandType = CommandType.StoredProcedure;

                    RepositoryHelper<T>.CreateDbDataParameters(command, parameters);
                    IDataReader reader = command.ExecuteReader();
                    ICollection<T2> results = new List<T2>();

                    while (reader.Read())
                        results.Add(converter(reader));

                    reader.Close();

                    return results;
                }
            }
            finally
            {
                ((ISessionFactoryImplementor)SessionFactory).ConnectionProvider.CloseConnection(connection);
            }
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

    }


    public class StoredProcedureResultMapper
    {
        public static T2 Map<T2>(IDataReader dataReader)
        {
            var fields = new object[dataReader.FieldCount];
            for (var i = 0; i < dataReader.FieldCount; i++)
                fields[i] = dataReader[i] == DBNull.Value ? null : dataReader[i];

            return (T2)Activator.CreateInstance(typeof(T2), fields);
        }
    }


}


