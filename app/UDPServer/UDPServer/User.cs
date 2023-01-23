using System;
using System.Net;
using System.Net.Sockets;

namespace UDPServer
{
    /// <summary>
    /// Class representing user on server side
    /// Contains endpoints of itself and its receiver
    /// </summary>
	public class User
	{
        private IPEndPoint clientDestination, receiverDestination;
        private bool connected;

        /// <summary>
        /// Constructor that sets a client endpoint and receiver endpoint 
        /// </summary>
        /// <param name="endpoint"> Given endpoint of a user</param>
        public User(IPEndPoint endpoint)
		{
            clientDestination = endpoint;
            receiverDestination = endpoint;
            //
            connected = true;
        }

        public IPEndPoint ClientDestination
        {
            get { return clientDestination; }
        }

        public IPEndPoint ReceiverDestination
        {
            get { return receiverDestination; }
        }

    }

}

