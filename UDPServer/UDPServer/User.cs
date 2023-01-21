using System;
using System.Net;
using System.Net.Sockets;

namespace UDPServer
{
	public class User
	{
        private IPEndPoint clientDestination, receiverDestination;
        private bool connected;

        public User(IPEndPoint endpoint)
		{
            clientDestination = endpoint;
        }

    }
}

