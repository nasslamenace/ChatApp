using System;
namespace SharedLibrary.Messages.Request
{
    [Serializable]
    public class CreateRequest : Message
    {
        private Guid id;
        private string title;
        private string content;
        private DateTime when;

        public CreateRequest(Guid id, string title, string content, DateTime when)
        {
            this.id = id;
            this.title = title;
            this.content = content;
            this.when = when;
        }

        public Guid Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        public DateTime When { get => when; set => when = value; }
    }
}
