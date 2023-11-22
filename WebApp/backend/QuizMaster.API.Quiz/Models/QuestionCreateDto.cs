using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Questionnaire.Answers;
using QuizMaster.Library.Common.Entities.Questionnaire.Details;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.API.Quiz.Models
{
	public class QuestionCreateDto<TAnswer, TDetail>
	{
		[Required]
		public string QStatement { get; set; }

		[Required]
		public TAnswer QAnswer { get; set; }

		[Required]
		public string QImage { get; set; }

		[AllowNull]
		public string QAudio { get; set; }

		[Required]
		public int QTime { get; set; }

		[Required]
		public int QDifficultyId { get; set; }

		[Required]
		public int QCategoryId { get; set; }

		[Required]
		public int QTypeId { get; set; }

		// QDetails is a json serialized format of the type of details accepted by a question type
		public TDetail? QDetails { get; set; }
	}
	public class QuestionCreateDto
	{
        [Required]
		public string QStatement { get; set; }

		[Required]
		public string QImage { get; set; }

		public string? QAudio { get; set; }

		[Required]
		public int QTime { get; set; }

		[Required]
		public int QDifficultyId { get; set; }

		[Required]
		public int QCategoryId { get; set; }

		[Required]
		public int QTypeId { get; set; }

		[Required]
		public IEnumerable<QuestionDetailCreateDto> questionDetailCreateDtos { get; set; }


		public ValidationModel.ValidationModel IsValid()
		{
			string errors = "";

			switch (QTypeId)
			{
				case QuestionTypes.MultipleChoiceSeedDataId:
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("answer")).Count() == 0 ? "[Multiple Choice Question] Provide at least one answer for the given options. " : "";
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("option")).Count() < 2  ? "[Multiple Choice Question] Provide at least 2 options. " : "";
					break;

				case QuestionTypes.TrueOrFalseSeedDataId:
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("answer")).Count() == 0 ? "[True or False Question] Provide at least one answer for the given options. " : "";
					break;

				case QuestionTypes.TypeAnswerSeedDataId:
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("answer")).Count() == 0 ? "[Type Answer Question] Provide at least one answer for the given options. " : "";
					break;

				case QuestionTypes.PuzzleSeedDataId:
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("answer")).Count() < 2 ? "[Puzzle Question] Provide at least 2 answer for the given options.  " : "";
					break;

				case QuestionTypes.SliderSeedDataId:
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("answer")).Count() == 0 ? "[Puzzle Question] Provide at least one answer for the given options. " : "";
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("minimum")).Count() != 1 ? "[Puzzle Question] Provide one minimum value. " : "";
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("maximum")).Count() != 1 ? "[Puzzle Question] Provide one maximum value. " : "";
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("interval")).Count() != 1 ? "[Puzzle Question] Provide one interval value. " : "";
					break;

				case QuestionTypes.MultipleChoicePlusAudioSeedDataId:
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("answer")).Count() == 0 ? "[Multiple Choice Plus Audio Question] Provide at least one answer for the given options. " : "";
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("option")).Count() < 2 ? "[Multiple Choice Plus Audio Question] Provide at least 2 options. " : "";
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("textToAudio")).Count() != 1 ? "[Multiple Choice Plus Audio Question] Provide one textToAudio value. " : "";
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("language")).Count() != 1 ? "[Multiple Choice Plus Audio Question] Provide one language value. " : "";
					break;
			}

			return new ValidationModel.ValidationModel()
			{
				Error = errors,
			};
		}
	}

	//public class QuestionCreateDto
	//{
	//	[Required]
	//	public string QStatement { get; set; }

	//	[Required]
	//	public string QImage { get; set; }

	//	[AllowNull]
	//	public string QAudio { get; set; }

	//	[Required]
	//	public int QTime { get; set; }

	//	[Required]
	//	public int QDifficultyId { get; set; }

	//	[Required]
	//	public int QCategoryId { get; set; }

	//	[Required]
	//	public int QTypeId { get; set; }

	//	public IEnumerable<OptionCreateDto> Options { get; set; }
	//	public string? Minimum { get; set; }
	//	public string? Maximum { get; set; }
	//	public string? Interval { get; set; }
	//	public string? Margin { get; set; }
	//	public string? TextToAudio { get; set; }
	//	public string? Language { get; set; }

	//	public ValidationModel.ValidationModel IsValid()
	//	{
	//		string errors = "";

	//		switch (QTypeId)
	//		{
	//			case QuestionTypes.MultipleChoiceSeedDataId:
	//				errors += Options.Where(o => o.IsAnswer).Count() == 0 ? "[Multiple Choice Question] Provide at least one answer for the given options. " : "";
	//				errors += Options.Count() < 2 ? "[Multiple Choice Question] Provide at least 2 options. " : "";
	//				break;

	//			case QuestionTypes.TrueOrFalseSeedDataId:
	//				errors += Options.Where(o => o.IsAnswer).Count() == 0 ? "[True or False Question] Provide at least one answer for the given options. " : "";
	//				break;

	//			case QuestionTypes.TypeAnswerSeedDataId:
	//				errors += Options.Where(o => o.IsAnswer).Count() == 0 ? "[Type Answer Question] Provide at least one answer for the given options. " : "";
	//				break;

	//			case QuestionTypes.PuzzleSeedDataId:
	//				errors += Options.Where(o => o.IsAnswer).Count() == 0 ? "[Puzzle Question] Provide at least one answer for the given options. " : "";
	//				errors += Options.Count() < 2 ? "[Puzzle Question] Provide at least 2 options " : "";
	//				break;

	//			case QuestionTypes.SliderSeedDataId:
	//				errors += Options.Where(o => o.IsAnswer).Count() == 0 ? "[Puzzle Question] Provide at least one answer for the given options. " : "";
	//				errors += Minimum == null ? "[Puzzle Question] Provide minimum value. " : "";
	//				errors += Maximum == null ? "[Puzzle Question] Provide maximum value. " : "";
	//				errors += Interval == null ? "[Puzzle Question] Provide interval value. " : "";
	//				break;

	//			case QuestionTypes.MultipleChoicePlusAudioSeedDataId:
	//				errors += Options.Where(o => o.IsAnswer).Count() == 0 ? "[Multiple Choice Plus Audio Question] Provide at least one answer for the given options. " : "";
	//				errors += Options.Count() < 2 ? "[Multiple Choice Plus Audio Question] Provide at least 2 options. " : "";
	//				errors += TextToAudio == null ? "[Multiple Choice Plus Audio Question] Provide textToAudio value. " : "";
	//				errors += Language == null ? "[Multiple Choice Plus Audio Question] Provide language value. " : "";
	//				break;
	//		}

	//		return new ValidationModel.ValidationModel()
	//		{
	//			Error = errors,
	//		};
	//	}
	//}
}
