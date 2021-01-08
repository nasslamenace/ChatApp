using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SharedLibrary
{
    [Serializable]
    public class User
    {
        private Guid id;
        private string username;
        private string password;

        public Guid Id { get => id; set => id = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        public User(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.id = Guid.NewGuid();
        }

        public User()
        {
        }

        public static List<User> retrieveUsers()
        {
            try { 
            FileStream fs = new FileStream("users.bin", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            List<User> messages = (List<User>)bf.Deserialize(fs);
            fs.Close();

            return messages;
            }
            catch (FileNotFoundException e)
            {
                return new List<User>();
            }

        }

        public static void saveUsers(List<User> users)
        {
            FileStream fs = new FileStream("users.bin",
            FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, users);
            fs.Close();
            Console.WriteLine("we are registered !!!");
        }
        

        public User(SerializationInfo info, StreamingContext ctxt)
        {
            username = (string)info.GetValue("username", typeof(string));
            password = (string)info.GetValue("password", typeof(string));
            id = (Guid)info.GetValue("id", typeof(Guid));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("username", username);
            info.AddValue("password", password);
            info.AddValue("id", id);
            //throw new NotImplementedException();
        }
    }
}
