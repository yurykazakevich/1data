namespace Borigran.OneData.Platform.NHibernate.Repository
{
    public enum ResultSetOptions
    {
        /// <summary>
        /// No change to result set, the result may contain duplicates
        /// </summary>
        None,
        /// <summary>
        /// The union of the result sets, the unique set (no duplicates)
        /// </summary>
        Union,
        /// <summary>
        /// The set of results common to all sets
        /// </summary>
        Intersect
    }
}
