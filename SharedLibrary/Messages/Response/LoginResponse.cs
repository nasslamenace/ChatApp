using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class LoginResponse : Message
    {
        bool isLogged;
        Guid id;
        User user;

        public LoginResponse(bool isLogged, User user)
        {
            this.isLogged = isLogged;
            this.user = user;
        }
        public LoginResponse(bool isLogged)
        {
            this.isLogged = isLogged;
        }

        public bool IsLogged { get => isLogged; set => isLogged = value; }
        public Guid Id { get => id; set => id = value; }
        public User User { get => user; set => user = value; }
    }
}
