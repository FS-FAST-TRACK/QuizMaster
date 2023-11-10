using AutoMapper;
using Newtonsoft.Json;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Questionnaire.Answers;
using QuizMaster.Library.Common.Entities.Questionnaire.Details;
using QuizMaster.Library.Common.Entities.Questionnaire.Options;

namespace QuizMaster.API.Quiz.Profiles
{
	public class QuestionProfile: Profile
	{
		public QuestionProfile()
		{
			CreateMap<Question, QuestionDto>();
			CreateMap<QuestionCreateDto, Question>()
				.ForMember(destination => destination.QAnswer, act => act.MapFrom(src => 
				src.QTypeId == QuestionTypes.MultipleChoiceSeedData.Id 
					? JsonConvert.SerializeObject(src.MultipleChoiceAnswer) :
				src.QTypeId == QuestionTypes.TrueOrFalseSeedData.Id
					? JsonConvert.SerializeObject(src.TrueOrFalseAnswer) :
				src.QTypeId == QuestionTypes.TypeAnswerSeedData.Id
					? JsonConvert.SerializeObject(src.TypeAnswer) :
				src.QTypeId == QuestionTypes.SliderSeedData.Id
					? JsonConvert.SerializeObject(src.SliderAnswer) :
				src.QTypeId == QuestionTypes.PuzzleSeedData.Id
					? JsonConvert.SerializeObject(src.PuzzleAnswer) :
				src.QTypeId == QuestionTypes.MultipleChoicePlusAudioSeedData.Id 
					? JsonConvert.SerializeObject(src.MultipleChoiceAnswer) :
				"")).ReverseMap();

			
		}
	}
}
