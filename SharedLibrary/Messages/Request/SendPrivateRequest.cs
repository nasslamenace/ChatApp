using System;
namespace SharedLibrary.Messages.Request
{
    [Serializable]
    public class SendPrivateRequest : Message
    {
        PrivateMessage message;

        public SendPrivateRequest(PrivateMessage message)
        {
            this.message = message;
        }

        public PrivateMessage Message { get => message; set => message = value; }
    }
}
