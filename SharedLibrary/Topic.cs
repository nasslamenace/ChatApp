using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharedLibrary
{
    [Serializable]
    public class Topic
    {

        private int id;
        private Guid creator;
        private string title;
        private string content;
        private DateTime when;
        private List<TopicMessage> messages;
        private List<Guid> users;

        public int getLastId()
        {
            List<Topic> topics = retrieveTopics();
            if (topics.Count == 0)
                return 0;
            
            return topics[topics.Count - 1].id;

        }


        public void displayMessages()
        {


                foreach (TopicMessage msg in messages)
                {
                    Console.WriteLine(msg.Creator + " : " + msg.Text + " | sent date : " + msg.When);
                }




        }

        public Topic()
        {
        }

        public Topic(Guid creator, string title, string content, DateTime when)
        {
            this.creator = creator;
            this.title = title;
            this.content = content;
            this.when = when;
            id = getLastId() + 1;
            users = new List<Guid>();
            messages = new List<TopicMessage>();
        }

        public void addUser(Guid id)
        {
            users.Add(id);
        }

        public void addMessage(TopicMessage message)
        {
            this.messages.Add(message);

        }

        public static List<Topic> retrieveTopics()
        {
            try { 
            FileStream fs = new FileStream("topics.bin", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            List<Topic> topics = (List<Topic>)bf.Deserialize(fs);
            fs.Close();
                return topics;
            }
            catch (FileNotFoundException e)
            {
                return new List<Topic>();
            }

            

        }

        public static void saveTopics(List<Topic> topics)
        {
            FileStream fs = new FileStream("topics.bin",
            FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, topics);
            fs.Close();
        }



        public Topic(SerializationInfo info, StreamingContext ctxt)
        {
            when = (DateTime)info.GetValue("when", typeof(DateTime));
            creator = (Guid)info.GetValue("creator", typeof(Guid));
            title = (string)info.GetValue("title", typeof(string));
            content = (string)info.GetValue("content", typeof(string));
            messages = (List<TopicMessage>)info.GetValue("content", typeof(TopicMessage));
            id = (int)info.GetValue("id", typeof(int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("when", when);
            info.AddValue("creator", creator);
            info.AddValue("content", content);
            info.AddValue("title", title);
            info.AddValue("id", id);
            info.AddValue("messages", messages);
            //throw new NotImplementedException();
        }

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Content { get => content; set => content = value; }
        public DateTime When { get => when; set => when = value; }
        public Guid Creator { get => creator; set => creator = value; }
        public List<TopicMessage> Messages { get => messages; set => messages = value; }
    }
}
