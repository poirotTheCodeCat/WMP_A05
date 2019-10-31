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
        private static Thread serverMessage;

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
        }

        public void Send_Click(object sender, RoutedEventArgs arg)
        {

        }

        /*
         * Function : connect_Button()
         * Parameters : object sender, RoutedEventArgs arg
         * Description :  This connects the client to the server and reacts to the button press
         * Returns : Nothing
         */
        public void connect_Button(object sender, RoutedEventArgs arg)
        {
            string address = "140.0.0.1";
            Int32 port = 15000;
            chatClient = new TcpClient(address, port);    // connect to the server
            try    // make sure that the connection succeeded
            { 
                // we want to set up a thread to wait for a return message
                ThreadStart waitThread = new ThreadStart(messageWait);
                serverMessage = new Thread(waitThread);
                serverMessage.Start();      // start a thread that waits for an incoming message

                // change the color of the connected ellipse
                SolidColorBrush connectColor = new SolidColorBrush();
                connectColor.Color = Color.FromRgb(0, 255, 0);
                connectedElipse.Fill = connectColor;

                connectButton.IsEnabled = false;        // disable the connect button
                disconnectButton.IsEnabled = true;      // enable the disconnect button
                sendButton.IsEnabled = true;            // enable the send button 
            }
            catch(SocketException e)
            {
                connectionError.Text = "There was an error connecting to the server";
            }
            catch(ArgumentNullException n)
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
            SolidColorBrush connectColor = new SolidColorBrush();
            connectColor.Color = Color.FromRgb(255, 0, 0);
            connectedElipse.Fill = connectColor;

            ChatClient.Close();     // close the connection to the server
            connectionError.Text = "Disconnected from the server";
        }

        public static void messageWait()
        {   
            Byte[] bytes = new byte[256];       // bytes will be used to read data
            String data = null;                 // this string will be used to read data

            data = null;

            NetworkStream serverStream = chatClient.GetStream();      // used to recieve message
            int i;

            while ((i = serverStream.Read(bytes, 0, bytes.Length)) != 0) // iterate through read stream
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);   // convert bytes recieved to a string
            }
        }
    }
}
