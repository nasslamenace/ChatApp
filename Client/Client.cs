using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SharedLibrary;
using SharedLibrary.Messages;
using SharedLibrary.Messages.Request;
using SharedLibrary.Messages.Response;

namespace Client
{
    public class Client
    {
        private string hostname;
        private int port;
        private Status status;
        private Guid id;
        private User currentUser;

        public Client(string h, int p)
        {
            hostname = h;
            port = p;
            status = Status.Disconnected;
        }




        public void start()
        {

            string username;
            string password;

            int choix = 0;
            Boolean logged = false;


            Console.WriteLine("Connection established");
            while (true)
            {
                TcpClient comm = new TcpClient(hostname, port);
                do
                {

                    do
                    {
                        Console.WriteLine("1- Login\n2-Register ");
                        choix = int.Parse(Console.ReadLine());
                    } while (choix != 1 && choix != 2);

                    switch (choix)
                    {
                        case 1:
                            Console.WriteLine("\n\n-----------LOGIN---------");
                            Console.WriteLine("Username pls : ");
                            username = Console.ReadLine();
                            Console.WriteLine("Password pls : ");
                            password = Console.ReadLine();


                            Net.sendMsg(comm.GetStream(), new LoginRequest(username, password));

                            LoginResponse loginResponse = (LoginResponse)Net.rcvMsg(comm.GetStream());
                            logged = loginResponse.IsLogged;
                            id = loginResponse.Id;

                            if (!logged)
                            {
                                Console.WriteLine("Wrong credentials !!");
                            }
                            else
                            {
                                Console.WriteLine("you've been logged");
                                status = Status.Logged;
                                this.currentUser = loginResponse.User;
                            }
                            break;
                        case 2:

                            Console.WriteLine("\n\n-----------Register---------");
                            Console.WriteLine("Username pls : ");
                            username = Console.ReadLine();

                            bool isSame = false;

                            do
                            {
                                Console.WriteLine("Password pls : ");
                                password = Console.ReadLine();
                                Console.WriteLine("Repeat Password pls : ");
                                string repatPassword = Console.ReadLine();

                                if (password != repatPassword)
                                    Console.WriteLine("Please enter the same password !!!");
                                else
                                    isSame = true;
                            } while (!isSame);
                            Net.sendMsg(comm.GetStream(), new RegisterRequest(username, password));
                            RegisterResponse response = (RegisterResponse)Net.rcvMsg(comm.GetStream());

                            if (response.HasError)
                                Console.WriteLine(response.Error);
                            else
                            {
                                Console.WriteLine("you have been registered as " + response.Username);
                            }

                            break;
                    }

                } while (status == Status.Disconnected);

                do
                {

                    Console.WriteLine("1- List topics \n2- create topic\n3- join a topic\n4- send private message\n");
                    Console.Write("--> ");
                    try
                    {
                        choix = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        choix = 0;
                    }


                    switch (choix)
                    {
                        case 1:
                            Net.sendMsg(comm.GetStream(), new ListTopicRequest());
                            ListTopicResponse response = (ListTopicResponse)Net.rcvMsg(comm.GetStream());
                            response.display();
                            Console.WriteLine("\n");
                            break;
                        case 2:
                            Console.WriteLine("Please write down the topic title : ");
                            Console.Write("--> ");
                            string title = Console.ReadLine();
                            Console.WriteLine("Please write down a description for the topic : ");
                            string content = Console.ReadLine();

                            

                            Net.sendMsg(comm.GetStream(), new CreateRequest(id, title, content, DateTime.Now));

                            CreateResponse createResponse = (CreateResponse)Net.rcvMsg(comm.GetStream());

                            if (createResponse.HasError)
                                Console.WriteLine(createResponse.Error);
                            else
                            {
                                Console.WriteLine("Your topic have been created, you can check it out by listing the topics");
                            }
                            break;
                        case 3:
                            Console.WriteLine("Please choose the number of the topic from the list : ");
                            Console.Write("--> ");
                            int topicNumber;
                            try
                            {
                                topicNumber = int.Parse(Console.ReadLine());
                            }
                            catch
                            {
                                topicNumber = 0;
                            }

                            Net.sendMsg(comm.GetStream(), new JoinTopicRequest(topicNumber));

                            JoinTopicResponse joinResponse = (JoinTopicResponse)Net.rcvMsg(comm.GetStream());

                            if (joinResponse.HasError)
                                Console.WriteLine(joinResponse.Error);
                            else
                            {
                                joinResponse.display();

                                bool continuer = true;

                                do
                                {


                                    Console.WriteLine("send a message ? (y/n)");
                                    ConsoleKeyInfo input = Console.ReadKey(true);

                                    if (input.Key == ConsoleKey.N)
                                    {
                                        continuer = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Please, write your message : ");
                                        Console.Write("-->  ");
                                        string message = Console.ReadLine();

                                        TopicMessage topicMsg = new TopicMessage(DateTime.Now, currentUser.Username, message, topicNumber);

                                        Net.sendMsg(comm.GetStream(), new SendTopicMsgRequest(topicMsg));
                                        SendTopicMsgResponse msgResponse = (SendTopicMsgResponse)Net.rcvMsg(comm.GetStream());

                                        if (!msgResponse.HasError)
                                        {

                                            joinResponse.Topic.addMessage(topicMsg);

                                            joinResponse.Topic.displayMessages();
                                        }
                                        else
                                            Console.WriteLine("Error : " + msgResponse.Error);

                                    }

                                } while (continuer);
                            }
                            break;
                        case 4:
                            Console.WriteLine("Who do you want to send the message to :");
                            Console.Write("-->  ");
                            String receiver = Console.ReadLine();

                            if (receiver == currentUser.Username)
                            {
                                Console.WriteLine("\n\nYOU CAN'T SEND A MESSAGE TO YOURSELF !!\n\n");
                            }
                            else
                            {

                                Net.sendMsg(comm.GetStream(), new FindUserRequest(currentUser.Id, receiver));
                                FindUserResponse findResponse = (FindUserResponse)Net.rcvMsg(comm.GetStream());


                                if (!findResponse.HasError)
                                {
                                    findResponse.displayChat();

                                    bool continuer = true;

                                    do
                                    {


                                        Console.WriteLine("send a message ? (y/n)");
                                        ConsoleKeyInfo input = Console.ReadKey(true);

                                        if (input.Key == ConsoleKey.N)
                                        {
                                            continuer = false;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Please, write your message : ");
                                            Console.Write("-->  ");
                                            string message = Console.ReadLine();

                                            PrivateMessage privateMsg = new PrivateMessage(DateTime.Now, currentUser.Id, findResponse.Receiver.Id, message);

                                            Net.sendMsg(comm.GetStream(), new SendPrivateRequest(privateMsg));
                                            SendPrivateResponse msgResponse = (SendPrivateResponse)Net.rcvMsg(comm.GetStream());

                                            if (!msgResponse.HasError)
                                            {

                                                findResponse.Messages.Add(privateMsg);

                                                findResponse.displayChat();
                                            }
                                            else
                                                Console.WriteLine("Error : " + msgResponse.Error);

                                        }

                                    } while (continuer);

                                }
                                else
                                    Console.WriteLine("Error : " + findResponse.Error);
                            }
                            break;
                        default:
                            Console.WriteLine("make a wise choice mate");
                            break;
                    }

                } while (status == Status.Logged);

                //Net.sendMsg(comm.GetStream(), new Expr(op1, op2, op));
                //Console.WriteLine("Result = " + (Result)Net.rcvMsg(comm.GetStream()));

            }
        }
    }
}
