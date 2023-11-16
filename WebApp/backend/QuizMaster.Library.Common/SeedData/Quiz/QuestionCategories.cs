using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.Library.Common.SeedData.Quiz
{
	public static class QuestionCategories
	{
		private static readonly IEnumerable<string> _categories = new string[] { "Science", "Movies", "Animals", "Places", "People", "System Operations and Maintenance", "Data Structures", "Algorithms" };

		public static IEnumerable<QuestionCategory> Categories = _categories
			.Select((category, index) => new QuestionCategory
			{
				Id = index  + 1,
				QCategoryDesc = category,
				CreatedByUserId = 1,
			}).ToList();
	}
}
