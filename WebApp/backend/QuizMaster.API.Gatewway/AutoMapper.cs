using AutoMapper;
using QuizMaster.API.Account.Models;
using QuizMaster.API.Account.Proto;
using QuizMaster.Library.Common.Entities.Accounts;

namespace QuizMaster.API.Gatewway
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<AccountCreateDto, CreateAccountRequest>();
            CreateMap<AccountCreatePartialDto, CreateAccountPartialRquest>();
            CreateMap<UserAccount, AccountDto>().ReverseMap();
        }
    }
}
