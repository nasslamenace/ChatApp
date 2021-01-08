using System;
using System.Collections.Generic;

namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class ListTopicResponse : Message
    {
        private List<Topic> topics;

        public ListTopicResponse(List<Topic> topics)
        {
            this.topics = topics;
        }

        public void display()
        {
            Console.WriteLine("Here are all the topics created : \n");
            for(int i = 0; i < topics.Count; i++)
            {
                Console.WriteLine((i + 1) + ") " + topics[i].Title);
            }
        }

        public List<Topic> Topics { get => topics; set => topics = value; }
    }
}
