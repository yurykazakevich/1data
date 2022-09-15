using NHibernate;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Borigran.OneData.Platform.NHibernate.Repository
{
    public class NHibernateRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
    {
        private readonly ISession session;

        protected virtual ISession Session
        {
            get { return session; }
        }

        protected override DisposableAction<ISession> ActionToBePerformedOnSessionUsedForDBFetches => throw new NotImplementedException();

        /// <summary>
        /// Returns the nhibernate <see cref="ISessionFactory"/> associated with current unit of work's
        /// current nhibernate session, see <see cref="UnitOfWork.CurrentSession"/> and it's 
        /// GetSessionImplementation() method.
        /// </summary>
        protected override ISessionFactory SessionFactory
        {
            get { return Session.GetSessionImplementation().Factory; }
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
        public async Task<T> GetAsync(object id)
        {
            return (T) await Session.GetAsync(typeof(T), id);
        }

        /// <summary>
        /// Register the entity for deletion when the unit of work
        /// is completed. 
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public async Task DeleteAsync(T entity)
        {
            await Session.DeleteAsync(entity);
        }


        /// <summary>
        /// Register te entity for save in the database when the unit of work
        /// is completed. (INSERT)
        /// </summary>
        /// <param name="entity">the entity to save</param>
        /// <returns>The saved entity</returns>
        public async Task<T> SaveAsync(T entity)
        {
            await Session.SaveAsync(entity);
            return entity;
        }

        /// <summary>
        /// Saves or update the entity, based on its usaved-value
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved or updated entity</returns>
        public async Task<T> SaveOrUpdateAsync(T entity)
        {
            await Session.SaveOrUpdateAsync(entity);
            return entity;
        }

        /// <summary>
        /// Register the entity for update in the database when the unit of work
        /// is completed. (UPDATE)
        /// </summary>
        /// <param name="entity"></param>
        public async Task UpdateAsync(T entity)
        {
            await Session.UpdateAsync(entity);
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
