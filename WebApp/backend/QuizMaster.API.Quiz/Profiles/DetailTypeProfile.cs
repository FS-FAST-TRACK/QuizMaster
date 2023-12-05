using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class DetailTypeProfile : Profile
	{
		public DetailTypeProfile()
		{
			CreateMap<DetailType, DetailTypeDto>();
			CreateMap<DetailType, string>().ConvertUsing((src, dest) =>
			{ 
				if(src.Id == DetailTypes.TextToAudioDetailType.Id)
				{
					return "textToAudio";
				}
				return src.DTypeDesc.ToLower().Replace(" ", ""); }

				);
		}
	}
}
