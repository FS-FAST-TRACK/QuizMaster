using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class DetailTypeProfile : Profile
	{
        public DetailTypeProfile()
        {
			CreateMap<DetailType, DetailTypeDto>();
		}
    }
}
