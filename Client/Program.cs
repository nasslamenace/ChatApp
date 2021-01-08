using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client myClient = new Client("127.0.0.1", 9001);
            myClient.start();
            
        }
    }
}
