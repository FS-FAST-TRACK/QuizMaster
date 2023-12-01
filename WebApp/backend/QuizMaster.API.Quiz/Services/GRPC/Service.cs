using Azure;
using Grpc.Core;
using Newtonsoft.Json;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class Service : QuizCategoryService.QuizCategoryServiceBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly QuizAuditService.QuizAuditServiceClient _quizAuditServiceClient;

        public Service(IQuizRepository quizRepository, QuizAuditService.QuizAuditServiceClient quizAuditServiceClient)
        {
            _quizRepository = quizRepository;
            _quizAuditServiceClient = quizAuditServiceClient;
        }

        /// <summary>
        /// Get all the category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GetAllQuizCatagory(Empty request, IServerStreamWriter<GetAllQuizCatagoryResponse> responseStream, ServerCallContext context)
        {
            var reply = new GetAllQuizCatagoryResponse();
            // get all the categories from the service
            foreach(var categories in await _quizRepository.GetAllCategoriesAsync())
            {
                reply.Id = categories.Id;
                reply.QuizCategoryDesc = categories.QCategoryDesc;

                await responseStream.WriteAsync(reply);
            }
        }

        /// <summary>
        /// Get the category by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CategoryOrNotFound> GetCategoryById(GetCategoryByIdRequest request, ServerCallContext context)
        {
            var success = new GetCategoryByIdReply();
            var response = new CategoryOrNotFound();

            // check if category exist
            var category = await _quizRepository.GetCategoryAsync(request.Id);
            
            if(category == null)
            {
                // if not exist return not found
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

        /// <summary>
        /// Create question category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
         public override async Task<CreateCategoryOrFail> CreateCategory(CreateCategoryRequest request, ServerCallContext context)
        {
            var success = new CreatedCategoryReply();
            var fail = new CreateCategoryFail();
            var response = new CreateCategoryOrFail();



            // check if category already exist
            var categoryFromRepo = await _quizRepository.GetCategoryAsync(request.QuizCategoryDesc);

            if (categoryFromRepo != null && categoryFromRepo.ActiveData)
            {
                // if category already exist and active return fail
                fail.Type = "409";
                fail.Message = $"Category with description {request.QuizCategoryDesc} already exists.";
                response.CreateCategoryFail = fail;
                return await Task.FromResult(response);
            }

            bool isSuccess;

            // If category is not null and not active, we set active to true and update the category
            if (categoryFromRepo != null && !categoryFromRepo.ActiveData)
            {
                categoryFromRepo.ActiveData = true;
                isSuccess = _quizRepository.UpdateCategory(categoryFromRepo);
            }
            else
            {
                // else, we create a new category
                categoryFromRepo = new QuestionCategory { QCategoryDesc = request.QuizCategoryDesc };
                isSuccess = await _quizRepository.AddCategoryAsync(categoryFromRepo);

                // Log the create category event
                LogCreateQuizCategoryEvent(categoryFromRepo, request, context);
            }

            // if update or create is not successful
            if (!isSuccess)
            {
                fail.Type = "500";
                fail.Message = $"Failed to create category with description {request.QuizCategoryDesc}.";
                response.CreateCategoryFail = fail;
            }
            else
            {
                // else, we save the changes
                await _quizRepository.SaveChangesAsync();
                success.Id = categoryFromRepo.Id;
                success.QuizCategoryDesc = categoryFromRepo.QCategoryDesc;

                response.CreatedCategoryReply = success;
            }

            return await Task.FromResult(response);
        }

        private void LogCreateQuizCategoryEvent(QuestionCategory category, CreateCategoryRequest request, ServerCallContext context)
        {
            // Capture the details of the user being deleted, including who deleted it
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            var createEvent = new CreateQuizCategoryEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Create Category",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Category {category.QCategoryDesc} created by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = "",
                NewValues = JsonConvert.SerializeObject(category),
            };

            var logRequest = new LogCreateQuizCategoryEventRequest
            {
                Event = createEvent
            };
            createEvent.NewValues = JsonConvert.SerializeObject(new
            {
                category.QCategoryDesc,
            });


            try
            {
                // Make the gRPC call to log the create category event
                _quizAuditServiceClient.LogCreateQuizCategoryEvent(logRequest);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the gRPC call and log them
                //logger.LogError($"Error while logging create category event: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<DeleteCategoryReply> DeleteCategory(DeleteCategoryRequest request, ServerCallContext context)
        {
            var reply = new DeleteCategoryReply() { Response = 203 };

            // Capture the details of the user deleting the category
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            // get the category
            var category = await _quizRepository.GetCategoryAsync(request.Id);

            // if category does not exist
            if(category == null)
            {
                reply.Response = 404;
                return await Task.FromResult(reply);
            }

            // update active data to false
            category.ActiveData = false;
            var result = _quizRepository.UpdateCategory(category);
            await _quizRepository.SaveChangesAsync();
            LogDeleteQuizCategoryEvent(category, userRoles!, userNameClaim!, userId!);

            // if update is not success
            if (!result)
            {
                reply.Response = 500;
            }
            return await Task.FromResult(reply);
        }

        private void LogDeleteQuizCategoryEvent(QuestionCategory category, string userRoles, string userName, string userId)
        {
            // Construct the delete event
            var deleteEvent = new DeleteQuizCategoryEvent
            {
                UserId = int.Parse(userId!),
                Username = userName,
                Action = "Delete Category",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Category {category.QCategoryDesc} deleted by: {userName}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(category),
                NewValues = "",
            };

            var logRequest = new LogDeleteQuizCategoryEventRequest
            {
                Event = deleteEvent
            };
            deleteEvent.OldValues = JsonConvert.SerializeObject(new
            {
                category.Id,
                category.QCategoryDesc
            });
            try
            {
                // Make the gRPC call to log the delete category event
                _quizAuditServiceClient.LogDeleteQuizCategoryEvent(logRequest);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the gRPC call
            }
        }


        /// <summary>
        /// Check if category ecist and is active
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CheckCategoryOrActiveOrFail> CheckCategoryOrActive(CheckCategoryOrActiveRequest request, ServerCallContext context)
        {
            var success = new CheckCategoryOrActiveReply();
            var response = new CheckCategoryOrActiveOrFail();

            // check if category exist
            var category = await _quizRepository.GetCategoryAsync(request.Id);

            // if category does not exist
            if (category == null)
            {
                response.CheckCategoryOrActiveFail = new CheckCategoryOrActiveFail() { Type = "404", Message = "Category not found" };
            }
            else 
            {
                success.Category = JsonConvert.SerializeObject(category);
                response.CheckCategoryOrActiveReply = success;
            }


            return await Task.FromResult(response);
        }

        /// <summary>
        /// Check category name if exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CheckCategoryNameReply> CheckCategoryName(CheckCategoryNameRequest request, ServerCallContext context)
        {
            var reply = new CheckCategoryNameReply() { Status = "404" };

            // check if category exist
            var category = await _quizRepository.GetCategoryAsync(request.QuizCategoryDesc);

            // if category exist
            if(category != null)
            {
                reply.Status = "200";
            }

            return await Task.FromResult(reply);
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<UpdateCategoryReply> UpdateCategory(UpdateCategoryRequest request, ServerCallContext context)
        {
            var reply = new UpdateCategoryReply();
            var category = JsonConvert.DeserializeObject<QuestionCategory>(request.Category);

            // update category
         
            var oldCategory = await _quizRepository.GetCategoryAsync(category.Id);
            var result = _quizRepository.UpdateCategory(category);
            // if update is not success
            if (!result)
            {
                
                reply.StatusCode = 500;
                reply.Id = 0;
                reply.QuizCategoryDesc = "";
            }

            // else, we save the changes
                await _quizRepository.SaveChangesAsync();
                LogUpdateQuizCategoryEvent(oldCategory, category, context);

                reply.StatusCode = 200;
                reply.Id = category.Id;
                reply.QuizCategoryDesc = category.QCategoryDesc;

            return await Task.FromResult(reply);
        }
      

        private void LogUpdateQuizCategoryEvent(QuestionCategory oldCategory, QuestionCategory newCategory, ServerCallContext context)
        {
            // Capture the details of the user updating the category
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            // Construct the update event
            var updateEvent = new UpdateQuizCategoryEvent
            {
                UserId = int.Parse(userId!),
                Username = userNameClaim,
                Action = "Update Category",
                Timestamp = DateTimeHelper.GetPhilippinesTimestamp(),
                Details = $"Category updated by: {userNameClaim}",
                Userrole = userRoles,
                OldValues = JsonConvert.SerializeObject(oldCategory),
                NewValues = JsonConvert.SerializeObject(newCategory),
            };

            var logRequest = new LogUpdateQuizCategoryEventRequest
            {
                Event = updateEvent
            };
            updateEvent.NewValues = JsonConvert.SerializeObject(new
            {
                newCategory.Id,
                newCategory.QCategoryDesc,
            });
            updateEvent.OldValues = JsonConvert.SerializeObject(new
            {
                oldCategory.Id,
                oldCategory.QCategoryDesc,
            });

            try
            {
                // Make the gRPC call to log the update category event
                 _quizAuditServiceClient.LogUpdateQuizCategoryEvent(logRequest);
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the gRPC call
    
            }
        }
    }
}
