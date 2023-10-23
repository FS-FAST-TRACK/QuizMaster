using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.SeedData
{
	public static class QuestionTypes
	{
		private static readonly IEnumerable<string> _types = new string[] { "Multiple Choice", "Multiple Choice + Audio", "True or False", "Type Answer", "Slider", "Puzzle" };

		public static IEnumerable<QuestionType> Types = _types
			.Select((type, index) =>
				new QuestionType
				{
					Id = index + 1,
					QTypeDesc = type,
					QDetailRequired = ( type == "Slider" || type == "Puzzle" ),
					CreatedByUserId = 1,
				}
				)
			.ToList();
	}
}
