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

        protected override ISessionFactory SessionFactory
        {
            get { return Session.GetSessionImplementation().Factory; }
        }

        public NHibernateRepository(ISession session)
        {
            this.session = session;
        }

        public async Task<T> GetAsync(object id)
        {
            return (T) await Session.GetAsync(typeof(T), id);
        }

        public async Task DeleteAsync(T entity)
        {
            await Session.DeleteAsync(entity);
        }

        public async Task<T> SaveAsync(T entity)
        {
            await Session.SaveAsync(entity);
            return entity;
        }

        public async Task<T> SaveOrUpdateAsync(T entity)
        {
            await Session.SaveOrUpdateAsync(entity);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            await Session.UpdateAsync(entity);
        }
    }
}
