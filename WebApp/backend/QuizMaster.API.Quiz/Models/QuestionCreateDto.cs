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

		public MultipleChoiceAnswer? MultipleChoiceAnswer { get; set; }
		public bool? TrueOrFalseAnswer { get; set; }
		public TypeAnswer? TypeAnswer { get; set; }
		public SliderAnswer? SliderAnswer { get; set; }
		public PuzzleAnswer? PuzzleAnswer { get; set; }

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

		// Question detail for Multiple choice type of question
		public MultipleChoiceQuestionDetail? MultipleChoiceQuestionDetail { get; set; }

		// Question detail for Slider type of question
		public SliderQuestionDetail? SliderQuestionDetail { get; set; }

		// Question details for Multiple choice plus audio typ of questio
		public MultipleChoicePlusAudioQuestionDetail? MultipleChoicePlusAudioQuestionDetail { get; set; }

		public ValidationModel.ValidationModel IsValid()
		{
			string errors = "";
			if(QTypeId == QuestionTypes.MultipleChoiceSeedData.Id)
			{
				errors +=  MultipleChoiceAnswer == null ? "MultipleChoiceAnswer is required for Multiple Choice Question. " : "" ;
				errors += MultipleChoiceQuestionDetail == null ? "MultipleChoiceQuestionDetail is required for Multiple Choice Question. " : "";
			}

			if (QTypeId == QuestionTypes.TrueOrFalseSeedData.Id)
			{
				errors += TrueOrFalseAnswer == null ? "TrueOrFalseAnswer is required for True or False Question. " : "";
			}

			if (QTypeId == QuestionTypes.TypeAnswerSeedData.Id)
			{
				errors += TypeAnswer == null ? "TypeAnswer is required for TypeAnswer Question. " : "";
			}

			if (QTypeId == QuestionTypes.PuzzleSeedData.Id)
			{
				errors += MultipleChoiceAnswer == null ? "MultipleChoiceAnswer is required for PuzzleSeedData Question. " : "";
			}


			if (QTypeId == QuestionTypes.SliderSeedData.Id)
			{
				errors += SliderAnswer == null ? "SliderAnswer is required for Slider Question. " : "";
				errors += SliderQuestionDetail == null ? "SliderQuestionDetail is required for Slider Question. " : "";

			}
			return new ValidationModel.ValidationModel()
			{
				Error = errors,
			};

		}
	}
}
