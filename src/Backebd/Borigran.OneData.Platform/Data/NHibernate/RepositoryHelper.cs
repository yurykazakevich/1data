using System.Collections;
using System.Data;
using NHibernate;
using NHibernate.Criterion;

namespace Borigran.OneData.Platform.Data.NHibernate
{
    internal class RepositoryHelper<T>
    {
        private RepositoryHelper() { }


        internal static IQuery CreateQuery(ISession session, string namedQuery, Parameter[] parameters)
        {
            IQuery query = session.GetNamedQuery(namedQuery);
            foreach (Parameter parameter in parameters)
            {
                if (parameter.Type == null)
                {
                    if (!parameter.ValueIsIEnumerable || parameter.Value == null)
                        query.SetParameter(parameter.Name, parameter.Value);
                    else
                        query.SetParameterList(parameter.Name, (IEnumerable)parameter.Value);
                }
                else
                {
                    if (!parameter.ValueIsIEnumerable || parameter.Value == null)
                        query.SetParameter(parameter.Name, parameter.Value, parameter.Type);
                    else
                        query.SetParameterList(parameter.Name, (IEnumerable)parameter.Value, parameter.Type);
                }

            }

            return query;
        }

        public static ICriteria GetExecutableCriteria(ISession session, DetachedCriteria criteria, Order[] orders)
        {
            ICriteria executableCriteria;
            if (criteria != null)
            {
                executableCriteria = criteria.GetExecutableCriteria(session);
            }
            else
            {
                executableCriteria = session.CreateCriteria(typeof(T));
            }

            AddOrders(executableCriteria, orders);

            return executableCriteria;
        }

        public static IMultiCriteria GetExecutableMultiCriteria(ISession session, DetachedCriteria[] criteria)
        {
            IMultiCriteria executableCriteria = session.CreateMultiCriteria();
            foreach (DetachedCriteria dc in criteria)
            {
                executableCriteria.Add(dc);
            }

            return executableCriteria;
        }

        public static IMultiCriteria GetExecutableMultiCriteria(ISession session, ICriteria[] criteria)
        {
            IMultiCriteria executableCriteria = session.CreateMultiCriteria();
            foreach (ICriteria dc in criteria)
            {
                executableCriteria.Add(dc);
            }

            return executableCriteria;
        }

        public static ICriteria CreateCriteriaFromArray(ISession session, ICriterion[] criteria, Order[] orders)
        {
            ICriteria crit = session.CreateCriteria(typeof(T));
            foreach (ICriterion criterion in criteria)
            {
                //allow some fancy antics like returning possible return 
                // or null to ignore the criteria
                if (criterion == null)
                    continue;
                crit.Add(criterion);
            }

            AddOrders(crit, orders);

            return crit;
        }

        public static void AddOrders(ICriteria crit, Order[] orders)
        {
            if (orders != null)
            {
                foreach (Order order in orders)
                {
                    crit.AddOrder(order);
                }
            }
        }

        public static void AddOrders(DetachedCriteria crit, Order[] orders)
        {
            if (orders != null)
            {
                foreach (Order order in orders)
                {
                    crit.AddOrder(order);
                }
            }
        }

        public static void CreateDbDataParameters(IDbCommand command, Parameter[] parameters)
        {
            foreach (Parameter parameter in parameters)
            {
                IDbDataParameter sp_arg = command.CreateParameter();
                sp_arg.ParameterName = parameter.Name;
                sp_arg.Value = parameter.Value;
                command.Parameters.Add(sp_arg);
            }
        }
    }
}
