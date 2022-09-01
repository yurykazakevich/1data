using NHibernate.Transform;
using System;
using System.Collections;

namespace Borigran.OneData.Platform.Data.NHibernate
{
    /// <summary>
    /// This is used to convert resulting tuples into strongly typed objects.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    public class TypedResultTransformer<T1> : IResultTransformer
    {
        public object TransformTuple(object[] tuple, string[] aliases)
        {
            if (tuple.Length == 1)
            {
                return tuple[0];
            }

            //for (var i = 0; i < tuple.Length; i++)
            //{
            //    Debug.WriteLine(string.Format("{0}: {1}, {2}, {3}", i, aliases[i], tuple[i], tuple[i] != null ? tuple[i].GetType().FullName:null));
            //}

            return Activator.CreateInstance(typeof(T1), tuple);
        }

        IList IResultTransformer.TransformList(IList collection)
        {
            return collection;
        }
    }
}
