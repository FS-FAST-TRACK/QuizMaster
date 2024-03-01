using AutoMapper;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Proto;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Attributes;
using QuizMaster.API.Gateway.Hubs;
using QuizMaster.API.Gateway.Services;
using QuizMaster.API.Quiz.Protos;
using QuizMaster.API.QuizSession.Protos;
using QuizMaster.Library.Common.Entities.Rooms;
using QuizMaster.Library.Common.Models.QuizSession;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using QuizMaster.API.Gateway.Helper;
using QuizMaster.Library.Common.Entities.Roles;

namespace QuizMaster.API.Gateway.Controllers
{
    [ApiController]
    [Route("gateway/api")]
    public class QuizSetGatewayController : Controller
    {
        private readonly GrpcChannel _channel;
        private readonly QuizSetService.QuizSetServiceClient _channelClient;
        private readonly QuizRoomService.QuizRoomServiceClient roomChannelClient;
        private SessionHandler SessionHandler;
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;

        public QuizSetGatewayController(IOptions<GrpcServerConfiguration> options, SessionHandler sessionHandler)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _channel = GrpcChannel.ForAddress(options.Value.Session_Service, new GrpcChannelOptions { HttpHandler = handler });
            _channelClient = new QuizSetService.QuizSetServiceClient(_channel);
            _channel = GrpcChannel.ForAddress(options.Value.Session_Service, new GrpcChannelOptions { HttpHandler = handler });
            roomChannelClient = new QuizRoomService.QuizRoomServiceClient(_channel);
            SessionHandler = sessionHandler;
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service, new GrpcChannelOptions { HttpHandler = handler });
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
        }

        [QuizMasterAdminAuthorization]
        [HttpPost("set/create")]
        public async Task<IActionResult> CreateSet([FromBody] SetDTO setDTO)
        {
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return Unauthorized(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return Unauthorized(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";

            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            var request = new QuizSetRequest { QuizSet = JsonConvert.SerializeObject(setDTO) }; 
            var response = await _channelClient.AddQuizSetAsync(request, headers);

            if(response.Code == 404)
            { return NotFound(response.Message); }

            if (response.Code == 409)
            { return Conflict(response.Message); }

            if (response.Code == 500)
            { return BadRequest(response.Message); }

            return Ok(JsonConvert.DeserializeObject<Set>( response.Data));
        }

        [HttpPost("room/submitAnswer")]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDTO answerDTO)
        {
            string Message = await SessionHandler.SubmitAnswer(roomChannelClient, answerDTO.ConnectionId, answerDTO.QuestionId, answerDTO.Answer);
            return Ok(new {  Message });
        }

        [HttpPost("room/submitScreenshot")]
        public IActionResult SubmitScreenshot([FromBody] SubmitScreenshotDTO screenshotDTO)
        {
            string Message = SessionHandler.SubmitScreenshot(roomChannelClient, screenshotDTO.ConnectionId, screenshotDTO.QuestionId, screenshotDTO.ScreenshotLink);
            return Ok(new { Message });
        }

        [QuizMasterAdminAuthorization]
        [HttpGet("set/all_set")]
        public async Task<IActionResult> GetAllSet()
        {
            var response = await _channelClient.GetAllQuizSetAsync(new QuizSetEmpty());
            
            if(response.Code == 500)
            {
                return BadRequest(response.Message);
            }

            return Ok(JsonConvert.DeserializeObject<object[]>(response.Data));
        }

        [QuizMasterAdminAuthorization]
        [HttpGet("set/{id}")]
        public async Task<IActionResult> GetSet(int id)
        {
            var request = new GetQuizSetRequest { Id = id };
            var reply = await _channelClient.GetQuizSetAsync(request);

            if(reply.Code == 404)
            { return NotFound(reply.Message);}

            if(reply.Code == 500)
            { return BadRequest(reply.Message); }

            return Ok(JsonConvert.DeserializeObject<Set>(reply.Data));
        }

        [QuizMasterAdminAuthorization]
        [HttpGet("set/all_question_set")]
        public async Task<IActionResult> GetAllQuestionSet()
        {
            var response = await _channelClient.GetAllQuestionSetAsync(new QuizSetEmpty());

            if(response.Code == 500)
            { return BadRequest(response.Message); }

            return Ok(JsonConvert.DeserializeObject<QuestionSet[]>(response.Data));
        }

        [QuizMasterAdminAuthorization]
        [HttpGet("set/get_question_set/{id}")]
        public async Task<IActionResult> GetQuestionSet(int id)
        {
            var request = new GetQuizSetRequest { Id = id };
            var reply  = await _channelClient.GetQuestionSetAsync(request);

            if (reply.Code == 404)
            { return NotFound(reply.Message); }

            if (reply.Code == 500)
            { return BadRequest(reply.Message); }

            return Ok(JsonConvert.DeserializeObject<QuestionSet[]>(reply.Data));
        }

        [QuizMasterAdminAuthorization]
        [HttpPut("set/update_set/{id}")]
        public async Task<IActionResult> UpdateSet(int id, [FromBody] SetDTO setDTO)
        {
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return Unauthorized(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return Unauthorized(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";

            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            var request = new QuizSetRequest { Id = id, QuizSet = JsonConvert.SerializeObject(setDTO) };
            var reply = await _channelClient.UpdateQuizSetAsync(request, headers);

            if(reply.Code == 404)
            { return NotFound(reply.Message); }

            if(reply.Code == 500)
            { return BadRequest(reply.Message); }

            return Ok(JsonConvert.DeserializeObject<Set>(reply.Data));
        }

        [QuizMasterAdminAuthorization]
        [HttpDelete("set/delete_set/{id}")]
        public async Task<IActionResult> DeleteSet(int id)
        {
            var info = await ValidateUserTokenAndGetInfo();

            if (info == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            if (info == null || info.UserData == null)
            {
                return NotFound(new { Message = "Invalid user information in the token" });
            }

            var userName = info.UserData.UserName;
            var userId = info.UserData.Id;
            var userRole = info.Roles.Any(h => h.Equals("Administrator")) ? "Administrator" : "User";

            var headers = new Metadata
            {
                { "username", userName ?? "unknown" },
                { "id", userId.ToString() ?? "unknown" },
                { "role", userRole }
            };

            var request = new GetQuizSetRequest { Id = id };
            var reply = await _channelClient.DeleteQuizSetAsync(request, headers);

            if (reply.Code == 404)
            { return NotFound(reply.Message); }

            if (reply.Code == 500)
            { return BadRequest(reply.Message); }

            return Ok(reply.Message);
        }

        private async Task<AuthStore> ValidateUserTokenAndGetInfo()
        {
            string token = GetUserToken();

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var info = await GetAuthStoreInfo(token);

            if (info == null || info.UserData == null)
            {
                return null;
            }

            return info;
        }

        private string GetUserToken()
        {
            var tokenClaim = User.Claims.FirstOrDefault(e => e.Type == "token");

            if (tokenClaim != null)
            {
                return tokenClaim.Value;
            }

            try
            {
                return HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
            }
            catch
            {
                return null;
            }
        }

        private async Task<AuthStore> GetAuthStoreInfo(string token)
        {
            var requestValidation = new ValidationRequest()
            {
                Token = token
            };

            var authStore = await _authChannelClient.ValidateAuthenticationAsync(requestValidation);

            return !string.IsNullOrEmpty(authStore?.AuthStore) ? JsonConvert.DeserializeObject<AuthStore>(authStore.AuthStore) : null;
        }

        [QuizMasterAdminAuthorization]
        [HttpPost("room/createRoom")]
        public async Task<IActionResult> CreateRoom(CreateRoomDTO roomDTO)
        {
            var request = new CreateRoomRequest { Room = JsonConvert.SerializeObject(roomDTO) };
            var token = this.GetToken();
            AuthStore authStore = await GetAuthStoreInfo(token ?? "");
            Metadata? headers = null;
            try
            {
                headers = new Metadata
                {
                    { "username", authStore.UserData.UserName ?? "unknown" },
                    { "id", authStore.UserData.Id.ToString() ?? "unknown" },
                    { "role",  authStore.Roles.ToString() ?? "unknown" }
                };
            }
            catch
            {
                headers = new Metadata
                {
                    { "username", "unknown" },
                    { "id", "unknown" },
                    { "role", "unknown" }
                };
            }
            var reply = await roomChannelClient.CreateRoomAsync(request, headers);

            if (reply.Code == 200)
            {
                var quizRoom = JsonConvert.DeserializeObject<QuizRoom>(reply.Data);
                return Created("createRoom", new { Message = "Room was created", Data = quizRoom });
            }
            return BadRequest(new { Message = "Failed to create room", gRPC_Reply = reply });
        }

        [QuizMasterAdminAuthorization]
        [HttpGet("room/proceed/{id}")]
        public IActionResult RoomProceedNextRound(int id)
        {
            SessionHandler.SetPauseRoom(id, false);
            return Ok(new { Message = "Proceed Success"});
        }

        [QuizMasterAuthorization]
        [HttpGet("room/getAllRooms")]
        public async Task<IActionResult> GetAllRoomsAsync()
        {
            try
            {
                var roomResponse = await roomChannelClient.GetAllRoomAsync(new RoomsEmptyRequest());

                if (roomResponse.Code == 200)
                {
                    var quizRooms = JsonConvert.DeserializeObject<QuizRoom[]>(roomResponse.Data);

                    var quizRoomList = new List<object>();
                    foreach (var room in quizRooms)
                    {
                        var setQuizRoom = JsonConvert.DeserializeObject<IEnumerable<SetQuizRoom>>((await roomChannelClient.GetQuizSetAsync(new SetRequest() { Id = room.Id })).Data);
                        object roomObject = new
                        {
                            id = room.Id,
                            qRoomDesc = room.QRoomDesc,
                            qRoomPin = room.QRoomPin,
                            roomOptions = JsonConvert.DeserializeObject<IEnumerable<string>>(room.RoomOptions),
                            activeData = room.ActiveData,
                            dateCreated = room.DateCreated,
                            dateUpdated = room.DateUpdated,
                            createdByUserId = room.CreatedByUserId,
                            updatedByUserId = room.UpdatedByUserId,
                            set = setQuizRoom
                        };
                        quizRoomList.Add(roomObject);
                    }
                    return Ok(new { Message = "Retrieved Rooms", Data = quizRoomList });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to retrieve rooms: Error : " + ex.Message });
            }

            return Ok(new { Message = "No rooms to retrieve"});
        }

        [QuizMasterAdminAuthorization]
        [HttpDelete("room/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var roomResponse = await roomChannelClient.DeactivateRoomRequestAsync(new DeactivateRoom() { Id = id });

                if(roomResponse.Code == 200)
                {
                    return Ok(new { Message = "Room was deleted"});
                }
                else
                {
                    return BadRequest(new { Message = "Room was not deleted" });
                }
            }
            catch
            {
                return BadRequest(new { Message = "Room was not deleted" });
            }
        }

        [QuizMasterAuthorization]
        [HttpGet("room/getRoomByPin/{RoomPin}")]
        public async Task<IActionResult> GetRoomByPinAsync(int RoomPin, bool isId = false, bool isActive = true)
        {
            try
            {
                var roomResponse = await roomChannelClient.GetAllRoomAsync(new RoomsEmptyRequest());

                if (roomResponse.Code == 200)
                {
                    var quizRooms = JsonConvert.DeserializeObject<QuizRoom[]>(roomResponse.Data);

                    bool containsId = false;
                    var room = new QuizRoom();
                    object? roomObject = null;
                    foreach (QuizRoom rooms in quizRooms)
                    {
                        if ((rooms.QRoomPin == RoomPin && !isId) || (rooms.Id == RoomPin && isId) && rooms.ActiveData == isActive)
                        {
                            room = rooms;
                            containsId = true;
                            var setQuizRoom = JsonConvert.DeserializeObject<IEnumerable<SetQuizRoom>>((await roomChannelClient.GetQuizSetAsync(new SetRequest() { Id = room.Id })).Data);
                            roomObject = new
                            {
                                id = room.Id,
                                qRoomDesc = room.QRoomDesc,
                                qRoomPin = room.QRoomPin,
                                roomOptions = JsonConvert.DeserializeObject<IEnumerable<string>>(room.RoomOptions),
                                activeData = room.ActiveData,
                                dateCreated = room.DateCreated,
                                dateUpdated = room.DateUpdated,
                                createdByUserId = room.CreatedByUserId,
                                updatedByUserId = room.UpdatedByUserId,
                                set = setQuizRoom
                            };
                            break;
                        }
                    }

                    if (containsId)
                    {
                        return Ok( new { Message = $"Retrieved Room {room.QRoomDesc}", Data = roomObject });
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Failed to retrieve room: Error : " + ex.Message });
            }
            return Ok(new { Message = "No room to retrieve" });
        }

        [QuizMasterAuthorization]
        [HttpGet("room/getRoomById/{RoomId}")]
        public async Task<IActionResult> GetRoomByIdAsync(int RoomId, bool isActive = true)
        {
            return await GetRoomByPinAsync(RoomId, true, isActive: isActive);
        }
    }
}
