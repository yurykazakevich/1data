using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    public interface IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// Execute the specified stored procedure with the given parameters and then converts
        /// the results using the supplied delegate.
        /// </summary>
        /// <typeparam name="T2">The collection type to return.</typeparam>
        /// <param name="converter">The delegate which converts the raw results.</param>
        /// <param name="storedProcName">The name of the stored procedure.</param>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <returns></returns>
        ICollection<T2> ExecuteStoredProcedure<T2>(Converter<IDataReader, T2> converter, string storedProcName,
                                                   params Parameter[] parameters);

        ICollection<T2> ExecuteStoredProcedure<T2>(string storedProcName, params Parameter[] parameters);

        /// <summary>
        /// Check if any instance matches the criteria.
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        bool Exists(DetachedCriteria criteria);

        /// <summary>
        /// Check if any instance matches the criteria.
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        bool Exists(params ICriterion[] criteria);

        /// <summary>
        /// Counts the number of instances matching the criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        long Count(DetachedCriteria criteria);

        /// <summary>
        /// Execute the named query and return the number of matching criteria
        /// </summary>
        /// <param name="namedQuery">The named query to execute</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <returns>The results of the query</returns>
        long Count(string namedQuery, params Parameter[] parameters);

        /// <summary>
        /// Create the project of type <typeparamref name="TProj"/> (ie a
        /// DataTransferObject) that satisfies the criteria supplied. Throws a
        /// NHibernate.NonUniqueResultException if there is more than one
        /// result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <param name="projectionList">Maps the properties from the object 
        /// graph satisfiying <paramref name="criteria"/>  to the DTO 
        /// <typeparamref name="TProj"/></param>
        /// <returns>The DTO or null</returns>
        /// <remarks>
        /// The intent is for <paramref name="criteria"/> to be based (rooted)
        /// on <typeparamref name="T"/>. This is not enforced but is a
        /// convention that should be followed
        /// </remarks>
        TProj ReportOne<TProj>(DetachedCriteria criteria, ProjectionList projectionList);

        TProj ReportOne<TProj>(string namedQuery, params Parameter[] parameters);

        TProj ReportOne<TProj>(DetachedCriteria criteria, ProjectionList projectionList, IResultTransformer transformer);

        /// <summary>
        /// <seealso cref="ReportOne{TProj}(NHibernate.Criterion.DetachedCriteria,NHibernate.Criterion.ProjectionList)"/>
        /// </summary>
        TProj ReportOne<TProj>(ProjectionList projectionList, params ICriterion[] criteria);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList);

        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, IResultTransformer resultTransformer);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList,
                                            bool distinctResults);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// Includes additional paging behaviour
        /// <param name="firstResult">The first result to load</param>
        /// <param name="numberOfResults">Total number of results to load</param>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, int firstResult,
                                            int numberOfResults);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// Includes additional paging behaviour
        /// <param name="pagingInfo">The paging information</param>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, PagingInfo pagingInfo);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// Includes additional paging behaviour and additional sorting parameters
        /// <param name="firstResult">The first result to load</param>
        /// <param name="numberOfResults">Total number of results to load</param>
        /// <param name="orders">The fields the repository should order by</param>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, int firstResult,
                                            int numberOfResults, params Order[] orders);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// Includes additional paging behaviour and additional sorting parameters
        /// <param name="pagingInfo">The paging information</param>
        /// <param name="orders">The fields the repository should order by</param>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList, PagingInfo pagingInfo, params Order[] orders);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// Includes additional paging behaviour and additional sorting parameters
        /// <param name="pagingInfo">The paging information</param>
        /// <param name="orders">The fields the repository should order by</param>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria, ProjectionList projectionList,
                                                     PagingInfo pagingInfo, bool distinct, params Order[] orders);

        /// <summary>
        /// Create the projects of type <typeparamref name="TProj"/> (ie
        /// DataTransferObject(s)) that satisfies the criteria supplied.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <param name="projectionList">Maps the properties from the object 
        /// graph satisfiying <paramref name="criteria"/>  to the DTO 
        /// <typeparamref name="TProj"/></param>
        /// <param name="orders">The fields the repository should order by</param>
        /// <returns>The projection result (DTO's) built from the object graph 
        /// satisfying <paramref name="criteria"/></returns>
        /// <remarks>
        /// The intent is for <paramref name="criteria"/> to be based (rooted)
        /// on <typeparamref name="T"/>. This is not enforced but is a
        /// convention that should be followed
        /// </remarks>
        ICollection<TProj> ReportAll<TProj>(DetachedCriteria criteria,
                                            ProjectionList projectionList,
                                            params Order[] orders);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList,
                                            params ICriterion[] criterion);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList,
                                            Order[] orders,
                                            params ICriterion[] criteria);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList, params Order[] orders);

        /// <summary>
        /// <seealso cref="ReportAll{TProj}(DetachedCriteria,ProjectionList,Order[])"/>
        /// </summary>
        ICollection<TProj> ReportAll<TProj>(ProjectionList projectionList, bool distinctResults);

        /// <summary>
        /// Execute the named query and return all the resulting DTO's
        /// (projection)
        /// <seealso cref="ReportAll{TProj}(NHibernate.Criterion.DetachedCriteria,NHibernate.Criterion.ProjectionList,NHibernate.Criterion.Order[])"/>
        /// </summary>
        /// <typeparam name="TProj">the type returned</typeparam>
        /// <param name="namedQuery">the query to execute in the *.hbm 
        /// mapping files</param>
        /// <param name="parameters">parameters for the query</param>
        ICollection<TProj> ReportAll<TProj>(string namedQuery, params Parameter[] parameters);

        /// <summary>
        /// Get the entity from the persistance store, or return null
        /// if it doesn't exist.
        /// </summary>
        /// <param name="id">The entity's id</param>
        /// <returns>Either the entity that matches the id, or a null</returns>
        T Get(object id);

        /// <summary>
        /// Load the entity from the persistance store
        /// Will throw an exception if there isn't an entity that matches
        /// the id.
        /// </summary>
        /// <param name="id">The entity's id</param>
        /// <returns>The entity that matches the id</returns>
        T Load(object id);

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

        /// <summary>
        /// Read the unique checksum value of the object
        /// </summary>
        /// <param name="id">The entity's id</param>
        /// <returns>Checksum</returns>
        long Checksum(object id);

        IEnumerable<T> ODataQuery(string query, ICriterion[] additionalFilters = null);
        IEnumerable<T> ODataQuery(string query, Dictionary<string, string> aliases, ICriterion[] additionalFilters = null);

        IEnumerable<T> ODataQueryWithInlineCount(string query, out int count, ICriterion[] additionalFilters = null);
        IEnumerable<T> ODataQueryWithInlineCount(string query, out int count, IEnumerable<AliasDefinition> aliases, ICriterion[] additionalFilters = null);
        IEnumerable<T> ODataQueryWithInlineCount(string query, out int count, Dictionary<string, string> aliases, ICriterion[] additionalFilters = null);

        ICriteria ODataQueryCriteria(string query, int maxResults = 500);
    }
}
