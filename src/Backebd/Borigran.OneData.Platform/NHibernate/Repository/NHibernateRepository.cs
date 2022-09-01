using NHibernate;
using NHibernate.Criterion;
using NHibernate.OData;
using NHibernate.SqlCommand;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    public class NHibernateRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
    {
        private readonly ISession session;

        protected virtual ISession Session
        {
            get { return session; }
        }

        public NHibernateRepository(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Get the entity from the persistance store, or return null
        /// if it doesn't exist.
        /// </summary>
        /// <param name="id">The entity's id</param>
        /// <returns>Either the entity that matches the id, or a null</returns>
        public T Get(object id)
        {
            return (T)Session.Get(typeof(T), id);
        }

        /// <summary>
        /// Load the entity from the persistance store
        /// Will throw an exception if there isn't an entity that matches
        /// the id.
        /// </summary>
        /// <param name="id">The entity's id</param>
        /// <returns>The entity that matches the id</returns>
        public T Load(object id)
        {
            return (T)Session.Load(typeof(T), id);
        }

        /// <summary>
        /// Register the entity for deletion when the unit of work
        /// is completed. 
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public void Delete(T entity)
        {
            Session.Delete(entity);
        }

        /// <summary>
        /// Registers all entities for deletion when the unit of work
        /// is completed.
        /// </summary>
        public void DeleteAll()
        {
            Session.Delete(String.Format(CultureInfo.InvariantCulture, "from {0}", typeof(T).Name));
        }

        /// <summary>
        /// Registers all entities for deletion that match the supplied
        /// named query when the unit of work is completed.
        /// </summary>
        /// <param name="namedQuery">The named query to execute</param>
        /// <param name="parameters">Parameters for the query</param>
        public void DeleteAll(string namedQuery, params Parameter[] parameters)
        {
            IQuery query = RepositoryHelper<T>.CreateQuery(Session, namedQuery, parameters);

            foreach (T entity in query.List<T>())
            {
                Session.Delete(entity);
            }
        }

        /// <summary>
        /// Registers all entities for deletion that match the supplied
        /// criteria condition when the unit of work is completed.
        /// </summary>
        /// <param name="where">criteria condition to select the rows to be deleted</param>
        public void DeleteAll(DetachedCriteria where)
        {
            foreach (object entity in where.GetExecutableCriteria(Session).List())
            {
                Session.Delete(entity);
            }
        }

        /// <summary>
        /// Register te entity for save in the database when the unit of work
        /// is completed. (INSERT)
        /// </summary>
        /// <param name="entity">the entity to save</param>
        /// <returns>The saved entity</returns>
        public T Save(T entity)
        {
            Session.Save(entity);
            return entity;
        }

        /// <summary>
        /// Saves or update the entity, based on its usaved-value
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved or updated entity</returns>
        public T SaveOrUpdate(T entity)
        {
            Session.SaveOrUpdate(entity);
            return entity;
        }

        /// <summary>
        /// Saves or update the copy of entity, based on its usaved-value
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved entity</returns>
        public T SaveOrUpdateCopy(T entity)
        {
            return (T)Session.Merge(entity);
        }

        /// <summary>
        /// Register the entity for update in the database when the unit of work
        /// is completed. (UPDATE)
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            Session.Update(entity);
        }

        /// <summary>
        /// Update or delete using a named query
        /// </summary>
        /// <param name="namedQuery">the named query in mapping file</param>
        /// <param name="parameters">parameters</param>
        /// <param name="parameterlist"> </param>
        public void UpdateOrDelete(string namedQuery, Tuple<string, IEnumerable> parameterlist = null, params Parameter[] parameters)
        {
            IQuery query = RepositoryHelper<T>.CreateQuery(Session, namedQuery, parameters ?? new Parameter[0]);

            if (parameterlist != null)
                query.SetParameterList(parameterlist.Item1, parameterlist.Item2);

            query.ExecuteUpdate();
        }
    }
}
