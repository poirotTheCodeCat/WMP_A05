using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace chat_server
{
    class Program
    {
        Dictionary<IPAddress, TcpClient> clientList = new Dictionary<IPAddress, TcpClient>();       // keeps track of the clients that are connected
        Dictionary<IPAddress, Thread> threadList = new Dictionary<IPAddress, Thread>();             // keeps track of the threads running
        List<string> usersList = new List<string>();

        static void Main(string[] args)
        {
        }

        static void sendMessages()
        {

        }
    }
}
