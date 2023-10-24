namespace QuizMaster.API.Quiz.ResoureParameters
{
	public class CategoryResourceParameter : IResourceParameter
	{
		public int maxPageSize => 50;
		public bool IsOnlyActiveData { get; set; } = false;
		public string? SearchQuery { get; set; }
		public int PageNumber { get; set; } = 1;

		private int _pageSize = 20;
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = ( value > maxPageSize ) ? maxPageSize : value;
		}

	}
}
