using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.SeedData
{
	public static class QuestionDetails
	{
		public static IEnumerable<QuestionDetail> SeedData = new List<QuestionDetail>
		{
			new QuestionDetail
			{
				Id = 1,
				QuestionId = 1,
				QDetailDesc = "Oxygen"
			},
			new QuestionDetail
			{
				Id = 2,
				QuestionId = 1,
				QDetailDesc = "Nitrogen"
			},
			new QuestionDetail
			{
				Id = 3,
				QuestionId = 1,
				QDetailDesc = "Carbon Dioxide"
			},
			new QuestionDetail
			{
				Id = 4,
				QuestionId = 1,
				QDetailDesc = "Hydrogen"
			},
			new QuestionDetail
			{
				Id = 5,
				QuestionId = 2,
				QDetailDesc = "True"
			},
			new QuestionDetail
			{
				Id = 6,
				QuestionId = 3,
				QDetailDesc = "42"
			},new QuestionDetail
			{
				Id = 7,
				QuestionId = 3,
				QDetailDesc = "10"
			},
			new QuestionDetail
			{
				Id = 8,
				QuestionId = 3,
				QDetailDesc = "50"
			},
			new QuestionDetail
			{
				Id = 9,
				QuestionId = 3,
				QDetailDesc = "1"
			},
		};
	}
}
