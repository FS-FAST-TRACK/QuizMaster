using AutoMapper;
using Newtonsoft.Json;
using QuizMaster.API.Account.Models;
using QuizMaster.API.Account.Proto;
using QuizMaster.API.Gateway.Models.System;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.SeedData;
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
            CreateMap<AccountCreateDto, UserAccount>().ReverseMap();
            CreateMap<CategoryCreateDto, QuestionCategory>().ReverseMap();
            CreateMap<Question, QuestionDto>();
            CreateMap<DificultiesReply, DifficultyDto>().ReverseMap();
            CreateMap<DifficultyCreateDto, CreateDifficultyRequest>().ReverseMap();
            CreateMap<TypeReply, TypeDto>().ReverseMap();
            CreateMap<Question, QuestionDto>().ReverseMap();
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionCategory, CategoryDto>().ReverseMap();
            CreateMap<SubmitContactModel, ContactReaching>().ReverseMap();
            CreateMap<AboutModel, SystemAbout>().ReverseMap();
            CreateMap<ContactModel, SystemContact>().ReverseMap();
            CreateMap<ReviewModel, Reviews>().ReverseMap();
    //        CreateMap<QuestionCreateDto, Question>()
    //.ForMember(destination => destination.QAnswer, act => act.MapFrom(src =>
    //src.QTypeId == QuestionTypes.MultipleChoiceSeedData.Id
    //    ? JsonConvert.SerializeObject(src.MultipleChoiceAnswer) :
    //src.QTypeId == QuestionTypes.TrueOrFalseSeedData.Id
    //    ? JsonConvert.SerializeObject(src.TrueOrFalseAnswer) :
    //src.QTypeId == QuestionTypes.TypeAnswerSeedData.Id
    //    ? JsonConvert.SerializeObject(src.TypeAnswer) :
    //src.QTypeId == QuestionTypes.SliderSeedData.Id
    //    ? JsonConvert.SerializeObject(src.SliderAnswer) :
    //src.QTypeId == QuestionTypes.PuzzleSeedData.Id
    //    ? JsonConvert.SerializeObject(src.PuzzleAnswer) :
    //src.QTypeId == QuestionTypes.MultipleChoicePlusAudioSeedData.Id
    //    ? JsonConvert.SerializeObject(src.MultipleChoiceAnswer) :
    //"")).ReverseMap();
        }
    }
}
