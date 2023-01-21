using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UDPClient;

class Program
{
    public static void Main()
    {
        ClientClass clientCurrent = new ClientClass();
        

        if (clientCurrent.Connected)
        {
            //sendWindowThread.Start();
            //receiveWindowThread.Start();
            //windowMoveThread.Start();

            //sendThread.Start();
            //receiveThread.Start();
            //clientSend.Start();
        }

    }
}
