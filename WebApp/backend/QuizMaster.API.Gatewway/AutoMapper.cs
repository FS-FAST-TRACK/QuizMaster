using AutoMapper;
using QuizMaster.API.Account.Models;
using QuizMaster.API.Account.Proto;

namespace QuizMaster.API.Gatewway
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<AccountCreateDto, CreateAccountRequest>();
            CreateMap<AccountCreatePartialDto, CreateAccountPartialRquest>();
        }
    }
}
