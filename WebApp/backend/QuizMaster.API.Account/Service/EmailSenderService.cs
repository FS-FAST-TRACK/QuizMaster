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

        public void SendEmail(string email, string firstname, string token)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_settings.SMTP_EMAIL, "🔎QuizMaster@no-reply", System.Text.Encoding.UTF8);
            mailMessage.To.Add(email);
            mailMessage.Subject = "QuizMaster Password Change Request";
            mailMessage.Body = @$"
<html><body>
    <div style=""width: 400px"">
      <img
        style=""width: 400px; height: 84px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/wave_fs_vector_1.png?raw=true""
      />
      <h3 style=""text-align: center; color: #18a44c"">
        Change Password Confirmation
      </h3>
      <img
        style=""width: 400px; height: 300px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/reset_pass_fs.png?raw=true""
      />
      <h3 style=""font-size: 18px; padding: 5px 20px 5px 20px"">Hello {firstname}</h3>
      <p style=""font-size: 14px; padding: 5px 20px 5px 20px"">
        You are receiving this email because you requested a change on your
        password. Click the button below to change your password.
      </p>
      <div style=""padding: 5px 20px 5px 20px; text-align: center"">
        <a
          style=""
            background-color: #18a44c;
            color: white;
            padding: 10px;
            font-size: 14px;
            outline: none;
            border: none;
            border-radius: 4px;
            width: 500px;
            cursor: pointer;
            text-decoration: none;
          ""
          href='{_settings.GatewayHost}/gateway/api/account/update_password/{token}'
        >
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Confirm Change
          Password&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </a>
      </div>
      <p style=""padding: 0px 20px 0px 20px; font-size: 12px; color: gray"">
        If you did not request a password change, you can ignore this email.
      </p>
      <hr />
      <p style=""padding: 0px 20px 0px 20px; font-size: 12px; color: gray"">
        Copyright 2023 Ⓒ QuizMaster
      </p>
    </div>
</body></html>
";
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

        public void SendEmailResetPassword(string email, string firstname, string token, string generatedPassword)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_settings.SMTP_EMAIL, "🔎QuizMaster@no-reply", System.Text.Encoding.UTF8);
            mailMessage.To.Add(email);
            mailMessage.Subject = "QuizMaster Password Reset";
            mailMessage.Body = @$"
<html><body>
    <div style=""width: 400px"">
      <img
        style=""width: 400px; height: 84px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/wave_fs_vector_1.png?raw=true""
      />
      <h3 style=""text-align: center; color: #18a44c"">
        Reset Password Confirmation
      </h3>
      <img
        style=""width: 400px; height: 300px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/reset_pass_fs.png?raw=true""
      />
      <h3 style=""font-size: 18px; padding: 5px 20px 5px 20px"">Hello {firstname}</h3>
      <p style=""font-size: 14px; padding: 5px 20px 5px 20px"">
        You are receiving this email because you requested a reset on your
        password. Click the button below to reset your password.
      </p>
      <div style=""padding: 5px 20px 5px 20px; text-align: center"">
        <a
          style=""
            background-color: #18a44c;
            color: white;
            padding: 10px;
            font-size: 14px;
            outline: none;
            border: none;
            border-radius: 4px;
            width: 500px;
            cursor: pointer;
            text-decoration: none;
          ""
          href='{_settings.GatewayHost}/gateway/api/account/update_password/{token}'
        >
          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Confirm Reset
          Password&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </a>
        <p>Generated Password: {generatedPassword}</p>
      </div>
      <p style=""padding: 0px 20px 0px 20px; font-size: 12px; color: gray"">
        If you did not request a password reset, you can ignore this email.
      </p>
      <hr />
      <p style=""padding: 0px 20px 0px 20px; font-size: 12px; color: gray"">
        Copyright 2023 Ⓒ QuizMaster
      </p>
    </div>
</body></html>
";
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
