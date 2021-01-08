using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class RegisterResponse : Message
    {
        private string username;
        private string password;

        private string error;
        private bool hasError;



        public RegisterResponse(string error)
        {
            hasError = true;
            this.error = error;
            username = "";
            username = "";

        }

        public RegisterResponse(string username, string password)
        {
            hasError = false;
            this.username = username;
            this.password = password;
        }

        public string Password { get => password; set => password = value; }
        public string Username { get => username; set => username = value; }
        public string Error { get => error; set => error = value; }
        public bool HasError { get => hasError; set => hasError = value; }
    }
}
