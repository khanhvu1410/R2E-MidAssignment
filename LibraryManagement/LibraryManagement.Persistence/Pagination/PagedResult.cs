using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Persistence.Pagination
{
    internal class PagedResult<T> : IPagedResult<T>
    {
        public IEnumerable<T>? Items { get; }

        public int PageIndex { get; }

        public int TotalPages { get; }
       
        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public PagedResult(IEnumerable<T> items, int pageIndex, int totalPages)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
            Items = items;
        }
    }
}
