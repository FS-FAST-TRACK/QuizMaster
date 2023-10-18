using AutoMapper;
using QuizMaster.API.Account.Models;
using QuizMaster.Library.Common.Entities.Accounts;

namespace QuizMaster.API.Account.Profiles
{
	public class UserAccountProfile : Profile
	{
		public UserAccountProfile()
		{
			CreateMap<AccountCreateDto, UserAccount>();
			CreateMap<AccountCreatePartialDto, UserAccount>();
			CreateMap<UserAccount, AccountDto>();
		}
	}
}

