/*
 * File: messenger.xaml.cs
 * Programmer : Daniel Grew and Sasha Malesevic
 * Date Last Modified : 2019-11-01
 * Description: This contains the logic for the messenger window which will send and recieve messages from a server 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Threading;

namespace WMP_A05
{
    /// <summary>
    /// Interaction logic for messenger.xaml
    /// </summary>
    public partial class messenger : Window
    {

        private static string user;
        private static TcpClient chatClient;
        private static NetworkStream chatStream;
        private static Thread serverMessage;
        private static bool isConnected;


        public NetworkStream ChatStream
        {
            get { return chatStream; }
            set { chatStream = value; }
        }
        public TcpClient ChatClient         // accessor and modifier of client data member
        {
            get { return chatClient; }
            set { chatClient = value; }
        }

        public string User                 // accessor and modifier of user data member
        {
            get { return user; }        // gets the value of user
            set { user = value; }       // sets the value of user
        }
        public messenger()
        {
            InitializeComponent();

            isConnected = false;        // the client is not connected yet, so we initialize this to false
        }

        public void Send_Click(object sender, RoutedEventArgs arg)
        {
            string message = chatText.Text;
            string messageSend = User + ": " + message;
            if (message.Length <= 250)
            {
                ChatStream = chatClient.GetStream();        // connect to stream
                Byte[] sendBytes = new Byte[256];

                sendBytes = System.Text.Encoding.ASCII.GetBytes(messageSend);
                ChatStream.Write(sendBytes, 0, sendBytes.Length);       // send message

                chatText.Text = "";
                string chatMessage = "me: " + message;
                // create a listBox item
                chatBox.Items.Add(chatMessage);         // add chat to screen

                chatStream.Flush();
            }
            else
            {
                textError.Text = "You cannot exceed 250 characters";
            }
        }

        /*
         * Function : connect_Button()
         * Parameters : object sender, RoutedEventArgs arg
         * Description :  This connects the client to the server and reacts to the button press
         * Returns : Nothing
         */
        public void connect_Button(object sender, RoutedEventArgs arg)
        {
            connectionError.Text = "";
            isConnected = true;
            string address = "127.0.0.1";
            Int32 port = 15000;

            try    // make sure that the connection succeeded
            {
                chatBox.Items.Clear();
                chatClient = new TcpClient(address, port);    // connect to the server
                chatStream = chatClient.GetStream();
                chatBox.Items.Add("- Connected to the server -");      // inform the user that they are disconnected 
                // change the color of the connected ellipse
                SolidColorBrush connectColor = new SolidColorBrush();
                connectColor.Color = Color.FromRgb(0, 255, 0);
                connectedElipse.Fill = connectColor;

                connectButton.IsEnabled = false;        // disable the connect button
                disconnectButton.IsEnabled = true;      // enable the disconnect button
                sendButton.IsEnabled = true;            // enable the send button 

                // we want to set up a thread to wait for a return message
                ThreadStart waitThread = new ThreadStart(messageWait);
                serverMessage = new Thread(waitThread);
                serverMessage.Start();      // start a thread that waits for an incoming message
            }
            catch (SocketException e)
            {
                chatBox.Items.Add("Socket Exception was thrown: "+ e); 
            }
            catch (ArgumentNullException n)
            {
                chatBox.Items.Add("Argument Null Exception was thrown: " + n);
            }
        }
        /*
         * Function : disconnect_Button()
         * Parameters : object sender, RoutedEventArgs arg
         * Description :  This disconnects the client from the server
         * Returns : Nothing
         */
        public void disconnect_Button(object sender, RoutedEventArgs arg)
        {
            chatBox.Items.Clear();
            isConnected = false;
            SolidColorBrush connectColor = new SolidColorBrush();
            connectColor.Color = Color.FromRgb(255, 0, 0);          // change the connection button color to indicate status of connection
            connectedElipse.Fill = connectColor;

            chatBox.Items.Add("Disconnected from the server");      // inform the user that they are disconnected 

            connectButton.IsEnabled = true;        // enable the connect button
            disconnectButton.IsEnabled = false;      // disable the disconnect button
            sendButton.IsEnabled = false;            // disable the send button 
        }

        /*
         * Function : messageWait()
         * Parameters : none
         * Description : Waits for incoming messages
         * Returns : Nothing
         */
        public void messageWait()
        {
            int i = 0;
            Byte[] bytes = new byte[256];
            try
            {
                string message = String.Empty;
                while ((i = chatStream.Read(bytes, 0, bytes.Length)) != 0)      // attempt to read incoming messages - 0 when disconnected
                {
                    message = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    //chatBox.Items.Add(message)
                    this.Dispatcher.Invoke(() =>
                    {
                        this.chatBox.Items.Add(message);        // write recieved message to screen
                    });
                }
            }
            catch( SocketException s )
            {
                connectionError.Text = "The client has been disconnected from the server";
            }
            catch (NullReferenceException e)
            {
                connectionError.Text = "Unable to read incoming messages";
            }
            finally
            {
                chatStream.Close();     // close the stream
                ChatClient.Close();     // close the connection to the server   
            }
        }

        
    }
}