using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.SeedData
{
	public static class QuestionDetailTypes
	{
		public static IEnumerable<QuestionDetailType> SeedData = new List<QuestionDetailType>
		{
			new QuestionDetailType
			{
				QuestionDetailId = 1,
				DetailTypeId = 2,
			},
			new QuestionDetailType
			{
				QuestionDetailId = 2,
				DetailTypeId = 1,
			},
			new QuestionDetailType
			{
				QuestionDetailId = 2,
				DetailTypeId = 2,
			},
			new QuestionDetailType
			{
				QuestionDetailId = 3,
				DetailTypeId = 2,
			},
			new QuestionDetailType
			{
				QuestionDetailId = 4,
				DetailTypeId = 2,
			},
			new QuestionDetailType
			{
				QuestionDetailId = 5,
				DetailTypeId = 1,
			},
			new QuestionDetailType
			{
				QuestionDetailId =6,
				DetailTypeId = 1,
			},
			new QuestionDetailType
			{
				QuestionDetailId = 7,
				DetailTypeId = DetailTypes.MinimumDetailType.Id,
			},
			new QuestionDetailType
			{
				QuestionDetailId = 8,
				DetailTypeId = DetailTypes.MaximumDetailType.Id,
			},
			new QuestionDetailType
			{
				QuestionDetailId = 9,
				DetailTypeId = DetailTypes.IntervalDetailType.Id,
			},
		};
	}
}
