using System;
namespace SharedLibrary.Messages.Request
{
    [Serializable]
    public class JoinTopicRequest : Message
    {
        private int id;

        public JoinTopicRequest(int id)
        {
            this.id = id;
        }

        public int Id { get => id; set => id = value; }
    }
}
