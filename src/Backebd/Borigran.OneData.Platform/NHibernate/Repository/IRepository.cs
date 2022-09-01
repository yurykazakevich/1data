using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    /// <summary>
    /// <para>
    /// Primary repository interface, of which all repository implementation must implement.
    /// </para>
    /// <para>
    /// The interface is not generic or abstracted from the type of persistance mechanism
    /// employed within the implementation as it contains dependencies on nHibernate 
    /// and more specifically <see cref="ICriterion"/>. This is by design as seperating 
    /// out support for a theoretical switch to a different ORM solution was considered out of scope, although
    /// refactoring to this accomodate this scenario is certainly not out of reach if required in future.
    /// </para> 
    /// </summary>
    /// <typeparam name="T">The type of Entity or Value persistence object</typeparam>
    /// <seealso cref="Repository{T}"/>
    public interface IRepository<T> : IReadOnlyRepository<T> where T : class
    {
        /// <summary>
        /// Register the entity for deletion when the unit of work
        /// is completed. 
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Registers all entities for deletion when the unit of work
        /// is completed.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Registers all entities for deletion that match the supplied
        /// criteria condition when the unit of work is completed.
        /// </summary>
        /// <param name="where">criteria condition to select the rows to be deleted</param>
        void DeleteAll(DetachedCriteria where);

        /// <summary>
        /// Registers all entities for deletion that match the supplied
        /// named query when the unit of work is completed.
        /// </summary>
        /// <param name="namedQuery">The named query to execute</param>
        /// <param name="parameters">Parameters for the query</param>
        void DeleteAll(string namedQuery, params Parameter[] parameters);

        /// <summary>
        /// Register te entity for save in the database when the unit of work
        /// is completed. (INSERT)
        /// </summary>
        /// <param name="entity">the entity to save</param>
        /// <returns>The saved entity</returns>
        T Save(T entity);

        /// <summary>
        /// Saves or update the entity, based on its usaved-value
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved or updated entity</returns>
        T SaveOrUpdate(T entity);

        /// <summary>
        /// Saves or update the copy of entity, based on its usaved-value
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved entity</returns>
        T SaveOrUpdateCopy(T entity);

        /// <summary>
        /// Register the entity for update in the database when the unit of work
        /// is completed. (UPDATE)
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

        /// <summary>
        /// Update or delete using a named query
        /// </summary>
        /// <param name="namedQuery">the named query in mapping file</param>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterlist">parameter list </param>
        void UpdateOrDelete(string namedQuery, Tuple<string, IEnumerable> parameterlist = null, params Parameter[] parameters);

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="order"></param>
        /// <param name="criteria">the criteria to look for</param>
        /// <returns>All the entities that match the criteria</returns>
        ICollection<T> FindAll(Order order, params ICriterion[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="criteria">the criteria to look for</param>
        /// <param name="orders"> the order to load the entities</param>
        /// <returns>All the entities that match the criteria</returns>
        ICollection<T> FindAll(DetachedCriteria criteria, params Order[] orders);

        /// <summary>
        /// Loads all the entities that match the criteria(s)
        /// </summary>
        /// <param name="options">The result set options</param>
        /// <param name="criteria">the criteria(s) to look for</param>
        /// <returns>All the entities that match the criteria(s)</returns>
        ICollection<T> FindAll(ResultSetOptions options, params DetachedCriteria[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria(s), defaults to include duplicates
        /// </summary>
        /// <param name="criteria">the criteria(s) to look for</param>
        /// <returns>All the entities that match the criteria(s)</returns>
        ICollection<T> FindAll(DetachedCriteria[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="criteria">the criteria to look for</param>
        /// <param name="orders"> the order to load the entities</param>
        /// <param name="firstResult">the first result to load</param>
        /// <param name="maxResults">the number of result to load</param>
        /// <returns>All the entities that match the criteria</returns>
        ICollection<T> FindAll(DetachedCriteria criteria,
                               int firstResult, int maxResults,
                               params Order[] orders);

        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="criteria">the criteria to look for</param>
        /// <param name="pagingInfo">the paging information</param>
        /// <param name="orders"> the order to load the entities</param>
        /// <returns>All the entities that match the criteria</returns>
        ICollection<T> FindAll(DetachedCriteria criteria,
                               PagingInfo pagingInfo,
                               params Order[] orders);


        /// <summary>
        /// Loads all the entities that match the criteria
        /// by order
        /// </summary>
        /// <param name="orders"></param>
        /// <param name="criteria">the criteria to look for</param>
        /// <returns>All the entities that match the criteria</returns>
        ICollection<T> FindAll(Order[] orders, params ICriterion[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria
        /// </summary>
        /// <param name="criteria">the criteria to look for</param>
        /// <returns>All the entities that match the criteria</returns>
        ICollection<T> FindAll(params ICriterion[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria, and allow paging.
        /// </summary>
        /// <param name="firstResult">The first result to load</param>
        /// <param name="numberOfResults">Total number of results to load</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        ICollection<T> FindAll(int firstResult, int numberOfResults, params ICriterion[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria, and allow paging.
        /// </summary>
        /// <param name="pagingInfo">the paging information</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        ICollection<T> FindAll(PagingInfo pagingInfo, params ICriterion[] criteria);

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
        ICollection<T> FindAll(int firstResult, int numberOfResults,
                               Order selectionOrder,
                               params ICriterion[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria, with paging 
        /// and orderring by a single field.
        /// <param name="pagingInfo">the paging information</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// <param name="selectionOrder">The field the repository should order by</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// </summary>
        ICollection<T> FindAll(PagingInfo pagingInfo,
                               Order selectionOrder,
                               params ICriterion[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria, with paging 
        /// and orderring by a multiply fields.
        /// </summary>
        /// <param name="firstResult">The first result to load</param>
        /// <param name="numberOfResults">Total number of results to load</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// <param name="selectionOrder">The fields the repository should order by</param>
        ICollection<T> FindAll(int firstResult, int numberOfResults,
                               Order[] selectionOrder,
                               params ICriterion[] criteria);

        /// <summary>
        /// Loads all the entities that match the criteria, with paging 
        /// and orderring by a multiply fields.
        /// </summary>
        /// <param name="pagingInfo">the paging information</param>
        /// <param name="criteria">the cirteria to look for</param>
        /// <returns>number of Results of entities that match the criteria</returns>
        /// <param name="selectionOrder">The fields the repository should order by</param>
        ICollection<T> FindAll(PagingInfo pagingInfo,
                               Order[] selectionOrder,
                               params ICriterion[] criteria);


        /// <summary>
        /// Execute the named query and return all the results
        /// </summary>
        /// <param name="namedQuery">The named query to execute</param>
        /// <param name="parameters">Parameters for the query</param>
        /// <returns>The results of the query</returns>
        ICollection<T> FindAll(string namedQuery, params Parameter[] parameters);

        /// <summary>
        /// Execute the named query and return paged results
        /// </summary>
        /// <param name="parameters">Parameters for the query</param>
        /// <param name="namedQuery">the query to execute</param>
        /// <param name="firstResult">The first result to return</param>
        /// <param name="numberOfResults">number of records to return</param>
        /// <returns>Paged results of the query</returns>
        ICollection<T> FindAll(int firstResult,
            int numberOfResults, string namedQuery,
            params Parameter[] parameters);

        /// <summary>
        /// Find a single entity based on a criteria.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <returns>The entity or null</returns>
        T FindOne(params ICriterion[] criteria);

        /// <summary>
        /// Find a single entity based on a criteria.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <returns>The entity or null</returns>
        T FindOne(params DetachedCriteria[] criteria);

        /// <summary>
        /// Find a single entity based on a criteria.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <returns>The entity or null</returns>
        T FindOne(DetachedCriteria criteria);

        /// <summary>
        /// Find a single entity based on a named query.
        /// Thorws is there is more than one result.
        /// </summary>
        /// <param name="parameters">parameters for the query</param>
        /// <param name="namedQuery">the query to executre</param>
        /// <returns>The entity or null</returns>
        T FindOne(string namedQuery, params Parameter[] parameters);

        /// <summary>
        /// Find the entity based on a criteria.
        /// </summary>
        /// <param name="criteria">The criteria to look for</param>
        /// <param name="orders">Optional orderring</param>
        /// <returns>The entity or null</returns>
        T FindFirst(DetachedCriteria criteria, params Order[] orders);

        /// <summary>
        /// Find the first entity of type
        /// </summary>
        /// <param name="orders">Optional orderring</param>
        /// <returns>The entity or null</returns>
        T FindFirst(params Order[] orders);

        /// <summary>
        /// Check if any instance of the type exists
        /// </summary>
        /// <returns><c>true</c> if an instance is found; otherwise <c>false</c>.</returns>
        bool Exists();

        /// <summary>
        /// Counts the overall number of instances.
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Creates a <see cref="DetachedCriteria"/> compatible with this Repository
        /// </summary>
        /// <returns>The <see cref="DetachedCriteria"/></returns>
        DetachedCriteria CreateDetachedCriteria();

        /// <summary>
        /// Creates an aliases <see cref="DetachedCriteria"/> compatible with this Repository
        /// </summary>
        /// <param name="aliasName">the alias</param>
        /// <returns>The <see cref="DetachedCriteria"/></returns>
        DetachedCriteria CreateDetachedCriteria(string aliasName);

        /// <summary>
        /// Execute the specified stored procedure with the given parameters
        /// and return the result.
        /// Note: only scalar values are supported using this approach.
        /// </summary>
        /// <param name="storedProcName">The name of the stored procedure</param>
        /// <param name="parameters">parameters for the stored procedure</param>
        /// <returns>return value</returns>
        object ExecuteStoredProcedure(string storedProcName, params Parameter[] parameters);

    }
}
