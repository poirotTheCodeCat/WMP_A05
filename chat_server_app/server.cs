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

        private static List<TcpClient> clientList = new List<TcpClient>();          // keeps track of the clients that are connected
        private static List<Thread> threadList = new List<Thread>();                // keeps track of the threads running


        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // set up the tcpListener on port15000
                Int32 port = 15000;
                IPAddress localIP = IPAddress.Parse("127.0.0.1");

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

                    clientList.Add(client);       // add the client to the clientList
                    threadList.Add(waitThread);   // add the thread to the threadList

                    waitThread.Start(client);     // start the thread
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Socket Exception: {0}", e);
            }
            finally
            {
                stopAllClients();       // close all clients
                server.Stop();
            }
        }

        /*
         * Function : waitForMessage()
         * Parameters : object o
         * Description : This waits for a client to send a message and calls upon another function to send 
         *              that message to all other connected users
         * Returns : Nothing
         */
        public static void waitForMessage(object o)
        {
            TcpClient client = (TcpClient)o;
            Byte[] bytes = new byte[256];       // bytes will be used to read data
            String data = null;                 // this string will be used to read data

            data = null;

            NetworkStream stream = client.GetStream();      // used to recieve message
            int i;

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0 ) // iterate through read stream
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);   // convert bytes recieved to a string
                Console.WriteLine("{0}", data);
            }
            sendMessages(client, data);
        }

        /*
         * Function : sendMessage()
         * Parameters : object c, string message
         * Description : This function sends the message to all connected clients
         * Returns : nothing
         */
        static void sendMessages(object c, string message)
        {
            TcpClient client = (TcpClient)c;
            IPEndPoint currentClient = client.Client.LocalEndPoint as IPEndPoint;
            IPEndPoint checkIP;

            IPAddress clientIP = currentClient.Address;                         // get the current client's IP Address
            byte[] sendBytes = System.Text.Encoding.ASCII.GetBytes(message);    // convert message into bytes

            NetworkStream sendStream; 
            foreach (TcpClient tcpSend in clientList)                        // cycle through the clientList to send the current message
            {
                checkIP = tcpSend.Client.LocalEndPoint as IPEndPoint;   
                if (!(checkIP.Address == clientIP))                         // dont send back to sender
                {
                    sendStream = tcpSend.GetStream();                       // connect to a stream to send message
                    sendStream.Write(sendBytes, 0, sendBytes.Length);       // send the message to all connected clients
                    Console.WriteLine("Send {0} to {1} \n", message, tcpSend.ToString());
                }
            }
        }


        /*
         * Function : stopAllClients()
         * Parameters : none
         * Description : This closes all of the connections to the clients and empties the dictionaries
         * Returns : nothing
         */
        static void stopAllClients()
        {
            foreach (TcpClient tcpSend in clientList)
            {
                tcpSend.Close();
            }
            threadList.Clear();
            clientList.Clear();
        }
    }
}
