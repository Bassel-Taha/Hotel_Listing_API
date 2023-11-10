namespace ASP_API_Udemy_Course.Models
{
    public class PageResult<T>
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int RecordNumber { get; set; }
        public List<T> Itims { get; set; }
    }
}
    