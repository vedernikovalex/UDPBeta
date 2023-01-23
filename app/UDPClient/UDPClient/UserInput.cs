using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClient
{
    public class UserInput : IUserInputOne
    {

        public string GetInput()
        {
            string input = RedundantMethods.UserInput();
            return input;
        }
        public string GetInput(string message)
        {
            string input = RedundantMethods.UserInput(message);
            return input;
        }
    }

    public class ServerConfig : IUserInputServerConfiguration
    {
        public string GetIP(string message)
        {
            string input = RedundantMethods.UserInput(message);
            return input;
        }
        public string GetPORT(string message)
        {
            string input = RedundantMethods.UserInput(message);
            return input;
        }
        public string GetPORTClient(string message)
        {
            string input = RedundantMethods.UserInput(message);
            return input;
        }
    }
}
