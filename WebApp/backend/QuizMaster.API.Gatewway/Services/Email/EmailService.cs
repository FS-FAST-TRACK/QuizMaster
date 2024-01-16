using System.Net.Mail;
using System.Net;
using QuizMaster.API.Gateway.Helper.Email;
using Microsoft.Extensions.Options;
using QuizMaster.API.Gateway.Configuration;

namespace QuizMaster.API.Gateway.Services.Email
{
    public class EmailService
    {
        private readonly string SMTP_EMAIL;
        private readonly string SMTP_PASS;
        private readonly ILogger<EmailService> logger;
        public EmailService(IOptions<ApplicationSettings> appSettings, ILogger<EmailService> logger) 
        {
            SMTP_EMAIL = appSettings.Value.SMTP_EMAIL;
            SMTP_PASS = appSettings.Value.SMTP_PASSWORD;
            this.logger = logger;
        }

        private void SendEmailRequest(EmailTemplate emailTemplate, bool toAdmin)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(SMTP_EMAIL, "🔎QuizMaster@no-reply", System.Text.Encoding.UTF8);
                if (toAdmin)
                {
                    mailMessage.To.Add(SMTP_EMAIL);
                }
                else
                {
                    mailMessage.To.Add(emailTemplate.ToEmail);
                }
                mailMessage.Subject = emailTemplate.Subject;
                mailMessage.Body = emailTemplate.Body;
                mailMessage.IsBodyHtml = true;

                // Create the credentials to login to the gmail account associated with my custom domain
                string sendEmailsFrom = SMTP_EMAIL;
                string sendEmailsFromPassword = SMTP_PASS;
                NetworkCredential cred = new NetworkCredential(sendEmailsFrom, sendEmailsFromPassword);

                SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.UseDefaultCredentials = false;
                mailClient.Timeout = 20000;
                mailClient.Credentials = cred;
                mailClient.Send(mailMessage);
            }catch(Exception err)
            {
                logger.LogError("Error Sending Email: " + err.Message);
            }
        }

        public void SendEmail(EmailTemplate emailTemplate)
        {
            SendEmailRequest(emailTemplate, false);
        }

        public void SendEmailToAdmin(EmailTemplate emailTemplate)
        {
            SendEmailRequest(emailTemplate, true);
        }


    }
}
