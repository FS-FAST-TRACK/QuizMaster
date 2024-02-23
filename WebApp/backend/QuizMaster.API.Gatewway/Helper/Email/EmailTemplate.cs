namespace QuizMaster.API.Gateway.Helper.Email
{
    public class EmailTemplate
    {
        public string Body { get; set; } = string.Empty;
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
    }

    public class EmailDefaults
    {
        private const string WITH_STAR = "<img\r\nstyle=\"width: 42px\"\r\nsrc=\"https://github.com/jaymar921/Public-Repo/blob/main/quizmaster/star_review.png?raw=true\"\r\n/>";
        private const string WITH_NO_STAR = "<img\r\nstyle=\"width: 42px\"\r\nsrc=\"https://github.com/jaymar921/Public-Repo/blob/main/quizmaster/no_star_review.png?raw=true\"\r\n/>";
        public static EmailTemplate SUBMIT_REVIEW_ADMIN(string ToEmail, string Message, int Rating)
        {
            string stars = "";
            for(int i = 0; i < 5; i++)
            {
                if(i < Rating)
                {
                    stars += WITH_STAR;
                }
                else
                {
                    stars += WITH_NO_STAR;
                }
            }
            string body = @$"
<html>
  <body style=""background-color: #FFFFFF"">
    <div style=""width: 400px"">
      <img
        style=""width: 400px; height: 84px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/wave_fs_vector_1.png?raw=true""
      />
      <h3 style=""text-align: center; color: #18a44c"">
        A user submitted a feedback
      </h3>
      <img
        style=""width: 400px; height: 300px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/quizmaster/review_email_photo.png?raw=true""
      />
      <div style=""text-align: center"">
        {stars}
        <p style=""color: #706e6d; font-size: 12px"">
          Someone had given {Rating} out of 5 stars.
        </p>
      </div>
      <p style=""font-size: 14px; padding: 5px 20px 0px 20px; font-weight: 999"">
        Comment:
      </p>
      <div style=""padding: 0px 20px 5px 20px"">
        <p
          style=""
            font-size: 14px;
            padding: 10px 10px 10px 10px;
            color: #3c3c3c;
            background-color: #f8f9fa;
            border-radius: 8px;
          ""
        >
          ""{Message}""
        </p>
      </div>
      <hr />
      <p style=""padding: 0px 20px 0px 20px; font-size: 12px; color: gray"">
        Copyright 2023 Ⓒ QuizMaster
      </p>
    </div>
  </body>
</html>
";
            return new EmailTemplate { Body = body, ToEmail = ToEmail, Subject = "[QuizMaster] System Review" };
        }

        public static EmailTemplate SUBMIT_REVIEW_CLIENT(string ToEmail, string Message, int Rating)
        {
            string stars = "";
            for (int i = 0; i < 5; i++)
            {
                if (i < Rating)
                {
                    stars += WITH_STAR;
                }
                else
                {
                    stars += WITH_NO_STAR;
                }
            }
            string body = @$"
<html>
  <body style=""background-color: #FFFFFF"">
    <div style=""width: 400px"">
      <img
        style=""width: 400px; height: 84px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/wave_fs_vector_1.png?raw=true""
      />
      <h3 style=""text-align: center; color: #18a44c"">
        Thank you for the feedback
      </h3>
      <img
        style=""width: 400px; height: 300px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/quizmaster/review_email_photo.png?raw=true""
      />
      <div style=""text-align: center"">
        {stars}
        <p style=""color: #706e6d; font-size: 12px"">
          You have given {Rating} out of 5 stars.
        </p>
      </div>
      <p style=""font-size: 14px; padding: 5px 20px 0px 20px; font-weight: 999"">
        Comment:
      </p>
      <div style=""padding: 0px 20px 5px 20px"">
        <p
          style=""
            font-size: 14px;
            padding: 10px 10px 10px 10px;
            color: #3c3c3c;
            background-color: #f8f9fa;
            border-radius: 8px;
          ""
        >
          ""{Message}""
        </p>
      </div>
      <hr />
      <p style=""padding: 0px 20px 0px 20px; font-size: 12px; color: gray"">
        Copyright 2023 Ⓒ QuizMaster
      </p>
    </div>
  </body>
</html>
";
            return new EmailTemplate { Body = body, ToEmail = ToEmail, Subject = "[QuizMaster] System Review" };
        }

        public static EmailTemplate SUBMIT_CONTACT_ADMIN(string ToEmail, string Name, string Message)
        {
            string body = $@"
<html>
  <body style=""background-color: #FFFFFF"">
    <div style=""width: 400px"">
      <img
        style=""width: 400px; height: 84px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/wave_fs_vector_1.png?raw=true""
      />
      <h3 style=""text-align: center; color: #18a44c"">A user reached out.</h3>
      <img
        style=""width: 400px; height: 300px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/quizmaster/admin_contact_email_photo.png?raw=true""
      />
      <p style=""font-size: 14px; padding: 5px 20px 5px 20px; color: #3c3c3c"">
        Hello <a style=""font-size: 14px; font-weight: 999"">QuizMaster Team</a>,
        <br /><br />
        The user, <a style=""font-size: 14px; font-weight: 999"">{Name}</a>,
        reached out with the following message: <br /><br />
        ""{Message}""
      </p>
      <hr />
      <p style=""padding: 0px 20px 0px 20px; font-size: 12px; color: gray"">
        Copyright 2023 Ⓒ QuizMaster
      </p>
    </div>
  </body>
</html>
";
            return new EmailTemplate { Body = body, ToEmail = ToEmail, Subject = "[QuizMaster] Someone's reaching us!" };
        }

        public static EmailTemplate SUBMIT_CONTACT_CLIENT(string ToEmail)
        {
            string body = @$"
<html>
  <body style=""background-color: #FFFFFF"">
    <div style=""width: 400px"">
      <img
        style=""width: 400px; height: 84px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/wave_fs_vector_1.png?raw=true""
      />
      <h3 style=""text-align: center; color: #18a44c"">
        Thank you for reaching out!
      </h3>
      <img
        style=""width: 400px; height: 300px""
        src=""https://github.com/jaymar921/Public-Repo/blob/main/quizmaster/client_contact_email_photo.png?raw=true""
      />
      <p style=""font-size: 14px; padding: 5px 20px 5px 20px; color: #3c3c3c"">
        Thank you for reaching out to us. We will reach out to you as soon as we
        possibly can. <br /><br />Best regards,
      </p>
      <p style=""font-size: 14px; padding: 5px 20px 5px 20px; font-weight: 999"">
        QuizMaster Team
      </p>
      <hr />
      <p style=""padding: 0px 20px 0px 20px; font-size: 12px; color: gray"">
        Copyright 2023 Ⓒ QuizMaster
      </p>
    </div>
  </body>
</html>
";
            return new EmailTemplate { Body = body, ToEmail = ToEmail, Subject = "[QuizMaster] Thank you for reaching us!" };
        }
    }
}
