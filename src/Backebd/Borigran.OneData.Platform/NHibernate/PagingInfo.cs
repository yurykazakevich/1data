namespace Borigran.OneData.Platform.NHibernate
{
    public class PagingInfo
    {
        private readonly int pageNumber;
        private readonly int pageSize;
        private readonly bool isPageNumberZeroBased;
        private int total;

        /// <summary>
        /// <c>true</c> is the <see cref="PageNumber"/> starts at zero, <c>false</c> if it
        /// starts at one.
        /// </summary>
        public bool IsPageNumberZeroBased
        {
            get { return isPageNumberZeroBased; }
        }

        /// <summary>
        /// The first result to be returned which is a zero based row count
        /// to the very first result to be included.
        /// </summary>
        public int FirstResult
        {
            get
            {
                if (isPageNumberZeroBased)
                    return (pageSize * pageNumber);
                return (pageSize * (pageNumber - 1));
            }
        }

        public int PageCount
        {
            get
            {
                return (Total < PageSize) ? 1 : (Total + PageSize - 1) / PageSize;
            }

        }

        public int MaxResults
        {
            get { return pageSize; }
        }

        /// <summary>
        /// This property indicates the total number of records for the result set
        /// and can be used to retrieve this value after passing an instance of this class
        /// to a repository or service method.
        /// </summary>
        public int Total
        {
            get { return total; }
            set { total = value; }
        }

        /// <summary>
        /// This is number of page sets.
        /// </summary>
        public int PageNumber
        {
            get { return pageNumber; }
        }

        /// <summary>
        /// This is the maximum number of results that should be returned
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
        }

        private PagingInfo(bool isPageNumberZeroBased)
        {
            this.isPageNumberZeroBased = isPageNumberZeroBased;
        }

        public PagingInfo(int pageNumber, int pageSize)
            : this(false)
        {
            Check.IsTrue(pageNumber != 0, "pageNumber cannot be zero. Use another constructor instead.");
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }

        public PagingInfo(bool isPageNumberZeroBased, int pageNumber, int pageSize)
            : this(isPageNumberZeroBased)
        {
            this.pageNumber = pageNumber;
            this.pageSize = pageSize;
        }
    }
}
