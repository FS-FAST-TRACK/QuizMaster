namespace QuizMaster.API.Quiz.ResourceParameters
{
    public class DifficultyResourceParameter : IResourceParameter
    {
        public int maxPageSize => 50;
        public bool IsOnlyActiveData { get; set; } = true;
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        public bool IsGetAll { get; set; } = false;

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
        public object GetObject(string type)
        {
            return new
            {
                pageNumber =
                    type == "next" ? PageNumber + 1 :
                    type == "prev" ? PageNumber - 1 :
                    0,
                pageSize = PageSize,
                isOnlyActiveData = IsOnlyActiveData,
                searchQuery = SearchQuery
            };
        }
    }
}
