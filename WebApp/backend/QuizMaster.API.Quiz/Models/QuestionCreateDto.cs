using QuizMaster.API.Quiz.SeedData;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuizMaster.API.Quiz.Models
{
	public class QuestionCreateDto
	{
        [Required]
		public string QStatement { get; set; }

		[AllowNull]
		public string? QImage { get; set; }

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
					errors += questionDetailCreateDtos.Where(qDetail => qDetail.DetailTypes.Contains("option")).Count() < 2 ? "[Puzzle Question] Provide at least parameters for options in correct order.  " : "";
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

}
