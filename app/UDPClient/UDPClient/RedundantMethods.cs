using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClient
{
    /// <summary>
    /// Class to reduce redundant statements by using premade methods
    /// </summary>
    internal class RedundantMethods
    {
        /// <summary>
        /// UserAlert method to pring a message to a client 
        /// Accepts from 1 to 3 strings to print
        /// </summary>
        /// <param name="message"> Message(es) to print </param>
        public static void UserAlert(string message)
        {
            Console.WriteLine("");
            Console.WriteLine("!! " + message + " !!");
            Console.WriteLine("");
        }

        public static void UserAlert(string message, string message2)
        {
            Console.WriteLine("");
            Console.WriteLine("!! " + message + " !!");
            Console.WriteLine("!! " + message2 + " !!");
            Console.WriteLine("");
        }
        public static void UserAlert(string message, string message2, string message3)
        {
            Console.WriteLine("");
            Console.WriteLine("!! " + message + " !!");
            Console.WriteLine("!! " + message2 + " !!");
            Console.WriteLine("!! " + message3 + " !!");
            Console.WriteLine("");
        }

        /// <summary>
        /// UserInput method to receive user input
        /// Prints identifier for an input
        /// Takes input as a string
        /// </summary>
        /// <returns> User input as string </returns>
        public static string UserInput()
        {
            Console.Write(" >> ");
            string input = Console.ReadLine().ToLower().Trim();
            return input;
        }

        /// <summary>
        /// UserInput method to receive user input
        /// Prints identifier for an input
        /// Takes input as a string
        /// Prints a request for an input 
        /// </summary>
        /// <param name="message"> Request for an input </param>
        /// <returns> User input as string </returns>
        public static string UserInput(string message)
        {
            Console.WriteLine("?? " + message + " ??");
            Console.Write(" >> ");
            string input = Console.ReadLine().ToLower().Trim();
            return input;
        }

        /// <summary>
        /// Prints error exception message and custom message in specified format
        /// </summary>
        /// <param name="errorMessage"> exception message </param>
        /// <param name="customMessage"> custom message</param>
        public static void UserExceptionError(string errorMessage, string customMessage)
        {
            Console.WriteLine("==!! ERROR !!==");
            Console.WriteLine("!! " + errorMessage + " !!");
            Console.WriteLine("!! " + customMessage + " !!");
            Console.WriteLine("==!!!?????!!!==");
        }

    }
}
