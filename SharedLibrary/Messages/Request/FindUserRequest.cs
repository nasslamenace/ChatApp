using System;
namespace SharedLibrary.Messages.Request
{
    [Serializable]
    public class FindUserRequest : Message
    {

        Guid sender;
        string username;

        public FindUserRequest(Guid sender, string username)
        {
            this.sender = sender;
            this.username = username;

        }


        public Guid Sender { get => sender; set => sender = value; }
        public string Username { get => username; set => username = value; }
    }
}
