﻿namespace QuizMaster.API.Quiz.ResoureParameters
{
	public interface IResourceParameter
	{
		int maxPageSize { get; }
		bool IsOnlyActiveData { get; set; }
		public string? SearchQuery { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
	}
}
