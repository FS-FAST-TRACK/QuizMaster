using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.Library.Common.SeedData.Quiz
{
	public static class QuestionDifficulties
	{
		private static readonly IEnumerable<string> _difficulties = new string[] { "Easy", "Average", "Difficult"};

		public static IEnumerable<QuestionDifficulty> Difficulties = _difficulties
			.Select((category, index) => new QuestionDifficulty
			{
				Id = index + 1,
				QDifficultyDesc = category,
				CreatedByUserId = 1,
			}).ToList();
	}
}
