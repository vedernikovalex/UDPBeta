using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClient
{
    public interface IUserInputOne
    {
        string GetInput();
        string GetInput(string message);
    }

    public interface IUserInputServerConfiguration
    {
        string GetIP(string message);
        string GetPORT(string message);
        string GetPORTClient(string message);
    }
}
