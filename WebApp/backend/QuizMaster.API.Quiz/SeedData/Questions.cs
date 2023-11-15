using QuizMaster	.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.SeedData
{
	public static class Questions
	{
		public static IEnumerable<Question> SeedData = new List<Question>
		{
			new Question
			{
				Id = 1,
				QStatement = "What is the primary gas in Earth's atmosphere?",
				QImage= "",
				QAudio = "",
				QCategoryId = QuestionCategories.Categories.Where(c=>c.Id == 1).FirstOrDefault()!.Id,
				QDifficultyId = QuestionDifficulties.Difficulties.ToList()[0].Id,
				QTypeId = QuestionTypes.MultipleChoiceSeedData.Id,
				QTime = 30
			},new Question
			{
				Id = 2,
				QStatement = "True or False: Earth's moon is larger than Pluto.",
				QImage= "",
				QAudio = "",
				QCategoryId = QuestionCategories.Categories.Where(c=>c.Id == 1).FirstOrDefault()!.Id,
				QDifficultyId = QuestionDifficulties.Difficulties.ToList()[1].Id,
				QTypeId = QuestionTypes.TrueOrFalseSeedData.Id,
				QTime = 20
			},
			new Question
			{
				Id = 3,
				QStatement = "What is the number divisible by 3 and 7, anb between 10 and 50?",
				QImage= "",
				QAudio = "",
				QCategoryId = QuestionCategories.Categories.Where(c=>c.Id == 1).FirstOrDefault()!.Id,
				QDifficultyId = QuestionDifficulties.Difficulties.ToList()[1].Id,
				QTypeId = QuestionTypes.SliderSeedData.Id,
				QTime = 25
			}
		};
	}
}
