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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;


namespace WMP_A05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string user;
        public string User
        {
            get { return user; }        // gets the value of user
            set { user = value; }       // sets the value of user
        }
        public MainWindow()
        {
            InitializeComponent();

        }

        public void submit_Click(object sender, RoutedEventArgs arg)
        {
            user = username.Text;
            if(user.Trim() == "")
            {
                errorUsername.Text = "please enter a username ";
                return;
            }
            messenger newMessenger = new messenger();
            newMessenger.User = user;
            newMessenger.Show();
            this.Close();
        }

        public void waitForMessage()
        {

        }

    }
}
