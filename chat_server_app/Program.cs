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
        int hello;
        private static Dictionary<IPAddress, TcpClient> clientList = new Dictionary<IPAddress, TcpClient>();       // keeps track of the clients that are connected
        private static Dictionary<IPAddress, Thread> threadList = new Dictionary<IPAddress, Thread>();             // keeps track of the threads running

        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // set up the tcpListener on port15000
                Int32 port = 15000;
                IPAddress localIP = IPAddress.Parse("140.0.0.1");

                IPEndPoint clientIP;            // used to store the user's IP Address

                server = new TcpListener(localIP, port);    // set up server to listen for incoming connections
                server.Start();     // start listening on the server
                while (true)
                {
                    Console.WriteLine("Waiting to connect chat...");
                    TcpClient client = server.AcceptTcpClient();    // accept an incoming connection
                    Console.WriteLine("Connected to a chat...");

                    ParameterizedThreadStart startThread = new ParameterizedThreadStart(waitForMessage);    // declare a delegate pointing at waitForMessage()
                    Thread waitThread = new Thread(startThread);        // create a thread that will wait for an incoming message

                    clientIP = client.Client.LocalEndPoint as IPEndPoint;       // get the client's IP address

                    clientList.Add(clientIP.Address, client);       // add the client to the clientList
                    threadList.Add(clientIP.Address, waitThread);   // add the thread to the threadList

                    waitThread.Start();     // start the thread
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket Exception: {0}", e);
            }
            finally
            {
                server.Stop();
            }
        }

        static void waitForMessage(object o)
        {

        }

        static void sendMessages()
        {

        }
    }
}
