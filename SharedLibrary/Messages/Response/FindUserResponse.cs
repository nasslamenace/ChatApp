using System;
using System.Collections.Generic;

namespace SharedLibrary.Messages.Response
{
    [Serializable]
    public class FindUserResponse : Message
    {
        bool hasError;
        string error;
        List<PrivateMessage> messages;
        User sender;
        User receiver;

        public FindUserResponse(string error)
        {
            this.error = error;
            hasError = true;
        }

        public FindUserResponse(List<PrivateMessage> messages, User sender, User receiver)
        {
            this.messages = messages;
            this.sender = sender;
            this.receiver = receiver;
            hasError = false;
        }


        public void displayChat()
        {
            messages.Sort();

            Console.WriteLine("------CHAT WITH " + receiver.Username + " ------------");
            foreach(PrivateMessage m in messages)
            {
                if(m.Sender == sender.Id)
                {
                    Console.WriteLine("Me : " + m.Text + " | sent : " + m.When);
                }
                else
                {
                    Console.WriteLine(receiver.Username + " : " + m.Text + " | sent : " + m.When);
                }
            }

        }

        public bool HasError { get => hasError; set => hasError = value; }
        public string Error { get => error; set => error = value; }
        public List<PrivateMessage> Messages { get => messages; set => messages = value; }
        public User Sender { get => sender; set => sender = value; }
        public User Receiver { get => receiver; set => receiver = value; }
    }
}
