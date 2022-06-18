namespace StepEbay.Common.Models.Pagination
{
    public class PaginatedList<T>
    {
        public List<T> List { get; set; }
        public int CountAll { get; set; }
    }
}
