namespace QuizMaster.API.Account.Service
{
    public class PasswordHandler
    {
        private readonly IDictionary<string, IDictionary<string, string>> _passwordHolder;
        public PasswordHandler() { 
            _passwordHolder = new Dictionary<string, IDictionary<string, string>>();
        }

        public string GenerateToken(string userId,string currentPassword, string newPassword)
        {
            string token = Guid.NewGuid().ToString();
            IDictionary<string, string> Passwords = new Dictionary<string, string>();
            Passwords["userId"] = userId;
            Passwords["currentPassword"] = currentPassword;
            Passwords["newPassword"] = newPassword;
            _passwordHolder[token] = Passwords;
            return token;
        }

        public (string, string, string) GetPassword(string guid) 
        {
            if(_passwordHolder.ContainsKey(guid))
            {
                var map = _passwordHolder[guid];
                return (map["userId"], map["currentPassword"], map["newPassword"]);
            }
            return (string.Empty, string.Empty, string.Empty);
        }
    }
}
