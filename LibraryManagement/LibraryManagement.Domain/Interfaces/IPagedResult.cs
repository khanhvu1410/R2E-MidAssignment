namespace LibraryManagement.Domain.Interfaces
{
    public interface IPagedResult<T>
    {
        public IEnumerable<T>? Items { get; }

        public int PageIndex { get; }

        public int TotalPages { get; }

        public bool HasPreviousPage {  get; }  

        public bool HasNextPage { get; }
    }
}
