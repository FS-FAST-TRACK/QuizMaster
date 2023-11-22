using AutoMapper;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.SeedData;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Profiles
{
	public class QuestionProfile: Profile
	{
		public QuestionProfile()
		{
			CreateMap<Question, QuestionDto>();
			CreateMap<Question, QuestionCreateDto>();
			CreateMap<QuestionCreateDto, Question>();

			//CreateMap<QuestionCreateDto, QuestionDetailsCreateDto>()
			//	.ForMember(destination => destination.QuestionDetailCreateDtos, act => act.MapFrom((src, destination) =>
			//	{
			//		var questionDetailCreateDtos = new List<QuestionDetailCreateDto>();
			//		switch (src.QTypeId)
			//		{
			//			case QuestionTypes.MultipleChoiceSeedDataId:
			//				src.Options.ToList().ForEach(o =>
			//				{
			//					var detailCreateDto = new QuestionDetailCreateDto();
			//					var detailIds = new List<int>()
			//					{
			//						DetailTypes.OptionDetailType.Id,
			//					};
			//					if (o.IsAnswer)
			//					{
			//						detailIds.Add(DetailTypes.AnswerDetailType.Id);
			//					}
			//					detailCreateDto.DetailDesc = o.Value;
			//					detailCreateDto.DetailTypeIds = detailIds;
			//					questionDetailCreateDtos.Add(detailCreateDto);
			//				}
			//				);
			//				break;
			//			case QuestionTypes.TrueOrFalseSeedDataId:
			//				var detailCreateDto = new QuestionDetailCreateDto();
			//				var detailIds = new List<int>()
			//					{
			//						DetailTypes.AnswerDetailType.Id,
			//					};
			//				var answer = src.Options.Where(o => o.IsAnswer).SingleOrDefault();
			//				if (answer == null)
			//				{
			//					throw new Exception("Answer is not provided for True or False question");
			//				}
			//				detailCreateDto.DetailDesc = answer.Value;
			//				detailCreateDto.DetailTypeIds = detailIds;
			//				questionDetailCreateDtos.Add(detailCreateDto);
			//				break;
			//			case QuestionTypes.TypeAnswerSeedDataId:
			//				src.Options.ToList().ForEach(o =>
			//				{
			//					var detailCreateDto = new QuestionDetailCreateDto();
			//					var detailIds = new List<int>();
			//					if (o.IsAnswer)
			//					{
			//						detailIds.Add(DetailTypes.AnswerDetailType.Id);
			//					}
			//					detailCreateDto.DetailDesc = o.Value;
			//					detailCreateDto.DetailTypeIds = detailIds;
			//					questionDetailCreateDtos.Add(detailCreateDto);
			//				}
			//				);
			//				break;
			//			case QuestionTypes.SliderSeedDataId:
			//				src.Options.ToList().ForEach(o =>
			//				{
			//					var detailCreateDto = new QuestionDetailCreateDto();
			//					var detailIds = new List<int>();
			//					if (o.IsAnswer)
			//					{
			//						detailIds.Add(DetailTypes.AnswerDetailType.Id);
			//					}
			//					detailCreateDto.DetailDesc = o.Value;
			//					detailCreateDto.DetailTypeIds = detailIds;
			//					questionDetailCreateDtos.Add(detailCreateDto);
			//				}
			//				);
			//				var minimumDetailCreateDto = new QuestionDetailCreateDto() { DetailDesc = src.Minimum!, DetailTypeIds = new List<int>() { DetailTypes.MinimumDetailType.Id } };
			//				var maximumDetailCreateDto = new QuestionDetailCreateDto() { DetailDesc = src.Maximum!, DetailTypeIds = new List<int>() { DetailTypes.MaximumDetailType.Id } };
			//				var intervalDetailCreateDto = new QuestionDetailCreateDto() { DetailDesc = src.Interval!, DetailTypeIds = new List<int>() { DetailTypes.IntervalDetailType.Id } };
			//				var marginDetailCreateDto = new QuestionDetailCreateDto() { DetailDesc = src.Margin!, DetailTypeIds = new List<int>() { DetailTypes.MarginDetailType.Id } };
			//				questionDetailCreateDtos.AddRange(new List<QuestionDetailCreateDto> { minimumDetailCreateDto, maximumDetailCreateDto, intervalDetailCreateDto, marginDetailCreateDto });

			//				break;
			//			case QuestionTypes.PuzzleSeedDataId:
			//				src.Options.ToList().ForEach(o =>
			//				{
			//					var detailCreateDto = new QuestionDetailCreateDto();
			//					var detailIds = new List<int>() {
			//						DetailTypes.OptionDetailType.Id
			//					};
			//					if (o.IsAnswer)
			//					{
			//						detailIds.Add(DetailTypes.AnswerDetailType.Id);
			//					}
			//					detailCreateDto.DetailDesc = o.Value;
			//					detailCreateDto.DetailTypeIds = detailIds;
			//					questionDetailCreateDtos.Add(detailCreateDto);
			//				}
			//				);
			//				break;
			//			case QuestionTypes.MultipleChoicePlusAudioSeedDataId:
			//				src.Options.ToList().ForEach(o =>
			//				{
			//					var detailCreateDto = new QuestionDetailCreateDto();
			//					var detailIds = new List<int>()
			//					{
			//						DetailTypes.OptionDetailType.Id
			//					};
			//					if (o.IsAnswer)
			//					{
			//						detailIds.Add(DetailTypes.AnswerDetailType.Id);
			//					}
			//					detailCreateDto.DetailDesc = o.Value;
			//					detailCreateDto.DetailTypeIds = detailIds;
			//					questionDetailCreateDtos.Add(detailCreateDto);
			//				}
			//				);
			//				var textToAudioDetailCreateDto = new QuestionDetailCreateDto() { DetailDesc = src.TextToAudio!, DetailTypeIds = new List<int>() { DetailTypes.TextToAudioDetailType.Id } };
			//				var languageDetailCreateDto = new QuestionDetailCreateDto() { DetailDesc = src.Language!, DetailTypeIds = new List<int>() { DetailTypes.LanguageDetailType.Id } };
			//				questionDetailCreateDtos.AddRange(new List<QuestionDetailCreateDto> { textToAudioDetailCreateDto, languageDetailCreateDto });
			//				break;
			//		}
			//		return questionDetailCreateDtos;
			//	}
			//	));


		}
	}
}
