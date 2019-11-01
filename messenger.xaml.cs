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
                ChatStream = chatClient.GetStream();
                Byte[] sendBytes = new Byte[256];

                sendBytes = System.Text.Encoding.ASCII.GetBytes(messageSend);
                ChatStream.Write(sendBytes, 0, sendBytes.Length);

                chatText.Text = "";
                string chatMessage = "me: " + message;
                // create a listBox item
                chatBox.Items.Add(chatMessage);
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
                chatClient = new TcpClient(address, port);    // connect to the server
                chatStream = chatClient.GetStream();

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
                connectionError.Text = "There was an error connecting to the server";
            }
            catch (ArgumentNullException n)
            {
                connectionError.Text = "There was an error connecting to the server";
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
            isConnected = false;
            SolidColorBrush connectColor = new SolidColorBrush();
            connectColor.Color = Color.FromRgb(255, 0, 0);
            connectedElipse.Fill = connectColor;

            ChatClient.Close();     // close the connection to the server
            chatStream.Close();
            connectionError.Text = "Disconnected from the server";      // inform the user that they are disconnected 

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
            while (isConnected)
            {
                Byte[] bytes = new byte[256];
                try
                {
                    string message = String.Empty;
                    chatStream.Read(bytes, 0, bytes.Length);
                    message = System.Text.Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                    //chatBox.Items.Add(message)
                    this.Dispatcher.Invoke(() =>
                    {
                        this.chatBox.Items.Add(message);
                    });
                }
                catch(NullReferenceException e)
                {
                    connectionError.Text = "Unable to read incoming messages";
                }
            }

        }
    }
}
