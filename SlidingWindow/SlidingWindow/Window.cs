﻿using System.Net.Sockets;
using System.Text;
using System.Configuration;

namespace SlidingWindow;
public static class Window
{
    static int windowSize = int.Parse(ConfigurationManager.AppSettings["windowSize"]);
    static int currentPos = 0;
    static int packetLen = 0;

    static int packagePartLenght = 2;

    public static async void WindowSend(UdpClient client)
    {
        string input = "PAVEL KUBAT ZMRE";
            //await Task.Run(() => Console.ReadLine());
        byte[] windowSend = Encoding.UTF8.GetBytes(input);
        //while (currentPos <= windowSend.Length)
        //{
        int currentByteLen = 0;
        for (int i = currentPos; i < windowSize; i++)
        {
            int intervalLength = (currentByteLen + packagePartLenght) - (currentByteLen);
            byte[] interval = new byte[intervalLength];

            Array.Copy(windowSend, currentByteLen, interval, 0, intervalLength);
            Console.WriteLine(Encoding.UTF8.GetString(interval));

            try
            {
                await client.SendAsync(interval, interval.Length);
                currentByteLen += packagePartLenght;
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
                client.Close();
                break;
            }
            catch (ObjectDisposedException e)
            {
                Console.WriteLine(e.ToString());
                client.Close();
                break;
            }
        }
        //}
    }

}

public enum PackageType
{
    Message,
    Login,
    Logout,

}

