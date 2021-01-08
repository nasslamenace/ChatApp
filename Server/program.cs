using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server serveur = new Server(9001);
            serveur.start();
        }
    }
}
