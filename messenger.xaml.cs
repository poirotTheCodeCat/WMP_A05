﻿using System;
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


namespace WMP_A05
{
    /// <summary>
    /// Interaction logic for messenger.xaml
    /// </summary>
    public partial class messenger : Window
    {
        private string user;
        private TcpClient client;

        public TcpClient Client         // accessor and modifier of client data member
        {
            get { return client; }
            set { client = value; }
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

        }
        /*
         * Function : disconnect_Button()
         * Parameters : object sender, RoutedEventArgs arg
         * Description :  This disconnects the client from the server
         * Returns : Nothing
         */
        public void disconnect_Button(object sender, RoutedEventArgs arg)
        {

        }

        public void waitForMessage()
        {

        }
    }
}