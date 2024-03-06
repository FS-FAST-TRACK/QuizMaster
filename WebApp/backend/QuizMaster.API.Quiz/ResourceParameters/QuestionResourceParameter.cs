﻿namespace QuizMaster.API.Quiz.ResourceParameters
{
	public class QuestionResourceParameter : IResourceParameter
	{
		public int maxPageSize { get; set; } = 50;
		public bool IsOnlyActiveData { get; set; } = true;
		public bool IncludeDetails { get; set; } = true;
		public string? SearchQuery { get; set; }
		public int PageNumber { get; set; } = 1;

		private int _pageSize = 40;
		public string? FilterByCategoriesId { get; set; }
		public string?  FilterByTypesId { get; set; }
		public string? FilterByDifficultiesId { get; set; }

		public string? ExludeQuestionsIds { get; set; }

		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = ( value > maxPageSize ) ? maxPageSize : value;
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
				includeDetails = IncludeDetails,
				searchQuery = SearchQuery,
				filterByCategories = FilterByCategoriesId,
				filterByDifficulties = FilterByDifficultiesId,
				filterByTypes = FilterByTypesId
			};
		}
	}
}
