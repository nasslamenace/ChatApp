using System;
namespace SharedLibrary.Messages.Request
{
    [Serializable]
    public class SendTopicMsgRequest : Message
    {
        TopicMessage message;
      
      

        public SendTopicMsgRequest(TopicMessage message)
        {
            this.message = message;
        }

        public TopicMessage Message { get => message; set => message = value; }
    }
}
