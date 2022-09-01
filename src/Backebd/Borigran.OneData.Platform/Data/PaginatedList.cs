using System.Collections.Generic;

namespace Borigran.OneData.Platform.Data
{
    public class PaginatedList<T> : List<T>, ICollection<T>
    {
        public PaginatedList(IEnumerable<T> collection, PagingInfo pagingInfo)
            : base(collection)
        {
            SetPagingInfo(pagingInfo);
        }

        public PaginatedList()
        {
        }

        public void SetPagingInfo(PagingInfo pagingInfo)
        {
            totalCount = pagingInfo.Total;
            currentPage = pagingInfo.PageNumber;
            pageSize = pagingInfo.MaxResults;
            pageCount = pagingInfo.PageCount;
        }

        private int totalCount;
        public int TotalCount
        {
            get { return totalCount; }
        }

        private int pageCount;
        public int PageCount
        {
            get { return pageCount; }
        }

        private int pageSize;
        public int PageSize
        {
            get { return pageSize; }
        }

        private int currentPage;
        public int CurrentPage
        {
            get { return currentPage; }
        }
    }
}
