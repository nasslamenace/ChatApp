using System;
namespace SharedLibrary.Messages
{
    [Serializable]
    public class RegisterRequest : Message
    {
        private string username;
        private string password;


        public RegisterRequest(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public string Password { get => password; set => password = value; }
        public string Username { get => username; set => username = value; }
    }
}
