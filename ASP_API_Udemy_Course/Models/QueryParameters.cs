namespace ASP_API_Udemy_Course.Models
{
    public class QueryParameters
    {
        public int StartIndex { get; set; }
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
    