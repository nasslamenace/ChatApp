using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharedLibrary
{
    [Serializable]
    public class PrivateMessage : IComparable
    {
        private DateTime when;
        private Guid sender;
        private Guid receiver;
        private string text;


        public PrivateMessage()
        {
        }

        public PrivateMessage(DateTime when, Guid sender, Guid receiver, string text)
        {
            this.when = when;
            this.sender = sender;
            this.receiver = receiver;
            this.text = text;
        }

        public static List<PrivateMessage> retrieveMessages()
        {
            try
            {
                FileStream fs = new FileStream("messages.bin", FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                List<PrivateMessage> messages = (List<PrivateMessage>)bf.Deserialize(fs);
                fs.Close();

                return messages;
            }
            catch (FileNotFoundException e)
            {
                return new List<PrivateMessage>();
            }
        }

        public static void saveMessages(List<PrivateMessage> messages)
        {
            FileStream fs = new FileStream("messages.bin",
            FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, messages);
            fs.Close();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("when", when);
            info.AddValue("sender", sender);
            info.AddValue("receiver", receiver);
            info.AddValue("text", text);
            //throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            PrivateMessage otherMsg = obj as PrivateMessage;
            if (otherMsg != null)
                return this.when.CompareTo(otherMsg.when);
            else
                throw new ArgumentException("Object is not a PrivateMessage");
        }

        public string Text { get => text; set => text = value; }
        public Guid Receiver { get => receiver; set => receiver = value; }
        public Guid Sender { get => sender; set => sender = value; }
        public DateTime When { get => when; set => when = value; }
    }
}
