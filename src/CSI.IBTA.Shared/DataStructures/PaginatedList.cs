namespace CSI.IBTA.Shared.DataStructures
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IQueryable<T> source, int pageIndex, int totalPages)
        {
            var items = source.ToList();
            PageIndex = pageIndex;
            TotalPages = totalPages;

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
