using AutoMapper;
using Newtonsoft.Json;
using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Questionnaire.Answers;
using QuizMaster.Library.Common.Entities.Questionnaire.Details;
using QuizMaster.Library.Common.Entities.Questionnaire.Options;

namespace QuizMaster.API.Quiz.Profiles
{
	public class DetailProfile : Profile   
	{
		public DetailProfile() {
			CreateMap<QuestionDetail, DetailDto>();

			// Multiple choice detail
			CreateMap<QuestionCreateDto<MultipleChoiceAnswer, MultipleChoiceQuestionDetail>, QuestionDetail>()
				.ForMember(destination => destination.QDetailDesc, act => act.MapFrom(src => JsonConvert.SerializeObject(src.QDetails)));

			// Slider detail
			CreateMap<QuestionCreateDto<SliderAnswer, SliderQuestionDetail>, QuestionDetail>()
				.ForMember(destination => destination.QDetailDesc, act => act.MapFrom(src => JsonConvert.SerializeObject(src.QDetails)));

			// Puzzle detail
			CreateMap<QuestionCreateDto<PuzzleAnswer, MultipleChoiceQuestionDetail>, QuestionDetail>()
				.ForMember(destination => destination.QDetailDesc, act => act.MapFrom(src => JsonConvert.SerializeObject(src.QDetails)));



		}

	}
}
