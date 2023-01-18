using System;
using System.Net;
using System.Net.Sockets;

namespace UDPServer
{
	public class User
	{
        private IPEndPoint clientEndPoint;

        public User(IPEndPoint cl)
		{
            clientEndPoint = cl;
        }

        public IPEndPoint ClientEndPoint
        {
            get { return clientEndPoint; }
            set { clientEndPoint = value; }
        }

        public User GetUserByClient(IPEndPoint endPoint)
        {
            if (endPoint == clientEndPoint)
            {
                Console.WriteLine("FOUND" + this);
                return this;
            }
            return null;
        }

        public override string? ToString()
        {
            return "User with endpoint: " + clientEndPoint.ToString();
        }
    }
}

