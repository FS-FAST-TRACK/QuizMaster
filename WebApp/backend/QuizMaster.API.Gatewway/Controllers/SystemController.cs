using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using QuizMaster.API.Authentication.Models;
using QuizMaster.API.Authentication.Proto;
using QuizMaster.API.Gateway.Attributes;
using QuizMaster.API.Gateway.Configuration;
using QuizMaster.API.Gateway.Helper;
using QuizMaster.API.Gateway.Helper.Email;
using QuizMaster.API.Gateway.Models.Report;
using QuizMaster.API.Gateway.Models.System;
using QuizMaster.API.Gateway.Services.Email;
using QuizMaster.API.Gateway.Services.ReportService;
using QuizMaster.API.Gateway.Services.SystemService;
using QuizMaster.API.Gatewway.Controllers;

namespace QuizMaster.API.Gateway.Controllers
{
    [Route("gateway/api/[controller]")]
    public class SystemController : Controller
    {
        private readonly SystemRepository systemRepository;
        private readonly EmailService emailService;
        private readonly GrpcChannel _authChannel;
        private readonly AuthService.AuthServiceClient _authChannelClient;
        private readonly ILogger<SystemController> logger;
        private readonly ReportRepository reportRepository;

        public SystemController(SystemRepository systemRepository, EmailService emailService, IOptions<GrpcServerConfiguration> options, ILogger<SystemController> logger, ReportRepository reportRepository)
        {
            this.systemRepository = systemRepository;
            this.emailService = emailService;

            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            _authChannel = GrpcChannel.ForAddress(options.Value.Authentication_Service, new GrpcChannelOptions { HttpHandler = handler });
            _authChannelClient = new AuthService.AuthServiceClient(_authChannel);
            this.logger = logger;
            this.reportRepository = reportRepository;
        }

        // Get System About
        [HttpGet("information")]
        public async Task<IActionResult> GetSystemInformationAsync()
        {
            return Ok(new { Status = "Success", Message = "Retrieved System Information", Data = await systemRepository.GetSystemAboutAsync() });
        }

        // Update System About
        [QuizMasterAdminAuthorization]
        [HttpPost("information")]
        public async Task<IActionResult> UpdateSystemInformationAsync([FromBody] AboutModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await systemRepository.EditSystemAboutAsync(model);
            return Ok(new { Status = "Success", Message = "Saved", Data = await systemRepository.GetSystemAboutAsync() });
        }

        // Get System Contact
        [HttpGet("contact_information")]
        public async Task<IActionResult> GetContactInformationAsync()
        {
            return Ok(new { Status = "Success", Message = "Retrieved Contact Information", Data = await systemRepository.GetContactInformationAsync() });
        }

        // Update System Contact
        [QuizMasterAdminAuthorization]
        [HttpPost("contact_information")]
        public async Task<IActionResult> UpdateContactInformation([FromBody] ContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await systemRepository.EditSystemContactInformationAsync(model);
            return Ok(new { Status = "Success", Message = "Saved", Data = await systemRepository.GetContactInformationAsync() });
        }

        // Submit Contact
        [HttpPost("reachOut")]
        public async Task<IActionResult> ReachOutAsync([FromBody] SubmitContactModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await systemRepository.SaveReachingContactAsync(model);

            #region Send Email
            // I'll run this in a thread
            var contactInformation = await systemRepository.GetContactInformationAsync();
            Task.Run(async () =>
            {
                // Lets try sending the email the the user authenticated, otherwise we'll send it to the email specified in the model
                var token = this.GetToken();
                var email = model.Email;
                if (!string.IsNullOrEmpty(token))
                {
                    var authStore = await GetAuthStoreInfo(token);
                    if (authStore != null) email = authStore.UserData.Email;
                }

                // Create the templates and send the corresponding emails
                var clientEmail = EmailDefaults.SUBMIT_CONTACT_CLIENT(email);
                emailService.SendEmail(clientEmail);

                logger.LogInformation("Sending Email Copy to: " + contactInformation.Email);
                var registeredAdminContactCopy = EmailDefaults.SUBMIT_CONTACT_ADMIN(contactInformation.Email, model.Firstname + " " + model.Lastname, model.Message);
                emailService.SendEmail(registeredAdminContactCopy);
                emailService.SendEmailToAdmin(registeredAdminContactCopy);
            });
            #endregion



            return Ok(new { Status = "Success", Message = "Successfully submitted a contact request." });
        }

        // Get All Submitted Contact
        [QuizMasterAdminAuthorization]
        [HttpGet("reachOut")]
        public async Task<IActionResult> GetAllReachOutAsync()
        {
            return Ok(new { Status = "Success", Message = "Retrieved Contacting Users", Data = await systemRepository.GetContactReachingsAsync() });
        }

        // Submit Review
        [HttpPost("review")]
        public async Task<IActionResult> SubmitReviewAsync([FromBody] ReviewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await systemRepository.SaveReviewsAsync(model);

            #region Send Email
            var contactInformation = await systemRepository.GetContactInformationAsync();
            // I'll run this in a separate thread
            Task.Run(async () =>
            {
                var token = this.GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    var AuthStore = await GetAuthStoreInfo(token);

                    if (AuthStore != null)
                    {
                        var clientEmail = EmailDefaults.SUBMIT_REVIEW_CLIENT(AuthStore.UserData.Email, model.Comment, model.StarRating);
                        emailService.SendEmail(clientEmail);

                    }
                }
                /*
                 * 
                 * This code right here will send an email to the SMTP account, which I commented it out first since we priority on
                 * the admin that will receive the email must be the email specified in the contact us
                var adminEmailCopy = EmailDefaults.SUBMIT_REVIEW_ADMIN("", model.Comment, model.StarRating);
                emailService.SendEmailToAdmin(adminEmailCopy);
                */
                var registeredAdminContactCopy = EmailDefaults.SUBMIT_REVIEW_ADMIN(contactInformation.Email, model.Comment, model.StarRating);
                logger.LogInformation("Sending Email Copy to: " + contactInformation.Email);
                emailService.SendEmail(registeredAdminContactCopy);
                emailService.SendEmailToAdmin(registeredAdminContactCopy);
            });
            #endregion

            return Ok(new { Status = "Success", Message = "Successfully submitted a system review." });
        }

        // Get All Submitted Review
        [QuizMasterAdminAuthorization]
        [HttpGet("review")]
        public async Task<IActionResult> GetAllReviewsAsync() 
        {
            return Ok(new { Status = "Success", Message = "Retrieved System Reviews", Data = await systemRepository.GetReviewsAsync() });
        }

        [HttpGet("review/client")]
        public async Task<IActionResult> GetAllReviewsClientCopyAsync()
        {
            var data = await systemRepository.GetReviewsAsync();
            data = data.Where(r => r.StarRating >= 4).ToList();
            return Ok(new { Status = "Success", Message = "Retrieved System Reviews", Data = data });
        }

        [QuizMasterAdminAuthorization]
        [HttpGet("quiz_reports")]
        public IActionResult GetAllReportsAsync()
        {
            var data = reportRepository.GetQuizReports();
            foreach(var report in data)
            {
                report.ParticipantAnswerReports = JsonConvert.DeserializeObject<IEnumerable<ParticipantAnswerReport>>(report.ParticipantAnswerReportsJSON);
                report.LeaderboardReports = JsonConvert.DeserializeObject<IEnumerable<LeaderboardReport>>(report.LeaderboardReportsJSON);
            }
            return Ok(new { Status = "Success", Message = "Retrieved System Reports", Data = data});
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
    }
}
