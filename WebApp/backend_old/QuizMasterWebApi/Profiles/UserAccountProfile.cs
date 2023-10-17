using AutoMapper;
using QuizMaster.Account.Api.Models;
using QuizMaster.Common.Library.Entities;

namespace QuizMaster.Account.Api.Profiles
{
	public class UserAccountProfile : Profile
	{
		public UserAccountProfile() {
			CreateMap<UserForRegisterDto, UserAccount>();
			CreateMap<UserAccount ,UserDto>();
			CreateMap<UserAccount ,UserForLoginDto>();
			CreateMap<UserForUpdateDto, UserAccount>();
			CreateMap<UserForRegisterDto , UserForLoginDto>();
		}
	}
}
