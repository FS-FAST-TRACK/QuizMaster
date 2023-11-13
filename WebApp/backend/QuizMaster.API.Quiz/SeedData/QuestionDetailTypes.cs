using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.SeedData
{
	public static class QuestionDetailTypes
	{

		public static readonly QuestionDetailType AnswerDetailType = new QuestionDetailType()
		{
			Id = 1,
			DTypeDesc = "Answer"
		};

		public static readonly QuestionDetailType OptionDetailType = new QuestionDetailType()
		{
			Id = 2,
			DTypeDesc = "Option"
		};

		public static readonly QuestionDetailType MinimumDetailType = new QuestionDetailType()
		{
			Id = 3,
			DTypeDesc = "Minimum"
		};
		public static readonly QuestionDetailType MaximumDetailType = new QuestionDetailType()
		{
			Id = 4,
			DTypeDesc = "Maximum"
		};
		public static readonly QuestionDetailType IntervalDetailType = new QuestionDetailType()
		{
			Id = 5,
			DTypeDesc = "Interval"
		};

		public static readonly QuestionDetailType MarginDetailType = new QuestionDetailType()
		{
			Id = 6,
			DTypeDesc = "Margin"
		};

		public static readonly QuestionDetailType TextToAudioDetailType = new QuestionDetailType()
		{
			Id = 7,
			DTypeDesc = "TextToAudio"
		};

		public static readonly QuestionDetailType LanguageDetailType = new QuestionDetailType()
		{
			Id = 8,
			DTypeDesc = "Language"
		};


		public static IEnumerable<QuestionDetailType> DetailTypes = new QuestionDetailType[]
		{
			AnswerDetailType, OptionDetailType, MinimumDetailType, MaximumDetailType, IntervalDetailType, MarginDetailType, TextToAudioDetailType, LanguageDetailType
		};
	}
}
