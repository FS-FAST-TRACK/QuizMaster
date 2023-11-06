using AutoMapper;
using QuizMaster.API.Account.Models;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Quiz.Models;
using QuizMaster.Library.Common.Entities.Accounts;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Gatewway
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<AccountCreateDto, CreateAccountRequest>();
            CreateMap<AccountCreatePartialDto, CreateAccountPartialRquest>();
            CreateMap<UserAccount, AccountDto>().ReverseMap();
            CreateMap<CategoryCreateDto, QuestionCategory>().ReverseMap();
        }
    }
}
