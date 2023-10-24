using Grpc.Core;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class Service : QuizCategoryService.QuizCategoryServiceBase
    {
        private readonly IQuizRepository _quizRepository;

        public Service(IQuizRepository quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public override async Task GetAllQuizCatagory(Empty request, IServerStreamWriter<GetAllQuizCatagoryResponse> responseStream, ServerCallContext context)
        {
            var reply = new GetAllQuizCatagoryResponse();
            foreach(var categories in await _quizRepository.GetAllCategoriesAsync())
            {
                reply.Id = categories.Id;
                reply.QuizCategoryDesc = categories.QCategoryDesc;

                await responseStream.WriteAsync(reply);
            }
        }

        public override async Task<CategoryOrNotFound> GetCategoryById(GetCategoryByIdRequest request, ServerCallContext context)
        {
            var success = new GetCategoryByIdReply();
            var response = new CategoryOrNotFound();

            var category = await _quizRepository.GetCategoryAsync(request.Id);
            if(category == null)
            {
                response.CategoryNotFound = new CategoryNotFound() { Code = "404", Message = "Category not found" };
            }
            else
            {
               success.Id = category.Id;
               success.QuizCategoryDesc = category.QCategoryDesc;
               response.GetCategoryByIdReply = success;
            }

            return await Task.FromResult(response);
        }

        public override async Task<CreateCategoryOrFail> CreateCategory(CreateCategoryRequest request, ServerCallContext context)
        {
            var success = new CreatedCategoryReply();
            var fail = new CreateCategoryFail();
            var response = new CreateCategoryOrFail();

            var categoryFromRepo = await _quizRepository.GetCategoryAsync(request.QuizCategoryDesc);
            if(categoryFromRepo != null && categoryFromRepo.ActiveData)
            {
                fail.Type = "409";
                fail.Message = $"Category with description {request.QuizCategoryDesc} already exist.";
                response.CreateCategoryFail = fail;
                return await Task.FromResult(response);
            }

            bool isSuccess;

            if(categoryFromRepo != null && !categoryFromRepo.ActiveData)
            {
                categoryFromRepo.ActiveData = true;
                isSuccess = _quizRepository.UpdateCategory(categoryFromRepo);
            }
            else
            {
                categoryFromRepo = new QuestionCategory { QCategoryDesc = request.QuizCategoryDesc };
                isSuccess = await _quizRepository.AddCategoryAsync(categoryFromRepo);
            }

            if (!isSuccess) 
            {
                fail.Type = "500";
                fail.Message = $"Failed to create category with description {request.QuizCategoryDesc}.";
                response.CreateCategoryFail = fail;
            }
            else 
            {
                await _quizRepository.SaveChangesAsync();
                success.Id = categoryFromRepo.Id;
                success.QuizCategoryDesc = categoryFromRepo.QCategoryDesc;

                response.CreatedCategoryReply = success;
            }

            return await Task.FromResult(response);
        }

        public override async Task<DeleteCategoryReply> DeleteCategory(DeleteCategoryRequest request, ServerCallContext context)
        {
            var reply = new DeleteCategoryReply() { Response = 203 };
            var category = await _quizRepository.GetCategoryAsync(request.Id);
            if(category == null)
            {
                reply.Response = 404;
                return await Task.FromResult(reply);
            }

            category.ActiveData = false;
            var result = _quizRepository.UpdateCategory(category);
            await _quizRepository.SaveChangesAsync();
            if (!result)
            {
                reply.Response = 500;
            }
            return await Task.FromResult(reply);
        }
    }
}
