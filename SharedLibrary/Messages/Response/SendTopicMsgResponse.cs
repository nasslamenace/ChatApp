using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class SendTopicMsgResponse : Message
    {
        bool hasError;
        string error;

        public bool HasError { get => hasError; set => hasError = value; }
        public string Error { get => error; set => error = value; }

        public SendTopicMsgResponse(string error)
        {
            this.error = error;
            this.hasError = true;
        }
        public SendTopicMsgResponse()
        {
            this.hasError = false;
        }


    }
}
