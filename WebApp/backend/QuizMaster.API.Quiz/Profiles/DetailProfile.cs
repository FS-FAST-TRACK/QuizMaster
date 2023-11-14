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
		}

	}
}
