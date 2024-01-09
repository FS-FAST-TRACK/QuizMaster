using Microsoft.Extensions.Options;
using QuizMaster.API.Account.Configuration;
using System.Net.Mail;
using System.Net;

namespace QuizMaster.API.Account.Service
{
    public class EmailSenderService
    {
        private readonly ApplicationSettings _settings;
        public EmailSenderService(IOptions<ApplicationSettings> appsettings)
        {
            _settings = appsettings.Value;
        }

        public void SendEmail(string email, string token)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_settings.SMTP_EMAIL, "🔎QuizMaster@no-reply", System.Text.Encoding.UTF8);
            mailMessage.To.Add(email);
            mailMessage.Subject = "Update Password like literal update";
            mailMessage.Body = $"<html><body>Confirm Update password: <a href='https://localhost:7081/gateway/api/account/update_password/{token}'>Click to update</a></body></html>";
            mailMessage.IsBodyHtml = true;

            // Create the credentials to login to the gmail account associated with my custom domain
            string sendEmailsFrom = _settings.SMTP_EMAIL;
            string sendEmailsFromPassword = _settings.SMTP_PASSWORD;
            NetworkCredential cred = new NetworkCredential(sendEmailsFrom, sendEmailsFromPassword);

            SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
            mailClient.EnableSsl = true;
            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mailClient.UseDefaultCredentials = false;
            mailClient.Timeout = 20000;
            mailClient.Credentials = cred;
            mailClient.Send(mailMessage);
        }
    }
}
