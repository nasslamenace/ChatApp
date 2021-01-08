using System;
using System.Runtime.Serialization;

namespace SharedLibrary
{
    [Serializable]
    public class TopicMessage
    {
        private DateTime when;
        private string creator;
        private string text;
        private int topicId;

        public TopicMessage()
        {
        }

        public TopicMessage(DateTime when, string creator, string text, int topicId)
        {
            this.when = when;
            this.creator = creator;
            this.text = text;
            this.topicId = topicId;
        }

        public TopicMessage(SerializationInfo info, StreamingContext ctxt)
        {
            when = (DateTime)info.GetValue("when", typeof(DateTime));
            creator = (string)info.GetValue("creator", typeof(string));
            text = (string)info.GetValue("text", typeof(string));
            topicId = (int)info.GetValue("topicId", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("when", when);
            info.AddValue("creator", creator);
            info.AddValue("text", text);
            info.AddValue("topicId", topicId);
            //throw new NotImplementedException();
        }

        public DateTime When { get => when; set => when = value; }
        public string Creator { get => creator; set => creator = value; }
        public string Text { get => text; set => text = value; }
        public int TopicId { get => topicId; set => topicId = value; }
    }
}
