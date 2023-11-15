using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.SeedData
{
	public static class DetailTypes
	{
		public static Dictionary<string, int> keyValuePairs = new Dictionary<string, int>()
		{
			{ "answer", 1 },
			{ "option", 2 },
			{ "minimum", 3 },
			{ "maximum", 4 },
			{ "interval", 5 },
			{ "margin", 6 },
			{ "textToAudio", 7 },
			{ "language", 8 },
		};
		public static readonly DetailType AnswerDetailType = new DetailType()
		{
			Id = 1,
			DTypeDesc = "Answer"
		};

		public static readonly DetailType OptionDetailType = new DetailType()
		{
			Id = 2,
			DTypeDesc = "Option"
		};

		public static readonly DetailType MinimumDetailType = new DetailType()
		{
			Id = 3,
			DTypeDesc = "Minimum"
		};
		public static readonly DetailType MaximumDetailType = new DetailType()
		{
			Id = 4,
			DTypeDesc = "Maximum"
		};
		public static readonly DetailType IntervalDetailType = new DetailType()
		{
			Id = 5,
			DTypeDesc = "Interval"
		};

		public static readonly DetailType MarginDetailType = new DetailType()
		{
			Id = 6,
			DTypeDesc = "Margin"
		};

		public static readonly DetailType TextToAudioDetailType = new DetailType()
		{
			Id = 7,
			DTypeDesc = "TextToAudio"
		};

		public static readonly DetailType LanguageDetailType = new DetailType()
		{
			Id = 8,
			DTypeDesc = "Language"
		};


		public static IEnumerable<DetailType> SeedData = new DetailType[]
		{
			AnswerDetailType, OptionDetailType, MinimumDetailType, MaximumDetailType, IntervalDetailType, MarginDetailType, TextToAudioDetailType, LanguageDetailType
		};
	}
}
