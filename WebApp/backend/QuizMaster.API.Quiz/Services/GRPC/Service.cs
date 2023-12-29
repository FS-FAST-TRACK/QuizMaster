using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using QuizMaster.API.Monitoring.Proto;
using QuizMaster.API.Quiz.Models;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.Quiz.ResourceParameters;
using QuizMaster.API.Quiz.Services.Repositories;
using QuizMaster.Library.Common.Entities.Questionnaire;
using QuizMaster.Library.Common.Helpers;
using QuizMaster.Library.Common.Helpers.Quiz;

namespace QuizMaster.API.Quiz.Services.GRPC
{
    public class Service : QuizCategoryService.QuizCategoryServiceBase
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IMapper _mapper;
        private readonly QuizAuditService.QuizAuditServiceClient _quizAuditServiceClient;

        public Service(IQuizRepository quizRepository, IMapper mapper, QuizAuditService.QuizAuditServiceClient quizAuditServiceClient)
        {
            _quizRepository = quizRepository;
            _quizAuditServiceClient = quizAuditServiceClient;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CategoryResponse> GetAllQuizCatagory(CategoryRequest request, ServerCallContext context)
        {

            var reply = new CategoryResponse();

            var categories = await _quizRepository.GetAllCategoriesAsync();

            reply.Content = JsonConvert.SerializeObject(categories);

            reply.Code = 200;
            reply.Type = "categories";
            return reply;

        }

        public override async Task<CategoryResponse> GetCategories(CategoryRequest request, ServerCallContext context)
        {
            var reply = new CategoryResponse();

            var resourceParameter = new CategoryResourceParameter();
            try
            {
                resourceParameter = JsonConvert.DeserializeObject<CategoryResourceParameter>(request.Content);
                if (resourceParameter == null)
                {
                    throw new Exception("GRPC request content cannot be deserialized in to Tuple<int, JsonPatchDocument<DifficultyCreateDto>>.");
                }
            }
            catch (Exception ex)
            {
                reply.Content = ex.Message;
                reply.Code = 500;
                reply.Type = "string";
                return reply;
            }

            var categories = await _quizRepository.GetAllCategoriesAsync(resourceParameter);

            var paginationMetadata = categories.GeneratePaginationMetadata(null, null);

            reply.Content = JsonConvert.SerializeObject(new Tuple<IEnumerable<CategoryDto>, Dictionary<string, object?>>(categories, paginationMetadata));
            reply.Code = 200;
            reply.Type = "pagedCategories";


            return reply;
        }

        /// <summary>
        /// Get the category by id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CategoryResponse> GetCategoryById(CategoryRequest request, ServerCallContext context)
        {
            var reply = new CategoryResponse();
            int id = request.Id;


            try
            {
                var category = await _quizRepository.GetCategoryAsync(id);

                if (category == null || !category.ActiveData)
                {
                    reply.Code = 404;
                    reply.Content = "Difficulty not found";
                    reply.Type = "string";
                }
                else
                {
                    reply.Content = JsonConvert.SerializeObject(category);
                    reply.Code = 200;
                    reply.Type = "difficulty";

                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }
            return reply;
        }

        /// <summary>
        /// Create question category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CategoryResponse> CreateCategory(CategoryRequest request, ServerCallContext context)
        {
            var reply = new CategoryResponse();
            var category = new CategoryCreateDto();
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                category = JsonConvert.DeserializeObject<CategoryCreateDto>(request.Content);
                if (category == null)
                {
                    throw new Exception("Failed to deserialize GRPC request content into CategoryCreateDto.");
                }
                if (userId == null)
                {
                    throw new Exception("Create category requires UserId to be not null.");

                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }

            // Check if category description already exist
            var categoryFromRepo = await _quizRepository.GetCategoryAsync(category!.QCategoryDesc);

            if (categoryFromRepo != null && categoryFromRepo.ActiveData)
            {
                reply.Code = 409;
                reply.Content = "Difficulty already exist";
                reply.Type = "string";
            }

            bool isSuccess;

            // If category is not null and not active, we set active to true and update the categrory
            if (categoryFromRepo != null && !categoryFromRepo.ActiveData)
            {
                categoryFromRepo.ActiveData = true;
                // update category
                _mapper.Map(category, categoryFromRepo);

                // udpate necessary properties
                categoryFromRepo.DateUpdated = DateTime.UtcNow;
                categoryFromRepo.UpdatedByUserId = int.Parse(userId!);

                isSuccess = _quizRepository.UpdateCategory(categoryFromRepo);
            }
            // else, we create new category
            else
            {
                categoryFromRepo = _mapper.Map<QuestionCategory>(category);
                categoryFromRepo.CreatedByUserId = int.Parse(userId!);
                categoryFromRepo.DateCreated = DateTime.UtcNow;

                isSuccess = await _quizRepository.AddCategoryAsync(categoryFromRepo);
            }


            // Check if update or create is not access 
            if (!isSuccess)
            {
                reply.Content = "Failed to create category.";
                reply.Type = "string";
                reply.Code = 500;
                return reply;
            }

            await _quizRepository.SaveChangesAsync();

            // Log changes after success full saving changes 
            LogCreateQuizCategoryEvent(categoryFromRepo, request, context);

            reply.Content = JsonConvert.SerializeObject(categoryFromRepo);
            reply.Type = "category";
            reply.Code = 201;

            return reply;
        }

        private void LogCreateQuizCategoryEvent(QuestionCategory category, CategoryRequest request, ServerCallContext context)
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
        public override async Task<CategoryResponse> DeleteCategory(CategoryRequest request, ServerCallContext context)
        {
            var reply = new CategoryResponse();
            int id = request.Id;
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;
            // Capture the details of the user attempting to delete the difficulty
            var userRoles = context.RequestHeaders.FirstOrDefault(h => h.Key == "role")?.Value;
            var userNameClaim = context.RequestHeaders.FirstOrDefault(h => h.Key == "username")?.Value;
            try
            {
                if (userId == null)
                {
                    throw new Exception("Delete category requires UserId to be not null.");

                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
            }

            var category = await _quizRepository.GetCategoryAsync(id);
            if (category == null || !category.ActiveData)
            {
                reply.Code = 404;
                reply.Type = "string";
                reply.Content = "Category is not found.";
            }
            else
            {

                // Capture the current state of the category for audit logging
                var deletedCategory = new QuestionCategory
                {
                    QCategoryDesc = category.QCategoryDesc,
                    // Include other properties as needed
                };

                // Set ActiveData to false to "soft delete" the category
                category.ActiveData = false;

                // Set updatedby userId and DateUpdated to latest
                category.DateUpdated = DateTime.UtcNow;
                category.UpdatedByUserId = int.Parse(userId!);

                var isSuccess = _quizRepository.UpdateCategory(category);

                if (!isSuccess)
                {
                    reply.Code = 500;
                    reply.Content = "Failed to delete category. Database throws an error.";
                    reply.Type = "string";
                    return reply;
                }


                reply.Code = 200;
                reply.Content = "Succesfully Deleted Category";
                reply.Type = "string";


                await _quizRepository.SaveChangesAsync();

                // Log the delete category event with the old values
                LogDeleteQuizCategoryEvent(category, userRoles!, userNameClaim!, userId!);
            }

            return reply;

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
        public override async Task<CategoryResponse> CheckCategoryOrActive(CategoryRequest request, ServerCallContext context)
        {
            var reply = new CategoryResponse()
            {
                Type = "string"
            };

            // check if category exist
            var category = await _quizRepository.GetCategoryAsync(request.Id);

            // if category does not exist
            if (category == null)
            {
                reply.Code = 404;
                reply.Content = "Category not found.";

            }
            else
            {
                reply.Code = 200;
                reply.Content = JsonConvert.SerializeObject(category);
            }

            return reply;
        }

        /// <summary>
        /// Check category name if exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CategoryResponse> CheckCategoryName(CategoryRequest request, ServerCallContext context)
        {
            var reply = new CategoryResponse() { Code = 400 };

            // check if category exist
            var category = await _quizRepository.GetCategoryAsync(request.Content);

            // if category exist
            if (category != null)
            {
                reply.Code = 200;
            }
            return reply;
        }

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CategoryResponse> UpdateCategory(CategoryRequest request, ServerCallContext context)
        {
            var reply = new CategoryResponse();
            int id = request.Id;
            var patch = new JsonPatchDocument<CategoryCreateDto>();
            var userId = context.RequestHeaders.FirstOrDefault(h => h.Key == "id")?.Value;

            try
            {
                patch = JsonConvert.DeserializeObject<JsonPatchDocument<CategoryCreateDto>>(request.Content);
                if (patch == null)
                {
                    throw new Exception("GRPC request content cannot be deserialized in to JsonPatchDocument<CategoryCreateDto>.");
                }
                if (userId == null)
                {
                    throw new Exception("User Id is not found in the GRPC context.");

                }
            }
            catch (Exception ex)
            {
                reply.Code = 500;
                reply.Content = ex.Message;
                reply.Type = "string";
                return reply;
            }



            var checkCategoryId = await _quizRepository.GetCategoryAsync(id);

            if (checkCategoryId == null || !checkCategoryId.ActiveData)
            {
                reply.Code = 404;
                reply.Content = "Category is not found";
                reply.Type = "string";
                return reply;
            }

            // Capture the old category state manually
            var oldCategory = new QuestionCategory
            {
                QCategoryDesc = checkCategoryId.QCategoryDesc,
                // Map other properties as needed
            };

            try
            {
                var categoryPatch = _mapper.Map<CategoryCreateDto>(checkCategoryId);
                patch.ApplyTo(categoryPatch);

                // Check if the QCategoryDesc already exists
                var existingCategory = await _quizRepository.GetCategoryAsync(categoryPatch.QCategoryDesc);
                if (existingCategory != null && existingCategory.Id != id)
                {
                    reply.Code = 409;
                    reply.Content = $"Difficulty \'{categoryPatch.QCategoryDesc}\' already exist.";
                    return reply;
                }



                // Update Properties
                _mapper.Map(categoryPatch, checkCategoryId);

                // Update other properties as needed
                checkCategoryId.UpdatedByUserId = int.Parse(userId!);
                checkCategoryId.DateUpdated = DateTime.UtcNow;

                var isSuccess = _quizRepository.UpdateCategory(checkCategoryId);

                if (!isSuccess)
                {
                    reply.Code = 500;
                    reply.Content = "Failed to update Category";
                    reply.Type = "string";
                    return reply;
                }
                await _quizRepository.SaveChangesAsync();

                reply.Code = 200;
                reply.Content = JsonConvert.SerializeObject(checkCategoryId);
                await _quizRepository.SaveChangesAsync();
                LogUpdateQuizCategoryEvent(oldCategory, checkCategoryId, context);
                return reply;

            }
            catch (Exception)
            {

                reply.Content = "Something went wrong.";
                reply.Type = "string";
            }

            return reply;
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
