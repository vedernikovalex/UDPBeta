using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Configuration;


namespace SlidingWindow
{
    public class Window
    {
        public int windowSize, packetSize, windowStart, windowEnd;

        public UdpClient senderClient, receiverClient;
        public IPEndPoint senderDestination, receiverDestination;

        public byte[] buffer, config;

        //public int currentPosition;

        public Window(UdpClient sender, UdpClient receiver)
        {
            senderClient = sender;
            receiverClient = receiver;
            try
            {
                windowSize = 4;
                //Int32.Parse(ConfigurationManager.AppSettings.Get("windowSize"));
                packetSize = 4;
                             //Int32.Parse(ConfigurationManager.AppSettings.Get("packetSize"));
            }
            catch (FormatException e)
            {
                Console.WriteLine("!! Configuration file was not found or incorrectly formated !!");
            }
        }

        public void WindowSender(string message, IPEndPoint receiver)
        {
            windowStart = 0;
            windowEnd = windowSize - 1;
            bool ackAllData = false;

            byte[] byteMessage = Encoding.UTF8.GetBytes(message);

            //stackoverflow.com/questions/21868956/how-to-obtain-the-actual-packet-size-byte-array-in-java-udps
            //legth of message to be exact as as packet size 
            byte[] buffer = new byte[byteMessage.Length + packetSize - (byteMessage.Length % packetSize)];
            Array.Copy(byteMessage, buffer, byteMessage.Length);
            int packetCount = (int)Math.Ceiling((double)buffer.Length / packetSize);

            Thread senderThread = new Thread(() =>
            {
                try
                {
                    int configState = 0;
                    config = BitConverter.GetBytes(configState);
                    senderClient.Send(config, config.Length);
                    Console.WriteLine(BitConverter.ToInt32(config));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                while (!ackAllData)
                {
                    for (int i = windowStart; i <= windowEnd; i++)
                    {
                        if (i < packetCount)
                        {
                            byte[] currentPacket = PacketDivider(packetSize, i, buffer);
                            try
                            {
                                senderClient.Send(currentPacket, currentPacket.Length);
                                Console.WriteLine("Packet was sent: " + Encoding.UTF8.GetString(currentPacket) + " | " + i);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                }
                try
                {
                    int configState = 1;
                    config = BitConverter.GetBytes(configState);
                    senderClient.Send(config, config.Length);
                    Console.WriteLine(BitConverter.ToInt32(config));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            });

            Thread moverThread = new Thread(() =>
            {
                while (!ackAllData)
                {
                    try
                    {
                        byte[] moverBuffer = senderClient.Receive(ref receiver);
                        int positionAck = BitConverter.ToInt32(moverBuffer, 0);
                        Console.WriteLine("received position of: " + positionAck);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            });

            senderThread.Start();
        }

        public void WindowReceiver()
        {
            Dictionary<int, byte[]> packets = new Dictionary<int, byte[]>();
            bool allReceived = false, packetStartReceived = false;
            byte[] data;
            Thread listenerThread = new Thread(() =>
            {
                while (!allReceived)
                {
                    Console.WriteLine("invoke");
                    try
                    {
                        data = receiverClient.Receive(ref senderDestination);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.ToString());
                        break;
                    }
                    if(data.Length == 1)
                    {
                        if (BitConverter.ToInt32(data) == 0)
                        {
                            packetStartReceived = true;
                        }
                        else if(BitConverter.ToInt32(data) == 1)
                        {
                            allReceived = true;
                            break;
                        }
                    }
                    if(data.Length == windowSize + 1)
                    {
                        string packetPart = Encoding.UTF8.GetString(data);
                        int packetNumber = Int32.Parse(packetPart[0].ToString());
                        Console.WriteLine(packetPart);
                    }
                }
            });

            listenerThread.Start();
        }

        public static byte[] PacketDivider(int packetSize, int currentPosition, byte[] message)
        {
            byte[] currentPositionBytes = BitConverter.GetBytes(currentPosition);
            byte[] packet = new byte[currentPositionBytes.Length + packetSize];

            //position in bytes
            int curerntByteInterval = packetSize * currentPosition;
            Array.Copy(BitConverter.GetBytes(currentPosition), packet, 1);
            Array.Copy(message, curerntByteInterval, packet, currentPositionBytes.Length, packetSize);
            return packet;
        }

    }
}
