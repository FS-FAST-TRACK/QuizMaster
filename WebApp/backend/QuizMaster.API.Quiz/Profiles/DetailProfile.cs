using AutoMapper;
using Newtonsoft.Json;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Entities.Questionnaire.Details;

namespace QuizMaster.API.Quiz.Profiles
{
	public class DetailProfile : Profile
	{
		public DetailProfile()
		{
			CreateMap<Detail, DetailDto>();

			//CreateMap<QuestionCreateDto, QuestionDetail>()
			//	.ForMember(destination => destination.QDetailDesc, act =>
			//	act.MapFrom(src =>
			//	src.QTypeId == QuestionTypes.MultipleChoiceSeedData.Id
			//		? JsonConvert.SerializeObject(src.MultipleChoiceQuestionDetail) :
			//	src.QTypeId == QuestionTypes.SliderSeedData.Id
			//		? JsonConvert.SerializeObject(src.SliderQuestionDetail) :
			//	src.QTypeId == QuestionTypes.MultipleChoicePlusAudioSeedData.Id
			//		? JsonConvert.SerializeObject(src.MultipleChoicePlusAudioQuestionDetail) :
			//	""));

			//CreateMap<QuestionDetail, QuestionCreateDto>()
			//	.ForMember(destination => destination.MultipleChoiceQuestionDetail, act =>
			//	act.MapFrom((src, destination)=> destination.QTypeId == QuestionTypes.MultipleChoiceSeedDataId ?JsonConvert.DeserializeObject<MultipleChoiceQuestionDetail>(src.QDetailDesc) : null
			//	))
			//	.ForMember(destination => destination.SliderQuestionDetail, act =>
			//	act.MapFrom((src, destination) => destination.QTypeId == QuestionTypes.SliderSeedDataId? JsonConvert.DeserializeObject<SliderQuestionDetail>(src.QDetailDesc) : null
			//	))
			//	.ForMember(destination => destination.MultipleChoicePlusAudioQuestionDetail, act =>
			//	act.MapFrom((src, destination) => destination.QTypeId == QuestionTypes.MultipleChoicePlusAudioSeedDataId ? JsonConvert.DeserializeObject<MultipleChoicePlusAudioQuestionDetail>(src.QDetailDesc) : null
			//	));
		}

	}
}
