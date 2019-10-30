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

namespace WMP_A05
{
    /// <summary>
    /// Interaction logic for messenger.xaml
    /// </summary>
    public partial class messenger : Window
    {
        private string user;
        public string User
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

        public void waitForMessage()
        {

        }
    }
}
