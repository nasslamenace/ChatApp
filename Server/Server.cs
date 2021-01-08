using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SharedLibrary;
using SharedLibrary.Messages;
using SharedLibrary.Messages.Request;
using SharedLibrary.Messages.Response;

namespace Server
{
    public class Server
    {
        private int port;
        private TcpListener ear;
        private TcpClient cli_sock;

        public Server(int port)
        {
            this.port = port;
        }

        public int Port { get => port; set => port = value; }

        public void start()
        {
			Socket server = new Socket(AddressFamily.InterNetwork,
														 SocketType.Stream,
										ProtocolType.Tcp);
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
			server.Bind(endpoint);

			server.Listen(10);
			Console.WriteLine("Waiting for clients on port " + port);

			while (true)
			{
				try
				{

					Socket client = server.Accept();
					ConnectionHandler handler = new ConnectionHandler(client);

					Thread thread = new Thread(new ThreadStart(handler.HandleConnection));
					thread.Start();
				}
				catch (Exception)
				{
					Console.WriteLine("Connection failed on port " + port);
				}
			}


		}


		class ConnectionHandler
		{

			private Socket client;
			private NetworkStream ns;

			private static int connections = 0;

			public ConnectionHandler(Socket client)
			{
				this.client = client;
			}


			public void HandleConnection()
			{
				try
				{
					ns = new NetworkStream(client);
					connections++;
					Console.WriteLine("New client accepted: {0} active connections", connections);

					Console.WriteLine("Welcome to my server");
					while (true)
					{
						Message msg = Net.rcvMsg(ns);

						Type type = msg.GetType();

						if (type == typeof(RegisterRequest))
						{
							Net.sendMsg(ns, handleRegisterRequest(msg as RegisterRequest));
						}
						else if(type == typeof(LoginRequest))
                        {
							Net.sendMsg(ns, handleLoginRequest(msg as LoginRequest));
						}
						else if(type == typeof(ListTopicRequest))
                        {
							Net.sendMsg(ns, handleListTopicRequest(msg as ListTopicRequest));
						}
						else if (type == typeof(CreateRequest))
						{
							Net.sendMsg(ns, handleCreateRequest(msg as CreateRequest));
						}
						else if (type == typeof(JoinTopicRequest))
						{
							Net.sendMsg(ns, handleJoinRequest(msg as JoinTopicRequest));
						}
						else if (type == typeof(SendTopicMsgRequest))
						{
							Net.sendMsg(ns, handleSendTopicMsgRequest(msg as SendTopicMsgRequest));
						}
						else if (type == typeof(FindUserRequest))
						{
							
							Net.sendMsg(ns, handleFindUserRequest(msg as FindUserRequest));
						}
						else if (type == typeof(SendPrivateRequest))
						{
							Net.sendMsg(ns, handleSendPrivateRequest(msg as SendPrivateRequest));
						}
					}
					ns.Close();
					client.Close();
					connections--;
					Console.WriteLine("Client disconnected: {0} active connections", connections);
				}
				catch (Exception)
				{
					connections--;
					Console.WriteLine("Client disconnected: {0} active connections", connections);
				}
			}

			public LoginResponse handleLoginRequest(LoginRequest msg)
            {
				List<User> users = User.retrieveUsers();


				foreach (User user in users)
				{
					if (user.Username == msg.Username && msg.Password == user.Password)
						return new LoginResponse(true, user);
				}

				return new LoginResponse(false);

			}
			public RegisterResponse handleRegisterRequest(RegisterRequest msg)
			{
				string error = "";
				List<User> users = User.retrieveUsers();

				foreach (User user in users)
				{
					if (user.Username == msg.Username)
					{
						error += msg.Username + " already exist ! choose another username pls";
						return new RegisterResponse(error);
					}
				}

				users.Add(new User(msg.Username, msg.Password));
				User.saveUsers(users);
				return new RegisterResponse(msg.Username, msg.Password);

			}

			public ListTopicResponse handleListTopicRequest(ListTopicRequest msg)
			{
				return new ListTopicResponse(Topic.retrieveTopics());
			}

			public CreateResponse handleCreateRequest(CreateRequest msg)
			{
				List<Topic> topics = Topic.retrieveTopics();

				Console.WriteLine("nass");

				topics.Add(new Topic(msg.Id, msg.Title, msg.Content, msg.When));

                

				Topic.saveTopics(topics);

				return new CreateResponse();
			}

			public JoinTopicResponse handleJoinRequest(JoinTopicRequest msg)
            {
				List<Topic> topics = Topic.retrieveTopics();

				foreach(Topic topic in topics)
                {
					Console.WriteLine(topic.Id + "  " + msg.Id);
					if (topic.Id == msg.Id)
						return new JoinTopicResponse(topic);
                }
				return new JoinTopicResponse("This topic has not been found ! try another one ");
			}

			public SendTopicMsgResponse handleSendTopicMsgRequest(SendTopicMsgRequest msg)
			{
				List<Topic> topics = Topic.retrieveTopics();

				foreach(Topic t in topics)
                {
					if (t.Id == msg.Message.TopicId)
						t.addMessage(msg.Message);
                }

				Topic.saveTopics(topics);

				return new SendTopicMsgResponse();

			}

			public FindUserResponse handleFindUserRequest(FindUserRequest msg)
			{

				
				List<User> users = User.retrieveUsers();

				User senderUser = new User();
				User receiverUser = new User();

				Guid receiver = Guid.NewGuid();

				bool found = false;

				foreach(User u in users)
                {
					if(u.Username == msg.Username)
                    {
						receiver = u.Id;
						found = true;
						receiverUser = u;
                    }
					if (u.Id == msg.Sender)
						senderUser = u;
                }



				if (!found)
					return new FindUserResponse("This user has not been found !!");

				List<PrivateMessage> messages = PrivateMessage.retrieveMessages();

				List<PrivateMessage> userMessages = new List<PrivateMessage>();

				
				
				foreach (PrivateMessage m in messages)
				{
					if((msg.Sender == m.Sender && receiver == m.Receiver) || (receiver == m.Sender && msg.Sender == m.Receiver))
                    {
						userMessages.Add(m);
                    }
				}


				return new FindUserResponse(userMessages, senderUser, receiverUser);

			}

			SendPrivateResponse handleSendPrivateRequest(SendPrivateRequest msg)
            {
				List<PrivateMessage> messages = PrivateMessage.retrieveMessages();

				messages.Add(msg.Message);

				PrivateMessage.saveMessages(messages);

				return new SendPrivateResponse();
            }

		}

	}
}
