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
        public static EmailTemplate SUBMIT_REVIEW_ADMIN(string ToEmail, string Message, int Rating)
        {
            return new EmailTemplate { Body = $"Someone reviewed: {Message}, Rating: {Rating}/5", ToEmail = ToEmail, Subject = "[QuizMaster] System Review" };
        }

        public static EmailTemplate SUBMIT_REVIEW_CLIENT(string ToEmail, string Message, int Rating)
        {
            return new EmailTemplate { Body = $"You reviewed: {Message}, Rating: {Rating}/5", ToEmail = ToEmail, Subject = "[QuizMaster] System Review" };
        }

        public static EmailTemplate SUBMIT_CONTACT_ADMIN(string ToEmail, string Name, string Message)
        {
            return new EmailTemplate { Body = $"Hello admin, this {Name} would like to reach us out with a following message: {Message}", ToEmail = ToEmail, Subject = "[QuizMaster] Someone's reaching us" };
        }

        public static EmailTemplate SUBMIT_CONTACT_CLIENT(string ToEmail)
        {
            return new EmailTemplate { Body = $"Thank you for reaching us, we will reach you out soonest.", ToEmail = ToEmail, Subject = "[QuizMaster] Thanks for reaching us" };
        }
    }
}
