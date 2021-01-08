using System;
namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class JoinTopicResponse : Message
    {
        private Topic topic;
        private string error;
        private bool hasError;

        public JoinTopicResponse(Topic topic)
        {
            this.topic = topic;
            this.hasError = false;

        }

        public JoinTopicResponse(string error)
        {
            this.error = error;
            this.hasError = true;
        }

        public void display()
        {
            Console.WriteLine("Topic title : " + topic.Title);
            Console.WriteLine("Topic description :" + topic.Content);

            Console.WriteLine("Chat : ");

            topic.displayMessages();
        }

        

        public Topic Topic { get => topic; set => topic = value; }
        public string Error { get => error; set => error = value; }
        public bool HasError { get => hasError; set => hasError = value; }
    }
}
