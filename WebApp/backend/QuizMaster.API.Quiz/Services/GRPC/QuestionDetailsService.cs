using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.API.Quiz.Services.Workers;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class QuestionDetailsService : QuestionDetailServices.QuestionDetailServicesBase
    {

        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionDetailManager _questionDetailManager;
        private readonly QuizAuditService.QuizAuditServiceClient _quizAuditServiceClient;
        private readonly IMapper _mapper;

        public QuestionDetailsService(IQuizRepository quizRepository, IQuestionDetailManager questionDetailManager, IMapper mapper, QuizAuditService.QuizAuditServiceClient quizAuditServiceClient, QuizDataSynchronizationWorker quizDataSynchronizationWorker)
        {
            _quizRepository = quizRepository;
            _questionDetailManager = questionDetailManager;
            _mapper = mapper;
            _quizAuditServiceClient = quizAuditServiceClient;
        }
        public override async Task<QuestionDetailResponse> GetQuestionDetails(QuestionDetailRequest request, ServerCallContext context)
        {
            var reply = new QuestionDetailResponse();
            var questionId = request.QuestionId;

            var questionDetails = await _quizRepository.GetQuestionDetailsAsync(questionId);

            reply.Code = 200;
            reply.Content = JsonConvert.SerializeObject(questionDetails);
            reply.Type = "IEnumerable<QuestionDetail>";

            return reply;
        }
        public override async Task<QuestionDetailResponse> GetQuestionDetail(QuestionDetailRequest request, ServerCallContext context)
        {
            var reply = new QuestionDetailResponse();
            var questionId = request.QuestionId;
            var id = request.Id;


            var questionDetail = await _quizRepository.GetQuestionDetailAsync(questionId, id);
            if (questionDetail == null || !questionDetail.ActiveData)
            {
                reply.Code = 404;
                reply.Content = "Question detail doesn't exist";
                reply.Type = "string";
                return reply;
            }
            reply.Code = 200;
            reply.Content = JsonConvert.SerializeObject(questionDetail);
            reply.Type = "QuestionDetail";

            return reply;
        }
        public override async Task<QuestionDetailResponse> AddQuestionDetail(QuestionDetailRequest request, ServerCallContext context)
        {
            var reply = new QuestionDetailResponse();
            var questionId = request.QuestionId;
            var questionDetailCreateDto = new QuestionDetailCreateDto();
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                questionDetailCreateDto = JsonConvert.DeserializeObject<QuestionDetailCreateDto>(request.Content);
                if (questionDetailCreateDto == null)
                {
                    throw new Exception("Failed to deserialized request content into QuestionDetailCreateDto.");
                }
                if (userId == null)
                {
                    throw new Exception("UserId must not be null when deleting question detail.");
                }

            }
            catch (Exception ex)
            {
                reply.Content = ex.Message;
                reply.Code = 500;
                reply.Type = "string";
                return reply;
            }
            // Check if question exist or not
            var question = await _quizRepository.GetQuestionAsync(questionId);
            if (question == null)
            {
                reply.Content = "Question doesn't exist";
                reply.Code = 404;
                reply.Type = "string";
                return reply;
            }

            // Map the questionDetailCreateDto to QuestionDetail
            var questionDetail = _mapper.Map<QuestionDetail>(questionDetailCreateDto);

            // Get detailTypes of the questionDetail is using
            var detailTypes = await _quizRepository.GetDetailTypesAsync(questionDetailCreateDto.DetailTypes);

            // Set the detailTypes of the questionDetail
            questionDetail.DetailTypes = detailTypes;

            questionDetail.CreatedByUserId = int.Parse(userId);
            questionDetail.DateCreated = DateTime.Now;

            // Add the questionDetail to the Db
            var success = await _questionDetailManager.AddQuestionDetailAsync(question, questionDetail);

            // Check if addQuestionDetail is a success
            if (!success)
            {
                reply.Content = "Failed to create question detail.";
                reply.Code = 500;
                reply.Type = "string";
                return reply;
            }

            // Save Changes to Db
            await _quizRepository.SaveChangesAsync();

            // Return the created question Detail in the form of QuestionDetailDto
            var questionDetailDto = _mapper.Map<QuestionDetailDto>(questionDetail);
            questionDetailDto.DetailTypes = questionDetailCreateDto.DetailTypes;

            reply.Content = JsonConvert.SerializeObject(questionDetail);
            reply.Code = 201;
            reply.Type = "QuestionDetail";

            return reply;
        }
        public override async Task<QuestionDetailResponse> DeleteQuestionDetail(QuestionDetailRequest request, ServerCallContext context)
        {
            var reply = new QuestionDetailResponse();
            var questionId = request.QuestionId;
            var id = request.Id;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;
            try
            {
                if (userId == null)
                {
                    throw new Exception("UserId must not be null when deleting question detail.");
                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
                return reply;
            }

            var questionDetailFromDb = await _quizRepository.GetQuestionDetailAsync(questionId, id);

            // Checks if category does not exist or (if it exist) checks if category is active
            if (questionDetailFromDb == null || !questionDetailFromDb.ActiveData)
            {
                reply.Content = "Question detail doesn't exist";
                reply.Code = 404;
                reply.Type = "string";
                return reply;
            }

            // change active to false 
            questionDetailFromDb.ActiveData = false;

            questionDetailFromDb.DateUpdated = DateTime.Now;
            questionDetailFromDb.UpdatedByUserId = int.Parse(userId);

            var isSuccess = _quizRepository.UpdateQuestionDetail(questionDetailFromDb);

            // Check if update is success
            if (!isSuccess)
            {
                reply.Content = "Failed to delete question detail.";
                reply.Code = 500;
                reply.Type = "string";
                return reply;
            }

            // Save changes
            await _quizRepository.SaveChangesAsync();
            return reply;
        }
        public override async Task<QuestionDetailResponse> PatchQuestionDetail(QuestionDetailRequest request, ServerCallContext context)
        {
            var reply = new QuestionDetailResponse();

            var questionId = request.QuestionId;
            var id = request.Id;
            var patch = new JsonPatchDocument<QuestionDetailCreateDto>();
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                patch = JsonConvert.DeserializeObject<JsonPatchDocument<QuestionDetailCreateDto>>(request.Content);

                if (patch == null)
                {
                    throw new Exception("Failed to deserialize GRPC request content into JsonPatchDocument<QuestionDetailCreateDto>.");
                }
                if (userId == null)
                {
                    throw new Exception("UserId must not be null when updating question detail.");
                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
                return reply;
            }


            var question = await _quizRepository.GetQuestionAsync(questionId);

            // Checks if question does not exist or (if it exist) checks if question is active
            if (question == null || !question.ActiveData)
            {
                reply.Content = "Question doesn't exist";
                reply.Code = 404;
                reply.Type = "string";
                return reply;
            }

            // Get QuestionDetail with associated questionID
            var questionDetailFromRepo = await _quizRepository.GetQuestionDetailAsync(questionId, id);

            // Guard if questionDetail doesn't exist or deleted
            if (questionDetailFromRepo == null || !questionDetailFromRepo.ActiveData)
            {
                reply.Content = "Question detail doesn't exist";
                reply.Code = 404;
                reply.Type = "string";
                return reply;
            }

            // Patch the changes into the question from repo
            var questionDetailToPatch = _mapper.Map<QuestionDetailCreateDto>(questionDetailFromRepo);
            patch.ApplyTo(questionDetailToPatch);
            _mapper.Map(questionDetailToPatch, questionDetailFromRepo);


            // update necessary properties
            questionDetailFromRepo.DateUpdated = DateTime.Now;
            questionDetailFromRepo.UpdatedByUserId = int.Parse(userId);

            // Checks if DetailTypes is updated or not
            if (questionDetailToPatch.DetailTypes != null)
            {
                var detailTypes = await _quizRepository.GetDetailTypesAsync(questionDetailToPatch.DetailTypes);
                questionDetailFromRepo.DetailTypes = detailTypes;
            }

            // Validate model of questionDetail
            //if (!TryValidateModel(questionDetailFromRepo))
            //{
            //    return ReturnModelStateErrors();
            //}

            var success = await _questionDetailManager.UpdateQuestionDetailAsync(question, questionDetailFromRepo);

            // Check if updateQuestionDetail is a success
            if (!success)
            {
                reply.Content = "Failed to create question detail.";
                reply.Code = 500;
                reply.Type = "string";
                return reply;

            }

            // Save Changes to Db
            await _quizRepository.SaveChangesAsync();

            // Return the created question Detail in 
            reply.Content = JsonConvert.SerializeObject(questionDetailFromRepo);
            reply.Code = 200;
            reply.Type = "QuestionDetail";
            return reply;
        }
    }
}
