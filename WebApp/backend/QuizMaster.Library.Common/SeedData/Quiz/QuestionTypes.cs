using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.Library.Common.SeedData.Quiz
{
	public static class QuestionTypes
	{
		public const int MultipleChoiceSeedDataId = 1; 
		public static QuestionType MultipleChoiceSeedData = new QuestionType()
		{
			Id = 1,
			QTypeDesc = "Multiple Choice",
			QDetailRequired = true,
			CreatedByUserId = 1,
		};

		public const int MultipleChoicePlusAudioSeedDataId =2;
		public static QuestionType MultipleChoicePlusAudioSeedData = new QuestionType()
		{
			Id = 2,
			QTypeDesc = "Multiple Choice + Audio",
			QDetailRequired = true,
			CreatedByUserId = 1,
		};

		public const int TrueOrFalseSeedDataId = 3;
		public static QuestionType TrueOrFalseSeedData = new QuestionType()
		{
			Id = 3,
			QTypeDesc = "True or False",
			QDetailRequired = false,
			CreatedByUserId = 1,
		};

		public const int TypeAnswerSeedDataId = 4;
		public static QuestionType TypeAnswerSeedData = new QuestionType()
		{
			Id = 4,
			QTypeDesc = "Type Answer",
			QDetailRequired = true,
			CreatedByUserId = 1,
		};

		public const int SliderSeedDataId  = 5;
		public static QuestionType SliderSeedData = new QuestionType()
		{
			Id = 5,
			QTypeDesc = "Slider",
			QDetailRequired = true,
			CreatedByUserId = 1,
		};

		public const int PuzzleSeedDataId = 6; 
		public static QuestionType PuzzleSeedData = new QuestionType()
		{
			Id = 6,
			QTypeDesc = "Puzzle",
			QDetailRequired = true,
			CreatedByUserId = 1,
		};

		public static IEnumerable<QuestionType> Types = new QuestionType[]
		{
			MultipleChoiceSeedData,
			MultipleChoicePlusAudioSeedData,
			TrueOrFalseSeedData,
			TypeAnswerSeedData,
			SliderSeedData, 
			PuzzleSeedData,
		};

		//private static readonly IEnumerable<string> _types = new string[] { "Multiple Choice", "Multiple Choice + Audio", "True or False", "Type Answer", "Slider", "Puzzle" };

		//public static IEnumerable<QuestionType> Types = _types
		//	.Select((type, index) =>
		//		new QuestionType
		//		{
		//			Id = index + 1,
		//			QTypeDesc = type,
		//			QDetailRequired = ( type == "Slider" || type == "Puzzle" ),
		//			CreatedByUserId = 1,
		//		}
		//		)
		//	.ToList();
	}
}
